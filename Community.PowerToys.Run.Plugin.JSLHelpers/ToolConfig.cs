using System.Text.Json.Serialization;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class ToolConfig
    {
        [JsonInclude]
        internal required string shortName { 
            get {
                return shortNameInternal;
            } 
            set {
                shortNameInternal = value.ToLower();
            } 
        }
        [JsonInclude]
        internal required string name;
        [JsonInclude]
        internal required bool useHttps;
        [JsonInclude]
        internal required uint port;
        [JsonInclude]
        internal required string remoteServerUrl;
        [JsonInclude]
        internal required string exePath;
        [JsonInclude]
        internal required string[] additionalPages;

        private string shortNameInternal = "";
    }
}
