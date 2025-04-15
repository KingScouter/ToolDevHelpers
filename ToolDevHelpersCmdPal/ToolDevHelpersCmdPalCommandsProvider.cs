// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace ToolDevHelpersCmdPal;

public partial class ToolDevHelpersCmdPalCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public ToolDevHelpersCmdPalCommandsProvider()
    {
        DisplayName = "ToolDevHelpers";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        Settings = ExtensionSettings.Instance.Settings;

        _commands = [
            new CommandItem(new ToolDevHelpersCmdPalPage()) { Title = DisplayName },
            new CommandItem(new ShowMessageCommand()) { Title = "Send a message" },
        ];

    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
