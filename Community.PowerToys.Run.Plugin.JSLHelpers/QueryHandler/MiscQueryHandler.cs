using CommonLib.Models;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.JSLHelpers.QueryHandler
{
    internal class MiscQueryHandler(AppConfigManager appConfigManager) : BaseQueryHandler
    {
        /// <summary>
        /// Handle the query to for general options
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <returns>List of query results (tools). Null if the query wasn't handled</returns>
        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            string modeQuery = query.FirstOrDefault("");

            if (string.Equals(modeQuery, "reload", StringComparison.CurrentCultureIgnoreCase))
            {
                return [
                    new Result()
                    {
                        QueryTextDisplay = "Reload",
                        Title = "Reload config project",
                        SubTitle = $"{appConfigManager.Config.ToolConfigFile}",
                        Action = _ => {
                            appConfigManager.HandleConfigFile();
                            return true;
                        }
                    }
                ];
            }

            return null;

        }

        /// <summary>
        /// Load the context-menus. Currently none available for misc queries.
        /// </summary>
        /// <param name="selectedResult">Query result</param>
        /// <param name="config">App configuration</param>
        /// <param name="pluginName">Plugin name</param>
        /// <returns>List of context-menu results. Null if the context-menu wasn't handled</returns>
        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, string pluginName)
        {
            return null;
        }

        public List<Result> GetQueryOptions()
        {
            return [
                new Result()
                {
                    Title = "<reload> Reload",
                    SubTitle = "Reload the tool configuration project",
                    QueryTextDisplay = "reload"
                }
            ];
        }
    }
}
