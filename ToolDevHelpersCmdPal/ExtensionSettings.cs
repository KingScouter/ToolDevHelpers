using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ToolDevHelpersCmdPal
{
    internal class ExtensionSettings : JsonSettingsManager
    {
        internal static ExtensionSettings Instance = new();

        public string? SourceFolder => _sourceFolderSettingSource.Value;

        private readonly TextSetting _sourceFolderSettingSource = new(
            "sourceFolder",
            "Source Folder",
            "Local source folder",
            "");

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

            LoadSettings();

            Settings.SettingsChanged += (s, a) => SaveSettings();
        }
    }
}
