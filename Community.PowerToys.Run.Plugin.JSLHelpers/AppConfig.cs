namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class AppConfig
    {
        // Settings values
        public string GitRepoUrl { get; set; } = "";
        public string JenkinsUrl { get; set; } = "";
        public string FolderPath { get; set; } = "";
        public string RemoteServerUrl { get; set; } = "";
        public string DownloadScriptPath { get; set; } = "";
        public string ToolConfigFile { get; set; } = "";
        public ToolConfigProject? ToolConfigProject = null;
    }
}
