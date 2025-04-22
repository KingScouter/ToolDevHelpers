using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using Windows.System;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class BranchListItem : ListItem
    {
        public BranchListItem(ICommand command) : base(command)
        {
            
        }

        public BranchListItem(string branchName) : base(new NoOpCommand())
        {
            var openBrowserCommand = new OpenUrlCommand("https://github.com") { Name = "Open in Browser" };
            Command = openBrowserCommand;


            var downloadBranchCommand = new AnonymousCommand(() =>
            {
                // Not implemented yet
            })
            { Name = "Download branch", Result = CommandResult.ShowToast(new ToastArgs() { Message = $"Download branch: {branchName}", Result = CommandResult.KeepOpen() }) };

            var openJenkinsCommand = new AnonymousCommand(() =>
            {
                // Not implemented yet
            })
            { Name = "Open Jenkins", Result = CommandResult.ShowToast(new ToastArgs() { Message = $"Open Jenkins: {branchName}", Result = CommandResult.KeepOpen() }) };

            MoreCommands = [
                new CommandContextItem(downloadBranchCommand) { RequestedShortcut = KeyChordHelpers.FromModifiers(false, false, true, false, (int)VirtualKey.Enter, 0)},
                new CommandContextItem(openJenkinsCommand) { RequestedShortcut = KeyChordHelpers.FromModifiers(false, true, false, false, (int)VirtualKey.Enter, 0)},
            ];

            Title = branchName;
        }
    }
}
