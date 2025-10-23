namespace CommonLib.Models
{
    public class AppConfig : IAppConfig
    {
        public string ProjectConfigPath { get; set; } = "";
        public ProjectConfigFile? ProjectConfigFile { get; set; }
        public PowershellVersion ShellType { get; set; } = PowershellVersion.Legacy;


        public IEnumerable<SelectionEntry> GetProjects()
        {
            if (ProjectConfigFile == null || ProjectConfigFile.ProjectConfigMap == null)
                return [];

            List<SelectionEntry> entries = [];

            foreach (var elem in ProjectConfigFile.ProjectConfigMap.Configs)
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
