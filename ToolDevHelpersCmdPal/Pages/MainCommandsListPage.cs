using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new ListItem(new AnonymousCommand(action: () => { 
                    Title = "Tools";
                }) { Result = CommandResult.KeepOpen() }) { Title = "Tools" },
                new ListItem(new AnonymousCommand(action: () => { Title = "Reload tool config"; }) { Result = CommandResult.KeepOpen() }) { Title = "Reload tool config" }
            ];
        }

        public override IListItem[] GetItems()
        {
            return items.ToArray();
        }
    }
}
