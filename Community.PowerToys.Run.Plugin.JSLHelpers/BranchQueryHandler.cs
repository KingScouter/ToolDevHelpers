﻿using LazyCache;
using System.Text;
using System.Web;
using System.Windows.Input;
using Wox.Infrastructure;
using Wox.Plugin;
using Wox.Plugin.Logger;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class BranchQueryHandler
    {
        private CachingService? _cache;

        private static readonly string remoteCacheKey = "remoteCache";
        private static readonly string localCacheKey = "localCache";

        private Dictionary<string, bool> cacheLoading = new()
        {
            {remoteCacheKey, false},
            {localCacheKey, false}
        };

        /// <summary>
        /// Initialize the caching-service
        /// </summary>
        public void Init()
        {
            _cache = new CachingService();
            _cache.DefaultCachePolicy.DefaultCacheDurationSeconds = (int)TimeSpan.FromMinutes(2).TotalSeconds;
        }

        /// <summary>
        /// Handle the query to select a branch
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <param name="checkLocal">Flag if the local branches from the source-folder should be checked (=true) or the remote branches (=false)</param>
        /// <returns>List of query results (branches)</returns>
        public List<Result> HandleQuery(IEnumerable<string> query, AppConfig config, bool checkLocal, Query origQuery)
        {
            int randomId = Random.Shared.Next();

            if (!checkLocal)
            {
                Log.Info($"Branch remote test: rawQuery: <{origQuery.RawQuery}> <{origQuery.RawUserQuery}>", GetType());
            }

            if (query.Count() > 1)
                return [];

            if (checkLocal && string.IsNullOrWhiteSpace(config.SourceFolder))
            {
                return [
                    new Result
                    {
                        Title = "Function (local branch) not available",
                        SubTitle ="Local source folder not configured!"
                    }
                ];
            } else if (!checkLocal && string.IsNullOrWhiteSpace(config.SourceFolder) && string.IsNullOrWhiteSpace(config.GitRepoUrl)) {
                return [
                    new Result
                    {
                        Title = "Function (remote branch) not available",
                        SubTitle = "Local source folder and GIT URL not configured!"
                    }
                ];
            }

            string searchString = query.FirstOrDefault("").ToLower();

            List<string> branches;
            string cacheKey = checkLocal ? localCacheKey : remoteCacheKey;

            if (!checkLocal)
                Log.Info($"Branch remote test {randomId} 1", GetType());
            branches = _cache.Get<List<string>>(cacheKey);
            if (branches == null)
            {
                if (!checkLocal)
                    Log.Info($"Branch remote test {randomId}: Cache not available", GetType());
                if (!cacheLoading.GetValueOrDefault(cacheKey, false))
                {
                    if (!checkLocal)
                        Log.Info($"Branch remote test {randomId}: Load cache", GetType());
                    cacheLoading[cacheKey] = true;

                    //_cache.Add(cacheKey, () => GetBranchesQuery(config.GitRepoUrl, config.SourceFolder, checkLocal));
                    return [
                        new Result {
                            QueryTextDisplay = "Loading branches",
                            Title = "Loading"
                        }
                    ];
                }


                if (!checkLocal)
                    Log.Info($"Branch remote test {randomId}: Getoradd cache", GetType());
                branches = _cache.GetOrAdd(cacheKey, () => GetBranchesQuery(config.GitRepoUrl, config.SourceFolder, checkLocal));
                if (!checkLocal)
                    Log.Info($"Branch remote test {randomId}: After Getoradd cache: {branches?.Count}", GetType());
            }
            cacheLoading[cacheKey] = false;
            //branches = cacheData;

            //branches = _cache.GetOrAdd(cacheKey, () => GetBranchesQuery(config.GitRepoUrl, config.SourceFolder, checkLocal));
            if (!checkLocal)
                Log.Info($"Branch remote test {randomId} 2", GetType());

            if (!string.IsNullOrWhiteSpace(searchString))
                branches = branches.FindAll(x => x.ToLower().Contains(searchString));

            if (branches == null)
            {
                if (!checkLocal)
                    Log.Info($"Branch remote test {randomId}: Branches null {checkLocal}", GetType());
                return [
                new Result {
                        QueryTextDisplay = $"Branches null {checkLocal}",
                        Title = $"Branches null {checkLocal}"
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

            if (!checkLocal)
                Log.Info($"Branch remote test {randomId} 3", GetType());

            return results;

            static List<string> GetBranchesQuery(string repoUrl, string sourceFolder, bool checkLocal) => GetBranches(repoUrl, sourceFolder, checkLocal).Result!.ToList();
        }

        /// <summary>
        /// Get a list of all branches for a given repostiory, filtered by a search value.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <param name="checkLocal">Flag if the local branches from the source-folder should be checked (=true) or the remote branches (=false)</param>
        /// <returns>List of branches</returns>
        public static Task<IEnumerable<string>> GetBranches(string repoUrl, string sourceFolder, bool checkLocal)
        {
            if (!checkLocal)
                Log.Info($"Branch remote test 4", new BranchQueryHandler().GetType());
            if (checkLocal)
                return GetLocalBranches(sourceFolder);
            else
                return GetRemoteBranches(repoUrl, sourceFolder);
        }

        /// <summary>
        /// Get a list of all remote branches for a given repostiory.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <returns>List of remote branches</returns>
        private static async Task<IEnumerable<string>> GetRemoteBranches(string repoUrl, string sourceFolder)
        {
            IEnumerable<string> branchesOutput;

            if (!string.IsNullOrWhiteSpace(repoUrl))
            {
                string getBranchesCmd = $"git ls-remote {repoUrl}";
                branchesOutput = await Utils.ExecuteCmdCommandAsync(getBranchesCmd);
            } else
            {
                string getBranchesCmd = $"git ls-remote";
                branchesOutput = await Utils.ExecuteCmdCommandAsync(getBranchesCmd, sourceFolder);
            }

            List<string> branchNames = [];
            foreach (string branch in branchesOutput)
            {
                var branchName = ParseLsRemoteOutput(branch);
                if (branchName != null)
                    branchNames.Add(branchName);
            }

            return branchNames;
        }

        /// <summary>
        /// Get a list of all local branches of a given source folder.
        /// </summary>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <returns>List of local branches</returns>
        private static async Task<IEnumerable<string>> GetLocalBranches(string sourceFolder)
        {
            string getBranchesCmd = "git branch --list -r";
            IEnumerable<string> branchesOutput = await Utils.ExecuteCmdCommandAsync(getBranchesCmd, sourceFolder);

            List<string> branchNames = [];
            foreach (string branch in branchesOutput)
            {
                var branchName = ParseBranchOutput(branch);
                if (branchName != null)
                    branchNames.Add(branchName);
            }

            return branchNames;
        }

        /// <summary>
        /// Parse a "git ls-remote" output (HEAD commit and branch-ref) to retrieve only the branch-name
        /// </summary>
        /// <param name="branchOutput">Output from the cmd (HEAD commit and branch-ref)</param>
        /// <returns>Branch-name</returns>
        private static string? ParseLsRemoteOutput(string branchOutput)
        {
            string refStart = "refs/heads/";
            var splitted = branchOutput.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length == 2)
            {
                var branchRef = splitted[1];
                if (branchRef.StartsWith(refStart))
                {
                    var simpleBranch = branchRef.Substring(refStart.Length);
                    return simpleBranch;
                }
            }

            return null;
        }

        /// <summary>
        /// Parse a "git branch" output to retrieve only the branch-name
        /// </summary>
        /// <param name="branchOutput">Output from the cmd</param>
        /// <returns>Branch-name</returns>
        private static string? ParseBranchOutput(string branchOutput)
        {
            // Skip potential mapping from HEAD to the tracking branch if it is out of date (e.g. origin/HEAD -> origin/main)
            if (branchOutput.Contains(" -> "))
                return null;

            var splitted = branchOutput.Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length == 1)
                return splitted[0];

            return splitted[1];
        }

        /// <summary>
        /// Load the context-menus for a selected branch (open Jenkins, download tools).
        /// </summary>
        /// <param name="branch">Branch name</param>
        /// <param name="name">Plugin name</param>
        /// <param name="config">App configuration</param>
        /// <returns></returns>
        public List<ContextMenuResult> LoadContextMenus(string branch, string name, AppConfig config)
        {
            return [
                new ContextMenuResult
                {
                    PluginName = name,
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
                    PluginName = name,
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
                    PluginName = name,
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

            Utils.ExecutePowershellCommand($"{scriptPath} {branch}", version);
        }

        /// <summary>
        /// Open the page for the branch on Jenkins
        /// </summary>
        /// <param name="branch">Branch name</param>
        /// <param name="jenkinsUrl">Base url for Jenkins</param>
        private void OpenJenkins(string branch, string jenkinsUrl)
        {
            Log.Info($"Open Jenkins: {branch}", GetType());
            StringBuilder sb = new();
            sb.Append(jenkinsUrl);

            if (!jenkinsUrl.EndsWith("job/") && !jenkinsUrl.EndsWith("job"))
            {
                if (!jenkinsUrl.EndsWith('/'))
                    sb.Append('/');
                sb.Append("job/");
            }

            sb.Append(HttpUtility.UrlEncode(branch));

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, sb.ToString());
        }

        /// <summary>
        /// Open the selected branch on Github
        /// </summary>
        /// <param name="branch">Branch to open</param>
        /// <param name="gitRepoUrl">Base URL of the Github-Repository</param>
        private void OpenGithub(string branch, string gitRepoUrl)
        {
            Log.Info($"Open Github: {branch}", GetType());
            string repoBaseUrl = ParseGitUrl(gitRepoUrl);
            if (string.IsNullOrEmpty(repoBaseUrl))
                return;

            string url = $"{repoBaseUrl}/tree/{branch}";

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, url);
        }

        /// <summary>
        /// Parse the GIT remote URL to the base Github-URL.
        /// Example: 
        ///   git@github.com:User/Repo.git => https://github.com/User/Repo
        ///   https://github.com/User/Repo.git => https://github.com/User/Repo
        /// </summary>
        /// <param name="gitUrl"></param>
        /// <returns></returns>
        private string ParseGitUrl(string gitUrl)
        {
            if (string.IsNullOrEmpty(gitUrl))
                return "";

            // Parse HTTPS URL
            if (gitUrl.StartsWith("https"))
            {
                return gitUrl[..^".git".Length];
            }

            // Parse SSH URL
            if (gitUrl.StartsWith("git"))
            {
                string[] urlParts = gitUrl.Split(':');
                if (urlParts.Length != 2)
                    return "";

                string repoUrl = urlParts[1];

                string url = new UriBuilder
                {
                    Scheme = "https",
                    Host = "github.com",
                    Path = repoUrl[..^".git".Length]
                }.ToString();

                return url;
            }

            return "";
        }
    }
}
