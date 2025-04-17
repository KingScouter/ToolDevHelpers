using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ToolDevHelpersCmdPal
{
    internal class ExtensionSettings : JsonSettingsManager
    {
        internal static ExtensionSettings Instance = new();

        private string sourceFolder = "";

        public string SourceFolder => sourceFolder;

        private readonly TextSetting sourceFolderSettingSource = new(
            "sourceFolder",
            "Source Folder",
            "Local source folder",
            "");


        private bool enableTest;

        public bool EnableTest => enableTest;

        private string testStringSetting = "";

        public string TestStringSetting => testStringSetting;

        private readonly ToggleSetting enableTestSettingSource = new(
            "enableTest",
            "Enable Test",
            "This enables the test",
            true
        );

        internal static string SettingsJsonPath()
        {
            string directory = Utilities.BaseSettingsPath("ToolDevHelpers");
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, "settings.json");
        }

        public ExtensionSettings()
        {
            FilePath = SettingsJsonPath();
            Settings.Add(enableTestSettingSource);
            Settings.Add(sourceFolderSettingSource);

            LoadSettings();

            Settings.SettingsChanged += (s, a) =>
            {
                SaveSettings();
            };
        }
    }
}
