using CommonLib.Models;
using System.IO;
using Wox.Infrastructure;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal sealed class Utils
    {
        /// <summary>
        /// Open a given URL in the default webbrowser
        /// </summary>
        /// <param name="url">URL to open</param>
        public static void OpenPageInBrowser(string url)
        {
            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, url);
        }
    }
}
