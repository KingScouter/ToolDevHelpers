using CommonLib.Models;
using System.IO;
using System.Text.Json;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.ToolDevHelpers
{
    public sealed class AppConfigManager
    {
        public AppConfig Config { get; set; } = new();

        /// <summary>
        /// Handle a new project config-file
        /// </summary>
        /// <param name="appConfig">App configuration</param>
        public void HandleConfigFile()
        {
            if (string.IsNullOrWhiteSpace(Config.ProjectConfigPath))
            {
                Log.Info("Project file not set => Skip project configuration", GetType());
                Config.ProjectConfigFile = null;
                return;
            }

            try
            {
                ProjectConfigFile? project = ProjectConfigFile.ReadProjectConfigFile(Config.ProjectConfigPath);
                if (project != null)
                {
                    Config.ProjectConfigFile = project;
                    Log.Info($"Project config {Config.ProjectConfigPath} loaded", GetType());
                }
            }
            catch (FileNotFoundException)
            {
                Log.Info("Project file not found => Create sample project in place", GetType());
                ProjectConfigFile.SaveTemplateProject(Config.ProjectConfigPath);
            }
            catch (JsonException ex)
            {
                Log.Info($"Project file invalid: {ex.Message}", GetType());
            }
            catch (Exception ex)
            {
                Log.Info($"Unknown exception occured during loading of project config: {ex.Message}", GetType());
            }
        }
    }
}
