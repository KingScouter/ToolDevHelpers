using Wox.Plugin.Logger;
using Wox.Plugin;
using System.Windows.Input;
using Wox.Infrastructure;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class ToolQueryHandler
    {
        public List<Result> HandleQuery(IEnumerable<string> query, AppConfig config)
        {
            var selectedToolQuery = query.FirstOrDefault("");

            if (string.IsNullOrWhiteSpace(selectedToolQuery))
                return [];

            ToolConfig toolConfig;

            try
            {
                toolConfig = ToolConfig.tools.First(x => x.Value.shortName.ToLower() == selectedToolQuery.ToLower()).Value;
            } catch (InvalidOperationException)
            {
                return [];
            }

            Log.Info("Selected Tool: " + toolConfig.name, GetType());

            return [
                new()
                {
                    QueryTextDisplay = $"query: {toolConfig.name}",
                    //IcoPath = IconPath,
                    Title = $"Tool: {toolConfig.name}",
                    SubTitle = $"Port: {toolConfig.port}",
                    ToolTipData = new ToolTipData("Tool", toolConfig.name),
                    ContextData = (toolConfig.id, OperationMode.Tool),
                }
            ];
        }

        public List<ContextMenuResult> LoadContextMenus(JSLTools data, string name, AppConfig config)
        {
            return [
                new ContextMenuResult
                {
                        PluginName = name,
                        Title = "Start locally",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xe756",
                        AcceleratorKey = Key.Enter,
                        Action = _ => StartTool(data, config)
                    },
                    new ContextMenuResult
                    {
                        PluginName = name,
                        Title = "Open locally",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xe774",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Control,
                        Action = _ => OpenTool(data, true, config)
                    },
                    new ContextMenuResult
                    {
                        PluginName = name,
                        Title = "Open on Testserver",
                        FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                        Glyph = "\xe753",
                        AcceleratorKey = Key.Enter,
                        AcceleratorModifiers = ModifierKeys.Shift,
                        Action = _ => OpenTool(data, false, config)
                    }
            ];
        }

        private bool StartTool(JSLTools tool, AppConfig config)
        {
            Log.Info($"Start tool: {tool}, {config.FolderPath}/test.ps1", GetType());
            Utils.ExecutePowershellCommand($"{config.FolderPath}/test.ps1");
            return true;
        }

        private bool OpenTool(JSLTools tool, bool isLocal, AppConfig config)
        {
            string url;
            ToolConfig toolConfig = ToolConfig.tools[tool];
            if (isLocal)
                url = "https://localhost";
            else
                url = config.TestServerUrl;

            url += $":{toolConfig.port}";

            Log.Info($"Open tool {url}, {BrowserInfo.Path}, {BrowserInfo.ArgumentsPattern}", GetType());

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, url);

            return true;
        }
    }
}
