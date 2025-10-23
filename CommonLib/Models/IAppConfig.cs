namespace CommonLib.Models
{
    public interface IAppConfig
    {
        public string? ProjectConfigPath { get; }
        public ProjectConfigFile? ProjectConfigFile { get; }
        public PowershellVersion ShellType { get; }
    }
}
