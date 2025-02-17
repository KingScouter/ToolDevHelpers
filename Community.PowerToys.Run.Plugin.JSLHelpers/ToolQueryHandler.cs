using Wox.Plugin.Logger;
using Wox.Plugin;
using System.Windows.Input;
using System.IO;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class ToolQueryHandler
    {
        /// <summary>
        /// Handle the query to select a tool
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="configProject">Configured tools</param>
        /// <returns>List of query results (tools)</returns>
        public List<Result> HandleQuery(IEnumerable<string> query, ToolConfigProject? configProject)
        {
            // No project configured => Skip
            if (configProject == null)
                return [];

            var selectedToolQuery = query.FirstOrDefault("");
            List<ToolConfig> configs = configProject.GetToolConfigs(selectedToolQuery);

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
        /// Load the context-menus for a selected tool (start locally, open locally, open remotely).
        /// </summary>
        /// <param name="toolConfig">Tool configuration</param>
        /// <param name="name">Plugin name</param>
        /// <param name="config">App configuration</param>
        /// <returns>List of context-menu results</returns>
        public List<ContextMenuResult> LoadContextMenus(ToolConfig toolConfig, string name, AppConfig config)
        {
            ContextMenuResult startLocallyOption = new ContextMenuResult
            {
                PluginName = name,
                Title = "Start locally",
                FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                Glyph = "\xe756",
                AcceleratorKey = Key.Enter,
                Action = _ => StartTool(toolConfig, config.FolderPath, config.ShellType)
            };

            ContextMenuResult openLocallyOption = new ContextMenuResult
            {
                PluginName = name,
                Title = "Open locally",
                FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                Glyph = "\xEC27",
                AcceleratorKey = Key.Enter,
                AcceleratorModifiers = ModifierKeys.Control,
                Action = _ => OpenTool(toolConfig, true)
            };
            ContextMenuResult openRemotelyOption = new ContextMenuResult
            {
                PluginName = name,
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

            string pathToExe = Path.Combine(baseFolder, toolConfig.exePath);

            if (!Path.Exists(pathToExe))
                return false;

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
