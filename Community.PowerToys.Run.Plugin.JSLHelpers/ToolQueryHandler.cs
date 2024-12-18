using Wox.Plugin.Logger;
using Wox.Plugin;
using System.Windows.Input;
using Wox.Infrastructure;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class ToolQueryHandler
    {
        public List<Result> HandleQuery(IEnumerable<string> query, ToolConfigProject? configProject)
        {
            // No project configured => Skip
            if (configProject == null)
                return [];

            var selectedToolQuery = query.FirstOrDefault("");

            if (string.IsNullOrWhiteSpace(selectedToolQuery))
                return [];

            ToolConfig? toolConfig = configProject.GetToolConfig(selectedToolQuery);

            if (toolConfig == null)
                return [];

            Log.Info("Selected Tool: " + toolConfig.name, GetType());

            return [
                new()
                {
                    QueryTextDisplay = $"query: {toolConfig.name}",
                    //IcoPath = IconPath,
                    Title = $"Tool: {toolConfig.name}",
                    SubTitle = $"Port: {toolConfig.port}",
                    ToolTipData = new ToolTipData("Tool", toolConfig.name),
                    ContextData = (toolConfig.shortName.ToLower(), OperationMode.Tool),
                }
            ];
        }

        public List<ContextMenuResult> LoadContextMenus(string tool, string name, AppConfig config)
        {
            if (config.ToolConfigProject == null)
                return [];

            return [
                new ContextMenuResult
                {
                        PluginName = name,
                        Title = "Start locally",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xe756",
                        AcceleratorKey = Key.Enter,
                        Action = _ => StartTool(tool, config)
                    },
                    new ContextMenuResult
                    {
                        PluginName = name,
                        Title = "Open locally",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xEC27",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => OpenTool(tool, true, config.ToolConfigProject, config.RemoteServerUrl)
                    },
                    new ContextMenuResult
                    {
                        PluginName = name,
                        Title = "Open on Testserver",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xe753",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Shift,
                        Action = _ => OpenTool(tool, false, config.ToolConfigProject, config.RemoteServerUrl)
                    }
            ];
        }

        private bool StartTool(string tool, AppConfig config)
        {
            Log.Info($"Start tool: {tool}, {config.FolderPath}/test.ps1", GetType());
            Utils.ExecutePowershellCommand($"{config.FolderPath}/test.ps1");
            return true;
        }

        private bool OpenTool(string tool, bool isLocal, ToolConfigProject configProject, string remoteServerUrl)
        {
            string url;
            ToolConfig? toolConfig = configProject.GetToolConfig(tool);
            if (toolConfig == null)
                return false;

            if (isLocal)
                url = "https://localhost";
            else
                url = remoteServerUrl;

            url += $":{toolConfig.port}";

            Log.Info($"Open tool {url}, {BrowserInfo.Path}, {BrowserInfo.ArgumentsPattern}", GetType());

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, url);

            return true;
        }
    }
}
