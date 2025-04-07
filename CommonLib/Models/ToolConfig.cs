using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace CommonLib.Models
{
    public class ToolConfig
    {
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public required string shortName
        {
            get
            {
                return shortNameInternal;
            }
            set
            {
                shortNameInternal = value.ToLowerInvariant();
            }
        }
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public required string name { get; set; }
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public bool useHttps { get; set; } = false;
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public uint port { get; set; } = 0;
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public string? remoteServerUrl { get; set; } = null;
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public required string exePath { get; set; }
        [JsonInclude]
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
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
