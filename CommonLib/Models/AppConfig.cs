namespace CommonLib.Models
{
    public class AppConfig : IAppConfig
    {
        public string GitRepoUrl { get; set; } = "";
        public string SourceFolder { get; set; } = "";
        public string JenkinsUrl { get; set; } = "";
        public string FolderPath { get; set; } = "";
        public string DownloadScriptPath { get; set; } = "";
        public string ToolConfigFile { get; set; } = "";
        public ToolConfigProject? ToolConfigProject { get; set; }
        public PowershellVersion ShellType { get; set; } = PowershellVersion.Legacy;
    }
}
