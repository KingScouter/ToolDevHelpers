using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CommonLib.Models
{
  [JsonSourceGenerationOptions(WriteIndented = true)]
  [JsonSerializable(typeof(ProjectConfiguration))]
  [JsonSerializable(typeof(ProjectConfig))]
  internal partial class ProjectConfigurationSourceGenerationContext : JsonSerializerContext
  {
  }
    
  public class ProjectConfiguration
  {
    [JsonInclude]
    internal BaseProjectConfig[] projects
    {
        get
        {
            return [.. projectConfigMap.Configs];
        }
        set
        {
            projectConfigMap.Configs = [.. value];
        }
    }

    ProjectConfigMap projectConfigMap = new();

    [JsonIgnore]
    public ProjectConfigMap ProjectConfigMap { get { return projectConfigMap; } }

    /// <summary>
    /// Serialize a project to JSON
    /// </summary>
    /// <param name="project">Project to serialize</param>
    /// <returns>JSON string</returns>
    public static string SerializeJson(ProjectConfiguration project)
    {
        return JsonSerializer.Serialize(project, ProjectConfigurationSourceGenerationContext.Default.ProjectConfiguration);
    }

    /// <summary>
    /// Deserialize a JSON string into a ProjectConfiguration
    /// </summary>
    /// <param name="json">JSON string to project</param>
    /// <returns>Deserialized project</returns>
    public static ProjectConfiguration? DeserializeJson(string json)
    {
        try
        {
            var project = JsonSerializer.Deserialize(json, ProjectConfigurationSourceGenerationContext.Default.ProjectConfiguration);
            return project;
        } catch { return null; }
    }
        

    public static ProjectConfiguration? ReadProjectConfiguration(string filename)
    {
        StreamReader sr = new(filename);
        string dataLine = sr.ReadToEnd();
        sr.Close();
        if (dataLine != null)
        {
            ProjectConfiguration? project = ProjectConfiguration.DeserializeJson(dataLine);
            return project;
        }

        return null;
    }

    /// <summary>
    /// Save a template project in a given file
    /// </summary>
    /// <param name="filename">File to save the project into</param>
    public static void SaveTemplateProject(string filename)
    {
      ProjectConfiguration sampleProject = new();
      sampleProject.AddConfig(new ProjectConfig()
      {
        key = "test",
        label = "Test Project",
        gitRepoUrl = "https://github.com",
        jenkinsUrl = "jenkins.com",
        localSourceFolder = "C:/AMR",
        downloadScriptPath = "test.ps1",
        toolConfigs = [],
        toolFolderPath = ""
      });

      StreamWriter sw = new(filename);
      sw.Write(ProjectConfiguration.SerializeJson(sampleProject));
      sw.Close();
    }
    
    /// <summary>
    /// Add a new project-config to the project.
    /// </summary>
    /// <param name="config">Project-config to add</param>
    /// <param name="generateName">Flag if a key and label should be automatically get generated for the
    /// new config.</param>
    /// <returns>True if the project-config was added successfully, otherwise false.</returns>
    public bool AddConfig(ProjectConfig config, bool generateName = false)
    {
        if (projectConfigMap.HasKey(config.key) && !generateName)
            return false;

        if (generateName)
        {
            string pattern = @"new_(\d)+";
            Regex r = new(pattern, RegexOptions.IgnoreCase);

            int maxIdx = 0;

            if (projectConfigMap.Configs.Count > 0)
            {
                maxIdx = projectConfigMap.Configs.Max((config) =>
                {
                    Match m = r.Match(config.key);
                    if (m.Success)
                        return int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);

                    return 0;
                });
                maxIdx++;
            }

            config.key = $"new_{maxIdx}";
            config.label = $"New {maxIdx}";
        }

        projectConfigMap.Add(config);
        return true;
    }
  }
}