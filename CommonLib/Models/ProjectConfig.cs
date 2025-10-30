using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace CommonLib.Models
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(ProjectConfig))]
    [JsonSerializable(typeof(ToolConfig))]
    [JsonSerializable(typeof(string))]
    internal partial class ProjectConfigSourceGenerationContext : JsonSerializerContext
    {
    }
    public class ProjectConfig : BaseProjectConfig
    {
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public required string key { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string label { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string gitRepoUrl { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string localSourceFolder { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string jenkinsUrl { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string toolFolderPath { get; set; } = "";
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string downloadScriptPath { get; set; } = "";

        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public bool useScriptTools { get; set; } = false;

        [JsonInclude]
        internal ToolConfig[] toolConfigs
        {
            get
            {
                return [.. ToolConfigMap.Configs];
            }
            set
            {
                ToolConfigMap.Configs = [.. value];
            }
        }

        [JsonIgnore]
        public ToolConfigMap ToolConfigMap { get; set; } = new();

        public ProjectConfig() { }

        public static string SerializeJson(ProjectConfig config)
        {
            var jsonString = JsonSerializer.Serialize(config, ProjectConfigSourceGenerationContext.Default.ProjectConfig);
            return jsonString;
        }

        public static ProjectConfig? DeserializeJson(string json)
        {
            try
            {
                var config = JsonSerializer.Deserialize(json, ProjectConfigSourceGenerationContext.Default.ProjectConfig);
                return config;
            }
            catch { return null; }
        }
    }
}