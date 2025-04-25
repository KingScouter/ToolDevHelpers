using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.IO;
using CommonLib.Models;

namespace ToolDevHelpersCmdPal
{
    internal class ExtensionSettings : JsonSettingsManager, IAppConfig
    {
        internal static ExtensionSettings Instance = new();

        // Properties
        public string? SourceFolder => _sourceFolderSettingSource.Value;

        public string? GitRepoUrl => _gitRepoUrlSettingSource.Value;

        public string? JenkinsUrl => _jenkinsUrlSettingSource.Value;

        public string? FolderPath => _folderPathSettingSource.Value;

        public string? DownloadScriptPath => _downloadScriptPathSettingSource.Value;

        public string? ToolConfigFile => _toolConfigFileSettingSource.Value;

        public ToolConfigProject? ToolConfigProject => throw new NotImplementedException();

        public PowershellVersion ShellType => ConvertPowershellVersion(_shellTypeSettingSource.Value);

        // Setting sources
        private readonly TextSetting _sourceFolderSettingSource = new(
            "sourceFolder",
            "Source Folder",
            "Local source folder",
            "");

        private readonly TextSetting _gitRepoUrlSettingSource = new(
            "gitRepoUrl",
            "GIT Repository URL",
            "URL for the GIT repository",
            "");

        private readonly TextSetting _jenkinsUrlSettingSource = new(
            "jenkinsUrl",
            "Jenkins Multibranch-Pipeline URL",
            "URL of the Multibranch-Pipeline on Jenkins",
            "");

        private readonly TextSetting _folderPathSettingSource = new(
            "folderPath",
            "Tool folder",
            "Folder to store the tools",
            "");

        private readonly TextSetting _downloadScriptPathSettingSource = new(
            "downloadScriptPath",
            "Download Script",
            "Path to the download script",
            "");

        private readonly TextSetting _toolConfigFileSettingSource = new(
            "toolConfigFile",
            "Tool config file",
            "Config file with the configuration of all tools. If the file doesn't exist, a sample project will be added at that path.",
            "");

        private readonly ChoiceSetSetting _shellTypeSettingSource = new(
            "shellType",
            "Powershell Version",
            "Which Powershell Version do you want to use?",
            [
                new("Legacy (powershell)", $"{(int)PowershellVersion.Legacy}"),
                new("Powershell 7 (pwsh)", $"{(int)PowershellVersion.LTS}")
            ]);
        

        internal static string SettingsJsonPath()
        {
            string directory = Utilities.BaseSettingsPath("ToolDevHelpers");
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, "settings.json");
        }

        public ExtensionSettings()
        {
            FilePath = SettingsJsonPath();
            Settings.Add(_sourceFolderSettingSource);
            Settings.Add(_gitRepoUrlSettingSource);
            Settings.Add(_jenkinsUrlSettingSource);
            Settings.Add(_folderPathSettingSource);
            Settings.Add(_downloadScriptPathSettingSource);
            Settings.Add(_toolConfigFileSettingSource);
            Settings.Add(_shellTypeSettingSource);


            LoadSettings();

            Settings.SettingsChanged += (s, a) => SaveSettings();
        }

        /// <summary>
        /// Converts the string value from the settings into the Powershell Version.
        /// </summary>
        /// <param name="setting">Setting value as a string</param>
        /// <returns>The converted value</returns>
        private static PowershellVersion ConvertPowershellVersion(string? setting)
        {
            if (string.IsNullOrEmpty(setting))
                return PowershellVersion.Legacy;

            if (!int.TryParse(setting, out int shellTypeValue))
                return PowershellVersion.Legacy;

            return (PowershellVersion)shellTypeValue;
        }
    }
}
