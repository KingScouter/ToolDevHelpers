namespace CommonLib.Models
{
    public interface IAppConfig
    {
        public string? GitRepoUrl { get; }
        public string? SourceFolder { get; }
        public string? JenkinsUrl { get; }
        public string? FolderPath { get; }
        public string? DownloadScriptPath { get; }
        public string? ToolConfigFile { get; }
        public ToolConfigProject? ToolConfigProject { get; }
        public PowershellVersion ShellType { get; }
    }
}
