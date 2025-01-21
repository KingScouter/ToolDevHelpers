namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class AppConfig
    {
        public string GitRepoUrl { get; set; } = "";
        public string JenkinsUrl { get; set; } = "";
        public string FolderPath { get; set; } = "";
        public string DownloadScriptPath { get; set; } = "";
        public string ToolConfigFile { get; set; } = "";
        public ToolConfigProject? ToolConfigProject = null;
        public PowershellVersion ShellType { get; set; } = PowershellVersion.Legacy;
    }
}
