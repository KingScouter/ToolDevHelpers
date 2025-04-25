using CommonLib.Models;
using CommonLib.Utils;
using LazyCache;
using System.Windows.Input;
using Wox.Infrastructure;
using Wox.Plugin;
using Wox.Plugin.Logger;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers.QueryHandler
{
    internal sealed class BranchQueryHandler : BaseQueryHandler
    {
        private readonly CachingService _cache;

        private static readonly string remoteCacheKey = "remoteCache";
        private static readonly string localCacheKey = "localCache";

        private Dictionary<string, bool> cacheLoading = new()
        {
            {remoteCacheKey, false},
            {localCacheKey, false}
        };

        internal BranchQueryHandler()
        {
            _cache = new CachingService();
            _cache.DefaultCachePolicy.DefaultCacheDurationSeconds = (int)TimeSpan.FromMinutes(2).TotalSeconds;
        }

        /// <summary>
        /// Handle the query to select a branch
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <returns>List of query results (branches). Null if the query wasn't handled</returns>
        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            var modeQuery = query.FirstOrDefault("");

            if (string.Equals(modeQuery, "bl", StringComparison.OrdinalIgnoreCase))
                return HandleQueryIntern(query.Skip(1), config, true);
            if (string.Equals(modeQuery, "br", StringComparison.OrdinalIgnoreCase))
                return HandleQueryIntern(query.Skip(1), config, false);

            return null;
        }

        /// <summary>
        /// Load the context-menus for a selected branch.
        /// </summary>
        /// <param name="selectedResult">Query result</param>
        /// <param name="config">App configuration</param>
        /// <param name="pluginName">Plugin name</param>
        /// <returns>Context menu results. Null if the context-menu wasn't handled</returns>
        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, string pluginName)
        {
            if (selectedResult?.ContextData is (string branch, OperationMode mode) && mode == OperationMode.Branch)
            {
                return [
                    new ContextMenuResult
                    {
                        PluginName = pluginName,
                        Title = "Open Jenkins",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xE774",
                        AcceleratorKey = Key.Enter,
                        Action = _ => {
                            OpenJenkins(branch, config.JenkinsUrl);
                            return true;
                        }
                    },
                    new ContextMenuResult
                    {
                        PluginName = pluginName,
                        Title = "Download Tools",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xebd3",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Shift,
                        Action = _ => {
                            DownloadTools(branch, config.DownloadScriptPath, config.ShellType);
                            return true;
                        }
                    },
                    new ContextMenuResult
                    {
                        PluginName = pluginName,
                        Title = "Open Github",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xE753",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => {
                            OpenGithub(branch, config.GitRepoUrl);
                            return true;
                        }
                    }
                ];
            }

            return null;
        }

        public List<Result> GetQueryOptions()
        {
            return [
                new Result()
                {
                    Title = "<bl> Local branches",
                    SubTitle = "Locally available branches",
                    QueryTextDisplay = "bl"
                },
                new Result()
                {
                    Title = "<br> Remote branches",
                    SubTitle = "Remotelly available branches",
                    QueryTextDisplay = "br"
                }
            ];
        }

        /// <summary>
        /// Handle the query to select a branch
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <param name="checkLocal">Flag if the local branches from the source-folder should be checked (=true) or the remote branches (=false)</param>
        /// <returns>List of query results (branches)</returns>
        private List<Result>? HandleQueryIntern(IEnumerable<string> query, AppConfig config, bool checkLocal)
        {
            if (checkLocal && string.IsNullOrWhiteSpace(config.SourceFolder))
            {
                return [
                    new Result
                    {
                        Title = "Function (local branch) not available",
                        SubTitle ="Local source folder not configured!"
                    }
                ];
            }
            else if (!checkLocal && string.IsNullOrWhiteSpace(config.SourceFolder) && string.IsNullOrWhiteSpace(config.GitRepoUrl))
            {
                return [
                    new Result
                    {
                        Title = "Function (remote branch) not available",
                        SubTitle = "Local source folder and GIT URL not configured!"
                    }
                ];
            }

            string searchString = query.FirstOrDefault("").ToLowerInvariant();

            List<string> branches;
            string cacheKey = checkLocal ? localCacheKey : remoteCacheKey;

            branches = _cache.Get<List<string>>(cacheKey);
            if (branches == null)
            {
                if (!cacheLoading.GetValueOrDefault(cacheKey, false))
                {
                    cacheLoading[cacheKey] = true;

                    return [
                        new Result {
                            QueryTextDisplay = "Loading branches",
                            Title = "Loading"
                        }
                    ];
                }


                branches = _cache.GetOrAdd(cacheKey, () => GetBranchesQuery(config.GitRepoUrl, config.SourceFolder, checkLocal));
            }
            cacheLoading[cacheKey] = false;

            if (!string.IsNullOrWhiteSpace(searchString))
                branches = branches.FindAll(x => x.Contains(searchString, StringComparison.InvariantCultureIgnoreCase));

            if (branches == null)
            {
                return [
                    new Result {
                        QueryTextDisplay = $"No branches currently found",
                        Title = $"No branches currently found"
                    }
                ];
            }

            List<Result> results = branches.ConvertAll(branch =>
            {
                return new Result
                {
                    QueryTextDisplay = $"Branch: {branch}",
                    //IcoPath = IconPath,
                    Title = branch,
                    ToolTipData = new ToolTipData("Branch", branch),
                    ContextData = (branch, OperationMode.Branch),
                };
            });

            return results;

            static List<string> GetBranchesQuery(string repoUrl, string sourceFolder, bool checkLocal) => BranchManager.GetBranches(repoUrl, sourceFolder, checkLocal).Result!.ToList();
        }

        /// <summary>
        /// Download the tools for a selected branch.
        /// </summary>
        /// <param name="branch">Branch name</param>
        /// <param name="scriptPath">Path to the download-script</param>
        /// <param name="version">Version of the Powershell to use</param>"
        private void DownloadTools(string branch, string scriptPath, PowershellVersion version)
        {
            Log.Info($"Download Tools: {scriptPath} {branch}", GetType());
            if (string.IsNullOrEmpty(scriptPath) || !System.IO.Path.Exists(scriptPath))
                return;

            ProcessUtils.ExecutePowershellCommand($"{scriptPath} {branch}", version, title: "Download Tools");
        }

        /// <summary>
        /// Open the page for the branch on Jenkins
        /// </summary>
        /// <param name="branch">Branch name</param>
        /// <param name="jenkinsUrl">Base url for Jenkins</param>
        private void OpenJenkins(string branch, string jenkinsUrl)
        {
            Log.Info($"Open Jenkins: {branch}", GetType());
            string? jobUrl = UrlUtils.BuildJenkinsUrl(branch, jenkinsUrl);
            if (string.IsNullOrEmpty(jobUrl))
                return;

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, jobUrl);
        }

        /// <summary>
        /// Open the selected branch on Github
        /// </summary>
        /// <param name="branch">Branch to open</param>
        /// <param name="gitRepoUrl">Base URL of the Github-Repository</param>
        private void OpenGithub(string branch, string gitRepoUrl)
        {
            Log.Info($"Open Github: {branch}", GetType());
            string? gitUrl = UrlUtils.BuildGithubUrl(branch, gitRepoUrl);
            if (string.IsNullOrEmpty(gitUrl))
                return;

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, gitUrl);
        }
    }
}
