using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CommonLib.Utils
{
    public class UrlUtils
    {
        /// <summary>
        /// Build the URL for the Jenkins Multibranch pipeline
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="baseUrl">Base URL to Jenkins</param>
        /// <returns>Jenkins URL (or null if something went wrong / is missing)</returns>
        public static string? BuildJenkinsUrl(string branch, string? baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(branch))
                return null;

            StringBuilder sb = new();
            sb.Append(baseUrl);

            if (!baseUrl.EndsWith("job/", StringComparison.InvariantCultureIgnoreCase) && !baseUrl.EndsWith("job", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!baseUrl.EndsWith('/'))
                    sb.Append('/');
                sb.Append("job/");
            }

            sb.Append(HttpUtility.UrlEncode(branch));
            return sb.ToString();
        }

        /// <summary>
        /// Build the URL for GitHub
        /// </summary>
        /// <param name="branch">Branch</param>
        /// <param name="baseUrl">Base URL to the GitHub Repository</param>
        /// <returns>GitHub URL (or null if something went wrong / is missing)</returns>
        public static string? BuildGithubUrl(string branch, string? baseUrl)
        {
            string repoBaseUrl = ParseGitUrl(baseUrl);
            if (string.IsNullOrEmpty(repoBaseUrl) || string.IsNullOrEmpty(branch))
                return null;

            return $"{repoBaseUrl}/tree/{branch}";
        }

        /// <summary>
        /// Parse the GIT remote URL to the base Github-URL.
        /// Example: 
        ///   git@github.com:User/Repo.git => https://github.com/User/Repo
        ///   https://github.com/User/Repo.git => https://github.com/User/Repo
        /// </summary>
        /// <param name="gitUrl"></param>
        /// <returns></returns>
        private static string ParseGitUrl(string gitUrl)
        {
            if (string.IsNullOrEmpty(gitUrl))
                return "";

            // Parse HTTPS URL
            if (!gitUrl.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
            {
                // Parse SSH URL
                if (gitUrl.StartsWith("git", StringComparison.InvariantCultureIgnoreCase))
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

            return gitUrl[..^".git".Length];
        }
    }
}
