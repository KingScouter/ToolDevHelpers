using CommonLib.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.IO;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class MainCommandsListPage : ListPage
    {
        private List<IListItem> items;

        public MainCommandsListPage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
            Title = "Tool Dev Helpers";
            Name = "Open";

            items = [.. GetDefaultItems()];
        }

        public IListItem[] GetDefaultItems()
        {
            return [
                new ListItem(new BranchListPage()) { Title = "Branches" },
                new ListItem(new ToolListPage()) { Title = "Tools" },
                new ListItem(new AnonymousCommand(action: () => { SomeTests(); }) { Result = CommandResult.KeepOpen() }) { Title = "Reload tool config" }
            ];
        }

        public override IListItem[] GetItems()
        {
            return items.ToArray();
        }

        private void SomeTests()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ExtensionHost.LogMessage($"Exception in tests: {ex.Message}");
            }
        }
    }
}
