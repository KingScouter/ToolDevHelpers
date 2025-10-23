using CommonLib.Models;
using System.Diagnostics;
using Wox.Plugin;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.ToolDevHelpers.QueryHandler
{
    public class ProjectQueryHandler : BaseQueryHandler
    {
        private AppConfigManager AppConfigManager { get; set; }
        private ProjectConfigFile? Config { get => AppConfigManager.Config.ProjectConfigFile; }

        private readonly BaseProjectQueryHandler[] queryHandlers;
        
        public ProjectQueryHandler(AppConfigManager appConfigManager)
        {
            AppConfigManager = appConfigManager;

            queryHandlers = [
                new BranchQueryHandler(),
                new ToolQueryHandler()
            ];
        }

        public List<Result> GetQueryOptions()
        {
            if (Config == null)
            {
                return [
                    new Result()
                {
                    QueryTextDisplay = $"No project found",
                    Title = $"No project configured in project file!"
                }];
            }

            return [.. Config.ProjectConfigMap.Configs.Select(x => {
                return new Result()
                {
                    QueryTextDisplay = $"query: {x.key}",
                    Title = $"{x.key}: {x.label}",
                    ToolTipData = new ToolTipData("Project", x.label),
                    ContextData = (x)
                };
            })];
        }

        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            var projectQuery = query.FirstOrDefault("");

            var project = config.ProjectConfigFile?.ProjectConfigMap.Get(projectQuery);
            if (project == null)
            {
                return null;
            }

            foreach (var queryHandler in queryHandlers)
            {
                var results = queryHandler.HandleQuery(query.Skip(1), config, project);
                if (results != null)
                    return results;
            }

            return queryHandlers.SelectMany(elem => elem.GetQueryOptions()).ToList();
        }

        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, string pluginName)
        {
            return null;
        }
    }
}
