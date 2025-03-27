using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CommonLib.Models
{
    public class ToolConfig
    {
        [JsonInclude]
        public required string shortName
        {
            get
            {
                return shortNameInternal;
            }
            set
            {
                shortNameInternal = value.ToLower();
            }
        }
        [JsonInclude]
        public required string name { get; set; }
        [JsonInclude]
        public bool useHttps { get; set; } = false;
        [JsonInclude]
        public uint port { get; set; } = 0;
        [JsonInclude]
        public string? remoteServerUrl { get; set; } = null;
        [JsonInclude]
        public required string exePath { get; set; }
        [JsonInclude]
        public string[] additionalPages { get; set; } = [];

        private string shortNameInternal = "";

        /// <summary>
        /// Copy constructor for ToolConfig.
        /// </summary>
        /// <param name="config">Tool config to copy</param>
        [SetsRequiredMembers]
        public ToolConfig(ToolConfig config)
        {
            shortName = config.shortName;
            name = config.name;
            useHttps = config.useHttps;
            port = config.port;
            remoteServerUrl = config.remoteServerUrl;
            exePath = config.exePath;
            additionalPages = [.. config.additionalPages];
        }

        /// <summary>
        /// Default constructor for ToolConfig
        /// </summary>
        public ToolConfig()
        {
        }
    }
}
