using System.Text.Json;
using System.Text.Json.Serialization;

namespace CommonLib.Models
{
    public class ToolConfigProject
    {
        [JsonInclude]
        internal ToolConfig[] toolConfigs { 
            get 
            {
                return [.. toolConfigMap.Values];
            }
            set
            {
                toolConfigMap = value.ToDictionary(elem => elem.shortName);
            }
        }

        internal Dictionary<string, ToolConfig> toolConfigMap = [];

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
        /// <returns>True if the tool-config was added successfully, otherwise false.</returns>
        public bool AddToolConfig(ToolConfig config)
        {
            if (toolConfigMap.ContainsKey(config.shortName))
                return false;

            toolConfigMap.Add(config.shortName, config);
            return true;
        }

        /// <summary>
        /// Get the configuration for a tool by its shortname
        /// </summary>
        /// <param name="shortName">Name of the tool to retrieve</param>
        /// <returns>Tool configuration (or null if the tool could not be found)</returns>
        public ToolConfig? GetToolConfig(string shortName)
        {
            ToolConfig? config = null;
            if (toolConfigMap.TryGetValue(shortName.ToLower(), out config))
                return config;

            return null;
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
                return [.. toolConfigMap.Values];

            return [.. toolConfigMap.Values.Where(config => config.shortName.Contains(filterQuery))];
        }
    }
}
