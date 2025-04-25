using CommonLib.Models;

namespace CommonLib.Utils
{
    public class ProcessUtils
    {
        /// <summary>
        /// Execute a command in the windows command-line
        /// </summary>
        /// <param name="cmd">Command template</param>
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <returns>Standard output</returns>
        public static Task<IEnumerable<string>> ExecuteCmdCommandAsync(string cmd, string? workingDir = null)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = "cmd.exe",
                RedirectStandardOutput = true,
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                Arguments = $"/C {cmd}",
            };

            if (!string.IsNullOrWhiteSpace(workingDir))
                startInfo.WorkingDirectory = workingDir;

            return ExecuteProcessAsync(startInfo);
        }

        /// <summary>
        /// Execute a command in the windows PowerShell
        /// </summary>
        /// <param name="commandTemplate">Command template</param>
        /// <param name="version">Version of the Powershell to use</param>"
        /// <param name="waitForResult">Flag to determine if the CLI should remain open after the execution finished</param>
        /// <param name="workingDir">Working directory</param>
        /// <param name="title">Optional title for the PowerShell window</param>
        public static void ExecutePowershellCommand(string commandTemplate, PowershellVersion version, string? workingDir = null, string? title = null)
        {
            string extraArgs = "";
            string fileName = "powershell.exe";
            string titleCmd = "";

            if (version == PowershellVersion.LTS)
            {
                extraArgs = "-command ";
                fileName = "pwsh.exe";
            }

            if (!string.IsNullOrWhiteSpace(title))
                titleCmd += $"$host.ui.RawUI.WindowTitle = '{title}'; ";

            System.Diagnostics.ProcessStartInfo startInfo = new()
            {
                FileName = fileName,
                Arguments = $"-ExecutionPolicy Bypass {extraArgs}\"{titleCmd} {commandTemplate}\""
            };

            if (!string.IsNullOrWhiteSpace(workingDir))
                startInfo.WorkingDirectory = workingDir;

            startInfo.UseShellExecute = false;

            ExecuteProcess(startInfo);
        }

        /// <summary>
        /// Internal method to startup and execute a process for the command
        /// </summary>
        /// <param name="startInfo">Process info for startup</param>
        /// <returns>Standard output</returns>
        private static async Task<IEnumerable<string>> ExecuteProcessAsync(System.Diagnostics.ProcessStartInfo startInfo)
        {
            List<string> result = [];

            System.Diagnostics.Process process = new()
            {
                StartInfo = startInfo,
            };
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            StreamReader reader = process.StandardOutput;

            while (true)
            {
                string? line = reader.ReadLine();
                if (line == null)
                    break;

                result.Add(line);
            }
            await process.WaitForExitAsync();

            return result;
        }

        /// <summary>
        /// Internal method to startup and execute a process for the command
        /// </summary>
        /// <param name="startInfo">Process info for startup</param>
        /// <returns>Standard output</returns>
        private static void ExecuteProcess(System.Diagnostics.ProcessStartInfo startInfo)
        {
            System.Diagnostics.Process process = new()
            {
                StartInfo = startInfo
            };
            process.Start();
        }
    }
}
