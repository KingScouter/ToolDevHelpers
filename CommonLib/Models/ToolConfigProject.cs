using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CommonLib.Models
{
    public class ToolConfigProject
    {
        [JsonInclude]
        internal ToolConfig[] toolConfigs
        {
            get
            {
                return [.. toolConfigMap.configs];
            }
            set
            {
                toolConfigMap.configs = [.. value];
            }
        }

        ToolConfigMap toolConfigMap = new();

        public static ToolConfigProject? ReadToolConfigProject(string filename)
        {
            StreamReader sr = new(filename);
            string dataLine = sr.ReadToEnd();
            sr.Close();
            if (dataLine != null)
            {
                ToolConfigProject? project = JsonSerializer.Deserialize<ToolConfigProject>(dataLine);
                return project;
            }

            return null;
        }

        /// <summary>
        /// Add a new tool-config to the project.
        /// </summary>
        /// <param name="config">Tool-config to add</param>
        /// <param name="generateName">Flag if a shortname and name should be automatically get generated for the
        /// new config.</param>
        /// <returns>True if the tool-config was added successfully, otherwise false.</returns>
        public bool AddToolConfig(ToolConfig config, bool generateName = false)
        {
            if (toolConfigMap.HasKey(config.shortName) && !generateName)
                return false;

            if (generateName)
            {
                string pattern = @"new_(\d)+";
                Regex r = new(pattern, RegexOptions.IgnoreCase);

                int maxIdx = 0;

                if (toolConfigMap.configs.Count > 0)
                {
                    maxIdx = toolConfigMap.configs.Select((config) =>
                    {
                        Match m = r.Match(config.shortName);
                        if (m.Success)
                            return int.Parse(m.Groups[1].Value);

                        return 0;
                    }).Max();
                    maxIdx++;
                }

                config.shortName = $"new_{maxIdx}";
                config.name = $"New {maxIdx}";
            }

            toolConfigMap.Add(config);
            return true;
        }

        /// <summary>
        /// Get the configuration for a tool by its shortname
        /// </summary>
        /// <param name="shortName">Name of the tool to retrieve</param>
        /// <returns>Tool configuration (or null if the tool could not be found)</returns>
        public ToolConfig? GetToolConfig(string shortName)
        {
            ToolConfig? config = toolConfigMap.Get(shortName.ToLower());
            return config;
        }

        /// <summary>
        /// Remove a tool from the project.
        /// </summary>
        /// <param name="shortName">Shortname of the tool to remove</param>
        public void RemoveToolConfig(string shortName)
        {
            toolConfigMap.Remove(shortName.ToLower());
        }

        /// <summary>
        /// Get all tool configurations which names matchthe filter query
        /// </summary>
        /// <param name="filterQuery">Query to filter the tool names with</param>
        /// <returns>List of tool configurations</returns>
        public List<ToolConfig> GetToolConfigs(string? filterQuery = null)
        {
            if (string.IsNullOrWhiteSpace(filterQuery))
                return [.. toolConfigMap.configs];

            return [.. toolConfigMap.configs.Where(config => config.shortName.Contains(filterQuery))];
        }

        /// <summary>
        /// Checks if a given key exists in the list of tool configs.
        /// </summary>
        /// <param name="key">Key to check for</param>
        /// <returns>True if the key exists in the list, otherwise false</returns>
        public bool HasToolConfigKey(string key)
        {
            return toolConfigMap.HasKey(key);
        }
    }
}
