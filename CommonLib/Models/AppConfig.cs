﻿namespace CommonLib.Models
{
    public class AppConfig
    {
        public string GitRepoUrl { get; set; } = "";
        public string SourceFolder { get; set; } = "";
        public string JenkinsUrl { get; set; } = "";
        public string FolderPath { get; set; } = "";
        public string DownloadScriptPath { get; set; } = "";
        public string ToolConfigFile { get; set; } = "";
        public ToolConfigProject? ToolConfigProject = null;
        public PowershellVersion ShellType { get; set; } = PowershellVersion.Legacy;
    }
}
