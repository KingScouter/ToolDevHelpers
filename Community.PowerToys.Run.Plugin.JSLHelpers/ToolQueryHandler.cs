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

            JSLTools selectedTool = JSLTools.None;
            string selectedToolName = "";
            switch (selectedToolQuery)
            {
                case "mt":
                    {
                        selectedTool = JSLTools.MT;
                        selectedToolName = "Modelling Tool";
                        break;
                    }
                case "mrcct":
                    {
                        selectedTool = JSLTools.MRCCT;
                        selectedToolName = "MRC Commissioning Tool";
                        break;
                    }
                case "hmi":
                    {
                        selectedTool = JSLTools.HMI;
                        selectedToolName = "HMI";
                        break;
                    }
                default:
                    return [];
            }

            int port = config.toolsPorts[selectedTool];


            Log.Info("Selected Tool: " + selectedTool.ToString(), GetType());

            return [
                new()
                {
                    QueryTextDisplay = $"query: {selectedToolName}",
                    //IcoPath = IconPath,
                    Title = $"Tool: {selectedToolName}",
                    SubTitle = $"Port: {port}",
                    ToolTipData = new ToolTipData("Tool", selectedToolName),
                    ContextData = (selectedTool, OperationMode.Tool),
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
                        Action = _ => StartTool(data)
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

        private bool StartTool(JSLTools tool)
        {
            Log.Info($"Start tool: {tool}", GetType());
            return false;
        }

        private bool OpenTool(JSLTools tool, bool isLocal, AppConfig config)
        {
            string url;
            int port = config.toolsPorts[tool];
            if (isLocal)
                url = "https://localhost";
            else
                url = config.TestServerUrl;

            url += $":{port}";

            Log.Info($"Open tool {url}, {BrowserInfo.Path}, {BrowserInfo.ArgumentsPattern}", GetType());

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, url);

            return true;
        }
    }
}
