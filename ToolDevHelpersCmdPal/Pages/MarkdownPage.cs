using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class MarkdownPage : ContentPage
    {
        public MarkdownPage()
        {
            Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
            Title = "Markdown page";
            Name = "Preview file";

            Commands = [
                new CommandContextItem(new OpenUrlCommand("E:\\Users\\ortne\\Downloads\\zoomit.png")) { Title = "Open in File Explorer" },
            ];
        }

        public override IContent[] GetContent()
        {
            return [
                new MarkdownContent("# Hello, world!\n This is a **markdown** document.\nI live at `E:\\Users\\ortne\\Downloads\\zoomit.png`"),
                new MarkdownContent("## Second block\n This is another block of content."),
            ];
        }
    }
}
