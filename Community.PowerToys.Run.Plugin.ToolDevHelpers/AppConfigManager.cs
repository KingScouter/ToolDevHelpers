using CommonLib.Models;
using System.IO;
using System.Text.Json;
using Wox.Plugin.Logger;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class AppConfigManager
    {
        public AppConfig Config { get; set; } = new();

        /// <summary>
        /// Handle a new tool config-file
        /// </summary>
        /// <param name="appConfig">App configuration</param>
        public void HandleConfigFile()
        {
            if (string.IsNullOrWhiteSpace(Config.ToolConfigFile))
            {
                Log.Info("Project file not set => Skip tool configuration", GetType());
                Config.ToolConfigProject = null;
                return;
            }

            try
            {
                ToolConfigProject? project = ToolConfigProject.ReadToolConfigProject(Config.ToolConfigFile);
                if (project != null)
                {
                    Config.ToolConfigProject = project;
                    Log.Info($"Tool config project {Config.ToolConfigFile} loaded", GetType());
                }
            }
            catch (FileNotFoundException)
            {
                Log.Info("Project file not found => Create sample project in place", GetType());
                // Write sample project
                ToolConfigProject sampleProject = new();
                sampleProject.AddToolConfig(new ToolConfig()
                {
                    shortName = "test",
                    name = "Test tool name",
                    useHttps = true,
                    port = 1234,
                    remoteServerUrl = "www.google.at",
                    exePath = "testTool/tool.exe",
                    additionalPages = ["#BASE#api", "#BASE_HOST#:#BASE_PORT#/test"]
                });

                StreamWriter sw = new(Config.ToolConfigFile);
                sw.Write(JsonSerializer.Serialize(sampleProject));
                sw.Close();
            }
            catch (JsonException ex)
            {
                Log.Info($"Project file invalid: {ex.Message}", GetType());
            }
            catch (Exception ex)
            {
                Log.Info($"Unknown exception occured during loading of tool config project: {ex.Message}", GetType());
            }
        }
    }
}
