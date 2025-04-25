using CommonLib.Utils;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Linq;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class BranchListItem : ListItem
    {
        public BranchListItem(ICommand command) : base(command)
        {
            
        }

        public BranchListItem(string branchName) : base(new NoOpCommand())
        {
            List<ICommand> moreCommands = [];

            // Create "Download tools" command
            string? scriptPath = ExtensionSettings.Instance.DownloadScriptPath;
            if (!string.IsNullOrEmpty(scriptPath) && System.IO.Path.Exists(scriptPath))
            {
                var downloadBranchCommand = new AnonymousCommand(() =>
                {
                    //ExtensionHost.LogMessage($"Start download with {scriptPath} and branch {branchName} and shelltype {ExtensionSettings.Instance.ShellType}");
                    ProcessUtils.ExecutePowershellCommand($"{scriptPath} {branchName}", ExtensionSettings.Instance.ShellType, title: "Download Tools");
                })
                { Name = "Download branch", Result = CommandResult.Hide()};
                moreCommands.Add(downloadBranchCommand);
            }

            // Create "Open Jenkins" command
            string? jenkinsUrl = UrlUtils.BuildJenkinsUrl(branchName, ExtensionSettings.Instance.JenkinsUrl);
            if (!string.IsNullOrEmpty(jenkinsUrl))
            {
                var openJenkinsCommand = new OpenUrlCommand(jenkinsUrl)
                { Name = "Open Jenkins", Result = CommandResult.Hide() };
                moreCommands.Add(openJenkinsCommand);
            }

            // Create "Open Github" command
            string? gitUrl = UrlUtils.BuildGithubUrl(branchName, ExtensionSettings.Instance.GitRepoUrl);
            if (!string.IsNullOrEmpty(gitUrl))
            {
                var openGithubCommand = new OpenUrlCommand(gitUrl)
                { Name = "Open GitHub", Result = CommandResult.Hide() };
                moreCommands.Add(openGithubCommand);
            }

            if (moreCommands.Count > 0)
            {
                Command = moreCommands.First();
                MoreCommands = moreCommands.Skip(1).Select(x => new CommandContextItem(x)).ToArray();
            }

            Title = branchName;
        }
    }
}
