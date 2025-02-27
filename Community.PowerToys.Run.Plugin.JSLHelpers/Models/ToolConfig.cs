using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.JSLHelpers.Models
{
    internal class ToolConfig
    {
        [JsonInclude]
        internal required string shortName
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
        internal required string name;
        [JsonInclude]
        internal bool useHttps = false;
        [JsonInclude]
        internal uint port = 0;
        [JsonInclude]
        internal string? remoteServerUrl = null;
        [JsonInclude]
        internal required string exePath;
        [JsonInclude]
        internal string[] additionalPages = [];

        private string shortNameInternal = "";
    }
}
