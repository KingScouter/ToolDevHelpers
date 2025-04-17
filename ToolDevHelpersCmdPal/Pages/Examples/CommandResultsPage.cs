using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class CommandResultsPage : ListPage
    {
        public CommandResultsPage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
            Title = "Example command results";
            Name = "Open";
        }

        public override IListItem[] GetItems()
        {
            ConfirmationArgs confirmArgs = new()
            {
                PrimaryCommand = new AnonymousCommand(
                    () =>
                    {
                        ToastStatusMessage t = new("The dialog was confirmed");
                        t.Show();
                    })
                {
                    Name = "Confirm",
                    Result = CommandResult.KeepOpen()
                },
                Title = "You can set a title for the dialog",
                Description = "Are you really sure you want to do the thing?"
            };

            return [
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.KeepOpen() }) { Title = "Keep the palette open" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.Hide() }) { Title = "Hide the palette" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.GoBack() }) { Title = "Go back" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.GoHome() }) { Title = "Go home" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.Dismiss() }) { Title = "Dismiss the palette" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.ShowToast("What's up") }) { Title = "Show a toast" },
                new ListItem(new AnonymousCommand(null) { Result = CommandResult.Confirm(confirmArgs) }) { Title = "Confirm something" },
            ];
        }
    }
}
