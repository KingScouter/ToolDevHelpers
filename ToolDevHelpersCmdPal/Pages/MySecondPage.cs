﻿using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevHelpersCmdPal.Pages
{
    internal class MySecondPage : ListPage
    {
        public MySecondPage()
        {
            Icon = new("\uF147");
            Title = "My second page";
            Name = "Open";
        }

        public override IListItem[] GetItems()
        {
            return Enumerable
                .Range(0, 100)
                .Select(i => new ListItem(new CopyTextCommand($"{i}"))
                {
                    Title = $"Copy text {i}"
                }).ToArray();
        }
    }
}
