// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ToolDevHelpersCmdPal.Pages;

namespace ToolDevHelpersCmdPal;

internal sealed partial class ToolDevHelpersCmdPalPage : ListPage
{
    private List<IListItem> _items;

    public ToolDevHelpersCmdPalPage()
    {
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Title = "ToolDevHelpers";
        Name = "Open";

        _items = [.. GetDefaultItems()];
    }

    public IListItem[] GetDefaultItems()
    {
        var testCommand = new OpenUrlCommand("https://www.github.com");
        var command = new OpenUrlCommand("https://learn.microsoft.com/windows/powertoys/command-palette/adding-commands");
        var showMessageCommand = new ShowMessageCommand();

        AnonymousCommand updateCommand = new(action: () => { Title += " Hello"; }) { Result = CommandResult.KeepOpen() };

        return [
            new ListItem(new SampleContentPage()) { Title = "Sample content page" },
            new ListItem(new MarkdownPage()) { Title = "Markdown page" },
            new ListItem(new CommandResultsPage()) { Title = "Command results page" },
            new IncrementingListItem(this) { Subtitle = $"Item 0" },
            new ListItem(updateCommand),
            new ListItem(showMessageCommand),
            new ListItem(command)
            {
                Title = "Open the Command Palette documentation",
            },
            new ListItem(testCommand) { Title = "Open Github" },
            new ListItem(new NoOpCommand()) { Title = "TODO: Implement your extension here" },
            new ListItem(new MySecondPage()) { Title = "My second page", Subtitle = "A second page of commands" },
        ];
    }

    public override IListItem[] GetItems()
    {
        return _items.ToArray();
    }

    internal void Increment()
    {
        this.IsLoading = true;
        Task.Run(() =>
        {
            Thread.Sleep(3000);
            _items.Add(new IncrementingListItem(this) { Subtitle = $"Item {_items.Count}" });
            RaiseItemsChanged();
            this.IsLoading = false;
        });
    }
}

internal sealed partial class ShowMessageCommand : InvokableCommand
{
    public override string Name => "Show message";
    public override IconInfo Icon => new("\uE8A7");

    public override ICommandResult Invoke()
    {
        _ = MessageBox(0, "I came from the Command Palette", "What's up?", 0x00001000);
        return CommandResult.KeepOpen();
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
}

internal sealed partial class IncrementingListItem : ListItem
{
    public IncrementingListItem(ToolDevHelpersCmdPalPage page) : base(new NoOpCommand())
    {
        _page = page;
        Command = new AnonymousCommand(action: _page.Increment) {  Result = CommandResult.KeepOpen() };
        Title = "Increment";
    }

    private ToolDevHelpersCmdPalPage _page;
}