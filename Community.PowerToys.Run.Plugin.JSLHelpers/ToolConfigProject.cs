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

        internal ToolConfig? GetToolConfig(string shortName)
        {
            ToolConfig? config = null;
            if (toolConfigs.TryGetValue(shortName.ToLower(), out config))
                return config;

            return null;
        }

        internal List<ToolConfig> GetToolConfigs(string filterQuery)
        {
            if (string.IsNullOrWhiteSpace(filterQuery))
                return [.. toolConfigs.Values];

            return [.. toolConfigs.Values.Where(config => config.shortName.Contains(filterQuery))];
        }
    }
}
