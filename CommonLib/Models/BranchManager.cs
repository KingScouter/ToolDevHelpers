using LazyCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Models
{
    public class BranchManager
    {
        //private readonly CachingService _cache;

        //private static readonly string remoteCacheKey = "remoteCache";
        //private static readonly string localCacheKey = "localCache";

        //private Dictionary<string, bool> cacheLoading = new()
        //{
        //    {remoteCacheKey, false},
        //    {localCacheKey, false}
        //};

        //public BranchManager()
        //{
        //    _cache = new CachingService();
        //    _cache.DefaultCachePolicy.DefaultCacheDurationSeconds = (int)TimeSpan.FromMinutes(2).TotalSeconds;
        //}

        /// <summary>
        /// Get a list of all branches for a given repostiory, filtered by a search value.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <param name="checkLocal">Flag if the local branches from the source-folder should be checked (=true) or the remote branches (=false)</param>
        /// <returns>List of branches</returns>
        public static Task<IEnumerable<string>> GetBranches(string repoUrl, string sourceFolder, bool checkLocal)
        {
            if (checkLocal)
                return GetLocalBranches(sourceFolder);
            else
                return GetRemoteBranches(repoUrl, sourceFolder);
        }

        /// <summary>
        /// Get a list of all local branches of a given source folder.
        /// </summary>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <returns>List of local branches</returns>
        public static async Task<IEnumerable<string>> GetLocalBranches(string sourceFolder)
        {
            string getBranchesCmd = "git branch --list -r";
            IEnumerable<string> branchesOutput = await ProcessUtils.ExecuteCmdCommandAsync(getBranchesCmd, sourceFolder);

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
        /// Get a list of all remote branches for a given repostiory.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <param name="sourceFolder">Path to the local source folder</param>
        /// <returns>List of remote branches</returns>
        public static async Task<IEnumerable<string>> GetRemoteBranches(string repoUrl, string sourceFolder)
        {
            IEnumerable<string> branchesOutput;

            if (!string.IsNullOrWhiteSpace(repoUrl))
            {
                string getBranchesCmd = $"git ls-remote {repoUrl}";
                branchesOutput = await ProcessUtils.ExecuteCmdCommandAsync(getBranchesCmd);
            }
            else
            {
                string getBranchesCmd = $"git ls-remote";
                branchesOutput = await ProcessUtils.ExecuteCmdCommandAsync(getBranchesCmd, sourceFolder);
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
                if (branchRef.StartsWith(refStart, StringComparison.InvariantCultureIgnoreCase))
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
    }
}
