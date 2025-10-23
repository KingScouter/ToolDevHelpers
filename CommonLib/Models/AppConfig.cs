namespace CommonLib.Models
{
    public class AppConfig : IAppConfig
    {
        public string ProjectConfigFile { get; set; } = "";
        public ProjectConfiguration? ProjectConfiguration { get; set; }
        public PowershellVersion ShellType { get; set; } = PowershellVersion.Legacy;


        public IEnumerable<SelectionEntry> GetProjects()
        {
            if (ProjectConfiguration == null || ProjectConfiguration.ProjectConfigMap == null)
                return [];

            List<SelectionEntry> entries = [];

            foreach (var elem in ProjectConfiguration.ProjectConfigMap.Configs)
            {
                entries.Add(new SelectionEntry()
                {
                    Value = elem.key,
                    Label = elem.label
                });
            }

            return entries;
        }
    }
}
