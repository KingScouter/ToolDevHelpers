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
        public required string name;
        [JsonInclude]
        public bool useHttps = false;
        [JsonInclude]
        public uint port = 0;
        [JsonInclude]
        public string? remoteServerUrl = null;
        [JsonInclude]
        public required string exePath;
        [JsonInclude]
        public string[] additionalPages = [];

        public string Name
        {
            get { return name; }
        }

        public bool UseHttps
        {
            get { return useHttps; }
        }

        public uint Port
        {
            get { return port; }
        }

        public string? RemoteServerUrl
        {
            get { return remoteServerUrl; }
        }

        public string ExePath
        {
            get { return exePath; }
        }

        private string shortNameInternal = "";
    }
}
