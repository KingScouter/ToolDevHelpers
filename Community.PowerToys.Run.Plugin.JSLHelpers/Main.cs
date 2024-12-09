using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;
using Wox.Plugin.Logger;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;
using LazyCache;
using System.Web;
using Wox.Infrastructure;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    enum JSLTools
    {
        None,
        MT,
        MRCCT,
        HMI
    }

    enum OperationMode
    {
        Branch,
        Tool
    }

    /// <summary>
    /// Main class of this plugin that implement all used interfaces.
    /// </summary>
    public class Main : IContextMenu, ISettingProvider, IDisposable, IPlugin, IReloadable
    {
        private Dictionary<string, JSLTools> toolNames = new()
        {
            { "MT", JSLTools.MT },
            { "MRCCT", JSLTools.MRCCT},
            { "HMI", JSLTools.HMI },
        };

        // Settings values
        private string GitRepoUrl { get; set; }
        private string JenkinsUrl { get; set; }
        private string FolderPath { get; set; }
        private string TestServerUrl { get; set; }
        private string DownloadScriptPath { get; set; }
        private List<string> ToolsPorts { 
            get 
            {
                List<string> val = new List<string>();
                foreach (var tool in toolsPorts)
                {
                    string toolName = toolNames.First(elem => elem.Value == tool.Key).Key;
                    val.Add($"{toolName} {tool.Value}");
                }

                return val;
            }
            set
            {
                toolsPorts.Clear();
                foreach (var tool in value)
                {
                    string[] splitted = tool.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length != 2)
                        continue;

                    int port;
                    if (!int.TryParse(splitted[1], out port))
                        continue;

                    JSLTools toolName;
                    if (!toolNames.TryGetValue(splitted[0].ToUpper(), out toolName))
                        continue;

                    toolsPorts.Add(toolName, port);
                }
            }
        }


        private Dictionary<JSLTools, int> toolsPorts { get; set; } = [];
        private CachingService? _cache;

        // General plugin members
        private PluginInitContext? Context { get; set; }
        private string? IconPath { get; set; }
        private bool Disposed { get; set; }
        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "AE953C974C2241878F282EA18A7769E4";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "JSL Helpers";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Helpers for JSL developers";

        /// <summary>
        /// Additional options for the plugin.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions => [
            new()
            {
                Key = nameof(GitRepoUrl),
                DisplayLabel = "GIT Repository URL",
                DisplayDescription = "URL for the GIT repository",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = GitRepoUrl
            },
            new()
            {
                Key = nameof(JenkinsUrl),
                DisplayLabel = "Jenkins Multibranch-Pipeline URL",
                DisplayDescription = "URL of the Multibranch-Pipeline on Jenkins",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = JenkinsUrl
            },
            new()
            {
                Key = nameof(TestServerUrl),
                DisplayLabel = "Testserver URL",
                DisplayDescription = "URL of the testserver where the tools are running",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = TestServerUrl
            },
            new()
            {
                Key = nameof(FolderPath),
                DisplayLabel = "Tool folder",
                DisplayDescription = "Folder to store the tools",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = FolderPath
            },
            new()
            {
                Key = nameof (DownloadScriptPath),
                DisplayLabel = "Download Script",
                DisplayDescription = "Path to the download script",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = DownloadScriptPath
            },
            new()
            {
                Key = nameof(ToolsPorts),
                DisplayLabel = "Tools and Ports",
                DisplayDescription = "List of tools and their productive ports",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.MultilineTextbox,
                TextValueAsMultilineList = ToolsPorts
            }
        ];


        public List<Result> Query(Query query)
        {
            if (query.Terms.Count == 0 || query.Terms.Count > 2)
                return [];

            var modeQuery = query.Terms.FirstOrDefault("");

            if (string.IsNullOrWhiteSpace(modeQuery))
                return [];

            switch (modeQuery)
            {
                case "b":
                    {
                        return HandleBranchQuery(query.Terms.Skip(1));
                    }
                case "t":
                    {
                        return HandleToolQuery(query.Terms.Skip(1));
                    }
            }

            return [];
        }

        

        private List<Result> HandleBranchQuery(IEnumerable<string> query)
        {
            if (query.Count() > 1)
                return [];

            string searchString = query.FirstOrDefault("");

            List<string> branches;
            branches = _cache.GetOrAdd("foo", () => GetBranchesQuery(GitRepoUrl));

            if (!string.IsNullOrWhiteSpace(searchString))
                branches = branches.FindAll(x => x.Contains(searchString));

            List<Result> results = branches.ConvertAll(branch =>
            {
                return new Result
                {
                    //QueryTextDisplay = $"query: {selectedToolName}",
                    IcoPath = IconPath,
                    Title = branch,
                    //SubTitle = $"Port: {port}",
                    ToolTipData = new ToolTipData("Branch", branch),
                    ContextData = (branch, OperationMode.Branch),
                };
            });

            return results;

            static List<string> GetBranchesQuery(string repoUrl) => GetBranches(repoUrl).Result!.ToList();
        }

        private List<Result> HandleToolQuery(IEnumerable<string> query)
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

            int port = toolsPorts[selectedTool];


            Log.Info("Selected Tool: " + selectedTool.ToString(), GetType());

            return [
                new()
                {
                    QueryTextDisplay = $"query: {selectedToolName}",
                    IcoPath = IconPath,
                    Title = $"Tool: {selectedToolName}",
                    SubTitle = $"Port: {port}",
                    ToolTipData = new ToolTipData("Tool", selectedToolName),
                    ContextData = (selectedTool, OperationMode.Tool),
                }
            ];
        }

        /// <summary>
        /// Get a list of all branches for a given repostiory, filtered by a search value.
        /// </summary>
        /// <param name="repoUrl">URL of the GIT repository</param>
        /// <returns>List of branches</returns>
        public static async Task<IEnumerable<string>> GetBranches(string repoUrl)
        {
            string getBranchesCmd = $"git ls-remote {repoUrl}";
            IEnumerable<string> branchesOutput = await ExecuteCmdCommand(getBranchesCmd);
            List<string> branchNames = [];
            foreach (string branch in branchesOutput)
            {
                var branchName = ParseBranchOutput(branch);
                if (branchName != null)
                    branchNames.Add(branchName);
            }
                
            return branchNames;
        }

        /// <summary>
        /// Parse a branch output (HEAD commit and branch-ref) to retrieve only the branch-name
        /// </summary>
        /// <param name="branchOutput">Branch output from the cmd (HEAD commit and branch-ref)</param>
        /// <returns>Branch-name</returns>
        private static string? ParseBranchOutput(string branchOutput)
        {
            string refStart = "refs/heads/";
            var splitted = branchOutput.Split('\t', StringSplitOptions.RemoveEmptyEntries);
            if (splitted.Length == 2)
            {
                var branchRef = splitted[1];
                if (branchRef.StartsWith(refStart))
                {
                    var simpleBranch = branchRef.Substring(refStart.Length);
                    return simpleBranch;
                }
            }

            return null;
        }

        /// <summary>
        /// Execute a command in the windows command-line
        /// </summary>
        /// <param name="cmd">Command template</param>
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <returns>Standard output</returns>
        private static Task<IEnumerable<string>> ExecuteCmdCommand(string cmd)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe",
                RedirectStandardOutput = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                //CreateNoWindow = true,
            };

            startInfo.Arguments = $"/C {cmd}";

            return ExecuteProcess(startInfo);
        }

        /// <summary>
        /// Internal method to startup and execute a process for the command
        /// </summary>
        /// <param name="startInfo">Process info for startup</param>
        /// <returns>Standard output</returns>
        private static async Task<IEnumerable<string>> ExecuteProcess(System.Diagnostics.ProcessStartInfo startInfo)
        {
            List<string> result = [];

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = startInfo;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            StreamReader reader = process.StandardOutput;

            while (true)
            {
                string? line = reader.ReadLine();
                if (line == null)
                    break;

                result.Add(line);
            }
            await process.WaitForExitAsync();

            return result;
        }

        /// <summary>
        /// Initialize the plugin with the given <see cref="PluginInitContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="PluginInitContext"/> for this plugin.</param>
        public void Init(PluginInitContext context)
        {
            Log.Info("Init", GetType());

            Context = context ?? throw new ArgumentNullException(nameof(context));
            Context.API.ThemeChanged += OnThemeChanged;

            _cache = new CachingService();
            _cache.DefaultCachePolicy.DefaultCacheDurationSeconds = (int)TimeSpan.FromMinutes(1).TotalSeconds;

            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        /// <summary>
        /// Return a list context menu entries for a given <see cref="Result"/> (shown at the right side of the result).
        /// </summary>
        /// <param name="selectedResult">The <see cref="Result"/> for the list with context menu entries.</param>
        /// <returns>A list context menu entries.</returns>
        public List<ContextMenuResult> LoadContextMenus(Result selectedResult)
        {
            Log.Info("LoadContextMenus", GetType());

            if (selectedResult?.ContextData is (JSLTools data, OperationMode mode))
            {
                switch (mode)
                {
                    case OperationMode.Tool:
                        {
                            return [
                                new ContextMenuResult
                                {
                                    PluginName = Name,
                                    Title = "Start locally",
                                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                                    Glyph = "\xe756",
                                    AcceleratorKey = Key.Enter,
                                    Action = _ => StartTool(data)
                                },
                                new ContextMenuResult
                                {
                                    PluginName = Name,
                                    Title = "Open locally",
                                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                                    Glyph = "\xe774",
                                    AcceleratorKey = Key.Enter,
                                    AcceleratorModifiers = ModifierKeys.Control,
                                    Action = _ => OpenTool(data, true)
                                },
                                new ContextMenuResult
                                {
                                    PluginName = Name,
                                    Title = "Open on Testserver",
                                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                                    Glyph = "\xe753",
                                    AcceleratorKey = Key.Enter,
                                    AcceleratorModifiers = ModifierKeys.Shift,
                                    Action = _ => OpenTool(data, false)
                                }
                            ];
                        }
                }
            } else if (selectedResult?.ContextData is (string branch, OperationMode branchMode))
            {
                switch (branchMode)
                {
                    case OperationMode.Branch:
                        {
                            return [
                                new ContextMenuResult
                                {
                                    PluginName = Name,
                                    Title = "Checkout branch",
                                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                                    Glyph = "\xe756",
                                    AcceleratorKey = Key.Enter,
                                    Action = _ => {
                                        CheckoutBranch(branch);
                                        return true;
                                    }
                                },
                                new ContextMenuResult
                                {
                                    PluginName = Name,
                                    Title = "Open Jenkins",
                                    FontFamily = "Segoe Fluent Icons,Segoe MDL2 Assets",
                                    Glyph = "\xE774",
                                    AcceleratorKey = Key.Enter,
                                    AcceleratorModifiers = ModifierKeys.Shift,
                                    Action = _ => {
                                        OpenJenkins(branch);
                                        return true;
                                    }
                                }
                            ];
                        }
                }
            }

            return [];
        }

        private void CheckoutBranch(string branch)
        {
            Log.Info($"Checkout branch: {branch}", GetType());

        }

        private void OpenJenkins(string branch)
        {
            Log.Info($"Open Jenkins: {branch}", GetType());
            StringBuilder sb = new();
            sb.Append(JenkinsUrl);

            if (!JenkinsUrl.EndsWith("job/") && !JenkinsUrl.EndsWith("job"))
            {
                if (!JenkinsUrl.EndsWith('/'))
                    sb.Append('/');
                sb.Append("job/");
            }

            sb.Append(HttpUtility.UrlEncode(branch));

            Helper.OpenCommandInShell(BrowserInfo.Path, BrowserInfo.ArgumentsPattern, sb.ToString());
        }

        /// <summary>
        /// Creates setting panel.
        /// </summary>
        /// <returns>The control.</returns>
        /// <exception cref="NotImplementedException">method is not implemented.</exception>
        public Control CreateSettingPanel() => throw new NotImplementedException();

        /// <summary>
        /// Updates settings.
        /// </summary>
        /// <param name="settings">The plugin settings.</param>
        public void UpdateSettings(PowerLauncherPluginSettings settings)
        {
            Log.Info("UpdateSettings", GetType());

            GitRepoUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(GitRepoUrl))?.TextValue ?? "";
            JenkinsUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(JenkinsUrl))?.TextValue ?? "";
            FolderPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(FolderPath))?.TextValue ?? "";
            TestServerUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(TestServerUrl))?.TextValue ?? "";
            DownloadScriptPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(DownloadScriptPath))?.TextValue ?? "";
            ToolsPorts = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(ToolsPorts))?.TextValueAsMultilineList ?? [];
        }

        /// <inheritdoc/>
        public void ReloadData()
        {
            if (Context is null)
            {
                return;
            }

            UpdateIconPath(Context.API.GetCurrentTheme());
            BrowserInfo.UpdateIfTimePassed();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Log.Info("Dispose", GetType());

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Wrapper method for <see cref="Dispose()"/> that dispose additional objects and events form the plugin itself.
        /// </summary>
        /// <param name="disposing">Indicate that the plugin is disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed || !disposing)
            {
                return;
            }

            if (Context?.API != null)
            {
                Context.API.ThemeChanged -= OnThemeChanged;
            }

            Disposed = true;
        }

        private void UpdateIconPath(Theme theme) => IconPath = theme == Theme.Light || theme == Theme.HighContrastWhite ? Context?.CurrentPluginMetadata.IcoPathLight : Context?.CurrentPluginMetadata.IcoPathDark;

        private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

        private bool StartTool(JSLTools tool)
        {
            Log.Info($"Start tool: {tool}", GetType());
            return false;
        }

        private bool OpenTool(JSLTools tool, bool isLocal)
        {
            string url = "";
            int port = toolsPorts[tool];
            if (isLocal)
                url = "https://localhost";
            else
                url = TestServerUrl;

            url += $":{port}";

            Log.Info($"Open tool {url}", GetType());

            return true;
        }
    }
}
