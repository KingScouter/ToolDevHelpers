using CommonLib.Models;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.ToolDevHelpers.QueryHandler
{
    internal interface BaseProjectQueryHandler
    {
        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config, ProjectConfig projectConfig);
        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, ProjectConfig projectConfig, string pluginName);

        /// <summary>
        /// Get all available querys of the handler
        /// </summary>
        /// <returns>List of queriess</returns>
        public List<Result> GetQueryOptions();
    }
}
