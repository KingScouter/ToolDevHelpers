using System.IO;
using System.Text.Json;
using System.Windows.Controls;
using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using Wox.Plugin;
using Wox.Plugin.Logger;
using BrowserInfo = Wox.Plugin.Common.DefaultBrowserInfo;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{

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
        private readonly AppConfigManager appConfigManager = new();
        private readonly BranchQueryHandler branchQueryHandler = new();
        private readonly ToolQueryHandler toolQueryHandler = new();
        private readonly MiscQueryHandler miscQueryHandler;

        public Main()
        {
            miscQueryHandler = new(appConfigManager);
        }

        private AppConfig appConfig => appConfigManager.Config;

        // General plugin members
        private PluginInitContext? Context { get; set; }
        private string? IconPath { get; set; } = null;
        private bool Disposed { get; set; }
        /// <summary>
        /// ID of the plugin.
        /// </summary>
        public static string PluginID => "AE953C974C2241878F282EA18A7769E4";

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name => "Tool Dev Helpers";

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description => "Helpers for (web) tool developers";

        /// <summary>
        /// Additional options for the plugin.
        /// </summary>
        public IEnumerable<PluginAdditionalOption> AdditionalOptions
        {
            get
            {
                return [
                    new()
                    {
                        Key = nameof(appConfig.GitRepoUrl),
                        DisplayLabel = "GIT Repository URL",
                        DisplayDescription = "URL for the GIT repository",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.GitRepoUrl
                    },
                    new()
                    {
                        Key = nameof(appConfig.SourceFolder),
                        DisplayLabel = "Local source folder",
                        DisplayDescription = "Local cloned source folder",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.SourceFolder
                    },
                    new()
                    {
                        Key = nameof(appConfig.JenkinsUrl),
                        DisplayLabel = "Jenkins Multibranch-Pipeline URL",
                        DisplayDescription = "URL of the Multibranch-Pipeline on Jenkins",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.JenkinsUrl
                    },
                    new()
                    {
                        Key = nameof(appConfig.FolderPath),
                        DisplayLabel = "Tool folder",
                        DisplayDescription = "Folder to store the tools",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.FolderPath
                    },
                    new()
                    {
                        Key = nameof (appConfig.DownloadScriptPath),
                        DisplayLabel = "Download Script",
                        DisplayDescription = "Path to the download script",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.DownloadScriptPath
                    },
                    new()
                    {
                        Key = nameof (appConfig.ToolConfigFile),
                        DisplayLabel = "Tool config file",
                        DisplayDescription = "Config file with the configuration of all tools. If the file doesn't exist, a sample project will be added at that path.",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                        TextValue = appConfig.ToolConfigFile
                    },
                    new()
                    {
                        Key = nameof (appConfig.ShellType),
                        DisplayLabel = "Powershell Version",
                        DisplayDescription = "Which Powershell Version do you want to use?",
                        PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Combobox,
                        ComboBoxItems = [
                            new("Legacy (powershell)", "0"),
                            new("Powershell 7 (pwsh)", "1")
                        ],
                        ComboBoxValue = -1
                    }
                ];
            }
        }

        /// <summary>
        /// Process the user query
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>List of query results</returns>
        public List<Result> Query(Query query)
        {
            if (query.Terms.Count == 0)
                return [];

            var modeQuery = query.Terms.FirstOrDefault("");

            if (string.IsNullOrWhiteSpace(modeQuery))
                return [];

            if (string.Equals(modeQuery, "bl", StringComparison.CurrentCultureIgnoreCase))
                return branchQueryHandler.HandleQuery(query.Terms.Skip(1), appConfig, true);
            if (string.Equals(modeQuery, "br", StringComparison.CurrentCultureIgnoreCase))
                return branchQueryHandler.HandleQuery(query.Terms.Skip(1), appConfig, false);
            else if (string.Equals(modeQuery, "t", StringComparison.CurrentCultureIgnoreCase))
                return toolQueryHandler.HandleQuery(query.Terms.Skip(1), appConfig.ToolConfigProject);

            return miscQueryHandler.HandleQuery(query.Terms);
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

            branchQueryHandler.Init();

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

            if (selectedResult?.ContextData is (string data, OperationMode mode) && mode == OperationMode.Branch)
            {
                return branchQueryHandler.LoadContextMenus(data, Name, appConfig);
            } else if (selectedResult?.ContextData is (ToolConfig config, OperationMode toolMode) && toolMode == OperationMode.Tool)
            {
                return toolQueryHandler.LoadContextMenus(config, Name, appConfig);
            }

            return [];
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

            appConfig.GitRepoUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.GitRepoUrl))?.TextValue ?? "";
            appConfig.SourceFolder = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.SourceFolder))?.TextValue ?? "";
            appConfig.JenkinsUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.JenkinsUrl))?.TextValue ?? "";
            appConfig.FolderPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.FolderPath))?.TextValue ?? "";
            appConfig.DownloadScriptPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.DownloadScriptPath))?.TextValue ?? "";
            appConfig.ShellType = (PowershellVersion)GetEnumSettingOrDefault(settings, nameof(appConfig.ShellType));
            string newConfigFile = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.ToolConfigFile))?.TextValue ?? "";

            if (newConfigFile != appConfig.ToolConfigFile)
            {
                appConfig.ToolConfigFile = newConfigFile;
                appConfigManager.HandleConfigFile();
            }
        }

        /// <summary>
        /// Get the enum-setting from the additional-options.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="name">Name of the setting to retrieve</param>
        /// <returns>The settings value</returns>
        private int GetEnumSettingOrDefault(PowerLauncherPluginSettings settings, string name)
        {
            var option = settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == name);

            // If a setting isn't available, we use the value defined in the method GetAdditionalOptions() as fallback.
            // We can use First() instead of FirstOrDefault() because the values must exist. Otherwise, we made a mistake when defining the settings.
            return option?.ComboBoxValue ?? AdditionalOptions.First(x => x.Key == name).ComboBoxValue;
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
    }
}
