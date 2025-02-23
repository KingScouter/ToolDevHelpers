using Wox.Plugin.Logger;
using Wox.Plugin;
using System.Windows.Input;
using System.IO;

namespace Community.PowerToys.Run.Plugin.JSLHelpers.QueryHandler
{
    internal class ToolQueryHandler : BaseQueryHandler
    {
        /// <summary>
        /// Handle the query to select a tool
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="config">App configuration</param>
        /// <returns>List of query results (tools). Null if the query wasn't handled</returns>
        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            var modeQuery = query.FirstOrDefault("");

            if (!string.Equals(modeQuery, "t", StringComparison.CurrentCultureIgnoreCase))
                return null;

            // No project configured => Skip
            if (config.ToolConfigProject == null)
                return [
                    new Result
                    {
                        Title = "Function (tool) not available",
                        SubTitle ="Tool configuration project not configured!"
                    }
                ];

            var selectedToolQuery = query.Skip(1).FirstOrDefault("");
            List<ToolConfig> configs = config.ToolConfigProject.GetToolConfigs(selectedToolQuery);

            return [.. configs.Select(x =>
            {
                return new Result()
                {
                    QueryTextDisplay = $"query: {x.name}",
                    //IcoPath = IconPath,
                    Title = $"{x.shortName}: {x.name}",
                    SubTitle = $"Port: {x.port}",
                    ToolTipData = new ToolTipData("Tool", x.name),
                    ContextData = (x, OperationMode.Tool),
                };
            })];
        }

        /// <summary>
        /// Load the context-menus for a selected tool.
        /// </summary>
        /// <param name="selectedResult">Query result</param>
        /// <param name="config">App configuration</param>
        /// <param name="pluginName">Plugin name</param>
        /// <returns>List of context-menu results. Null if the context-menu wasn't handled</returns>
        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, string pluginName)
        {
            if (selectedResult?.ContextData is (ToolConfig toolConfig, OperationMode toolMode) && toolMode == OperationMode.Tool)
            {
                ContextMenuResult startLocallyOption = new()
                {
                    PluginName = pluginName,
                    Title = "Start locally",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xe756",
                    AcceleratorKey = Key.Enter,
                    Action = _ => StartTool(toolConfig, config.FolderPath, config.ShellType)
                };

                ContextMenuResult openLocallyOption = new()
                {
                    PluginName = pluginName,
                    Title = "Open locally",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xEC27",
                    AcceleratorKey = Key.Enter,
                    AcceleratorModifiers = ModifierKeys.Control,
                    Action = _ => OpenTool(toolConfig, true)
                };
                ContextMenuResult openRemotelyOption = new()
                {
                    PluginName = pluginName,
                    Title = "Open on Remoteserver",
                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                    Glyph = "\xe753",
                    AcceleratorKey = Key.Enter,
                    AcceleratorModifiers = ModifierKeys.Shift,
                    Action = _ => OpenTool(toolConfig, false)
                };

                List<ContextMenuResult> options = [startLocallyOption];
                if (toolConfig.port != 0)
                    options.Add(openLocallyOption);

                if (!string.IsNullOrWhiteSpace(toolConfig.remoteServerUrl))
                    options.Add(openRemotelyOption);

                return options;
            }

            return null;
        }

        public List<Result> GetQueryOptions()
        {
            return [
                new Result()
                {
                    Title = "<t> Tools",
                    SubTitle = "Select a tool configuration",
                    QueryTextDisplay = "t",
                }
            ];
        }

        /// <summary>
        /// Start a given tool by its executable
        /// </summary>
        /// <param name="toolConfig">Tool configuration</param>
        /// <param name="baseFolder">Base folder with the downloaded tools</param>
        /// <param name="version">Version of the Powershell to use</param>"
        /// <returns>True</returns>
        private bool StartTool(ToolConfig toolConfig, string baseFolder, PowershellVersion version)
        {
            Log.Info($"Start tool: {toolConfig.name}", GetType());

            string pathToExe = toolConfig.exePath;
            if (!Path.IsPathRooted(pathToExe))
                pathToExe = Path.Combine(baseFolder, toolConfig.exePath);

            if (!Path.Exists(pathToExe))
            {
                Log.Info($"Path to tool {pathToExe} does not exist", GetType());
                return false;
            }

            Utils.ExecutePowershellCommand(pathToExe, version);

            return true;
        }

        /// <summary>
        /// Open a tool in the browser (locally or on a remote server).
        /// If configured, open additional pages as well.
        /// </summary>
        /// <param name="toolConfig">Tool configuration</param>
        /// <param name="isLocal">True if it should be opened locally, false to open it on a remote server</param>
        /// <returns>True</returns>
        private bool OpenTool(ToolConfig toolConfig, bool isLocal)
        {
            string url = "";
            string urlPrefix = toolConfig.useHttps ? "https" : "http";
            if (!isLocal && !string.IsNullOrWhiteSpace(toolConfig.remoteServerUrl))
                url = toolConfig.remoteServerUrl;
            else
                url = "localhost";

            url = new UriBuilder(urlPrefix, url, (int)toolConfig.port, "").ToString();

            Log.Info($"Open tool {url}", GetType());
            Utils.OpenPageInBrowser(url);

            // Open any additional page as well
            foreach (var page in toolConfig.additionalPages)
            {
                var pageUrl = page.Replace("#BASE#", url);
                Log.Info($"Open additional page {pageUrl}", GetType());
                Utils.OpenPageInBrowser(pageUrl);
            }

            return true;
        }
    }
}
