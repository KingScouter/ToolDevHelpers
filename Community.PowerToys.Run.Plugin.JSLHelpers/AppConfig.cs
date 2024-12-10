using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class AppConfig
    {
        private readonly Dictionary<string, JSLTools> toolNames = new()
        {
            { "MT", JSLTools.MT },
            { "MRCCT", JSLTools.MRCCT},
            { "HMI", JSLTools.HMI },
        };

        // Settings values
        public string GitRepoUrl { get; set; } = "";
        public string JenkinsUrl { get; set; } = "";
        public string FolderPath { get; set; } = "";
        public string TestServerUrl { get; set; } = "";
        public string DownloadScriptPath { get; set; } = "";
        public List<string> ToolsPorts
        {
            get
            {
                List<string> val = new List<string>();
                foreach (var tool in toolsPorts)
                {
                    string toolName = toolNames.First(elem => elem.Value == tool.Key).Key;
                    val.Add($"{toolName} {tool.Value}");
                }

                return val;
            }
            set
            {
                toolsPorts.Clear();
                foreach (var tool in value)
                {
                    string[] splitted = tool.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    if (splitted.Length != 2)
                        continue;

                    int port;
                    if (!int.TryParse(splitted[1], out port))
                        continue;

                    JSLTools toolName;
                    if (!toolNames.TryGetValue(splitted[0].ToUpper(), out toolName))
                        continue;

                    toolsPorts.Add(toolName, port);
                }
            }
        }


        public Dictionary<JSLTools, int> toolsPorts = [];
    }
}
