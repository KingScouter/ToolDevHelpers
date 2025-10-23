namespace CommonLib.Models
{
    public interface IAppConfig
    {
        public string? ProjectConfigFile { get; }
        public ProjectConfiguration? ProjectConfiguration { get; }
        public PowershellVersion ShellType { get; }
    }
}
