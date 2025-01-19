using LazyCache;
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

        /// <summary>
        /// Initialize the caching-service
        /// </summary>
        public void Init()
        {
            _cache = new CachingService();
            _cache.DefaultCachePolicy.DefaultCacheDurationSeconds = (int)TimeSpan.FromMinutes(1).TotalSeconds;
        }

        /// <summary>
        /// Handle the query to select a branch
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <returns>List of query results (branches)</returns>
        public List<Result> HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            if (query.Count() > 1)
                return [];

            string searchString = query.FirstOrDefault("").ToLower();

            List<string> branches;
            branches = _cache.GetOrAdd("foo", () => GetBranchesQuery(config.GitRepoUrl));

            if (!string.IsNullOrWhiteSpace(searchString))
                branches = branches.FindAll(x => x.ToLower().Contains(searchString));

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

            static List<string> GetBranchesQuery(string repoUrl) => GetBranches(repoUrl).Result!.ToList();
        }

        /// <summary>
        /// Get a list of all branches for a given repostiory, filtered by a search value.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <returns>List of branches</returns>
        public static async Task<IEnumerable<string>> GetBranches(string repoUrl)
        {
            string getBranchesCmd = $"git ls-remote {repoUrl}";
            IEnumerable<string> branchesOutput = await Utils.ExecuteCmdCommandAsync(getBranchesCmd);
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
        /// Parse a branch output (HEAD commit and branch-ref) to retrieve only the branch-name
        /// </summary>
        /// <param name="branchOutput">Branch output from the cmd (HEAD commit and branch-ref)</param>
        /// <returns>Branch-name</returns>
        private static string? ParseBranchOutput(string branchOutput)
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
                        DownloadTools(branch, config.DownloadScriptPath);
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
        private void DownloadTools(string branch, string scriptPath)
        {
            Log.Info($"Download Tools: {scriptPath} {branch}", GetType());
            if (string.IsNullOrEmpty(scriptPath) || !System.IO.Path.Exists(scriptPath))
                return;

            Utils.ExecutePowershellCommand($"{scriptPath} {branch}");
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
