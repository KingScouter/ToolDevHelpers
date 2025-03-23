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
    }
}
