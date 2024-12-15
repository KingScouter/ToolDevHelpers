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
        private readonly AppConfig appConfig = new();

        private readonly BranchQueryHandler branchQueryHandler = new();
        private readonly ToolQueryHandler toolQueryHandler = new();

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
                Key = nameof(appConfig.GitRepoUrl),
                DisplayLabel = "GIT Repository URL",
                DisplayDescription = "URL for the GIT repository",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = appConfig.GitRepoUrl
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
                Key = nameof(appConfig.TestServerUrl),
                DisplayLabel = "Testserver URL",
                DisplayDescription = "URL of the testserver where the tools are running",
                PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
                TextValue = appConfig.TestServerUrl
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
                case "b": return branchQueryHandler.HandleQuery(query.Terms.Skip(1), appConfig);
                case "t": return toolQueryHandler.HandleQuery(query.Terms.Skip(1), appConfig);
            }

            return [];
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

            if (selectedResult?.ContextData is (JSLTools data, OperationMode mode) && mode == OperationMode.Tool)
            {
                return toolQueryHandler.LoadContextMenus(data, Name, appConfig);
            } else if (selectedResult?.ContextData is (string branch, OperationMode branchMode) && branchMode == OperationMode.Branch)
            {
                return branchQueryHandler.LoadContextMenus(branch, Name, appConfig);
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
            appConfig.JenkinsUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.JenkinsUrl))?.TextValue ?? "";
            appConfig.FolderPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.FolderPath))?.TextValue ?? "";
            appConfig.TestServerUrl = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.TestServerUrl))?.TextValue ?? "";
            appConfig.DownloadScriptPath = settings.AdditionalOptions.SingleOrDefault(x => x.Key == nameof(appConfig.DownloadScriptPath))?.TextValue ?? "";
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
