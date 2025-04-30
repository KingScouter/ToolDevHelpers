using Microsoft.CommandPalette.Extensions.Toolkit;
using Microsoft.CommandPalette.Extensions;
using CommonLib.Models;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class ToolListItem : ListItem
    {
        public ToolListItem(ICommand command) : base(command) { }

        public ToolListItem(ToolConfig tool) : base(new NoOpCommand())
        {
            Title = tool.name;
        }
    }
}
