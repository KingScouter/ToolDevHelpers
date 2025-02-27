using Community.PowerToys.Run.Plugin.JSLHelpers.Models;
using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class ToolConfigProject
    {
        [JsonInclude]
        internal Dictionary<string, ToolConfig> toolConfigs = [];

        internal void AddToolConfig(ToolConfig config)
        {
            toolConfigs.Add(config.shortName, config);
        }

        /// <summary>
        /// Get the configuration for a tool by its shortname
        /// </summary>
        /// <param name="shortName">Name of the tool to retrieve</param>
        /// <returns>Tool configuration (or null if the tool could not be found)</returns>
        internal ToolConfig? GetToolConfig(string shortName)
        {
            ToolConfig? config = null;
            if (toolConfigs.TryGetValue(shortName.ToLower(), out config))
                return config;

            return null;
        }

        /// <summary>
        /// Get all tool configurations which names matchthe filter query
        /// </summary>
        /// <param name="filterQuery">Query to filter the tool names with</param>
        /// <returns>List of tool configurations</returns>
        internal List<ToolConfig> GetToolConfigs(string filterQuery)
        {
            if (string.IsNullOrWhiteSpace(filterQuery))
                return [.. toolConfigs.Values];

            return [.. toolConfigs.Values.Where(config => config.shortName.Contains(filterQuery))];
        }
    }
}
