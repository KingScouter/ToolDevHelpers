using CommonLib.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class BranchListPage : ListPage
    {
        private List<IListItem> items;
        private readonly Lock _resultsLock = new();

        private ListItem fetchListItem;

        public BranchListPage()
        {
            items = [];
            Title = "Open";
            Name = "Fetch";

            fetchListItem = new(new AnonymousCommand(() =>
            {
                _ = GetBranches();
            })
            { Name = "Fetch branches", Icon = new IconInfo("\uE72C"), Result = CommandResult.KeepOpen() });

            EmptyContent = new CommandItem(new AnonymousCommand(() =>
            {
                _ = GetBranches();
            })
            { Result = CommandResult.KeepOpen() })
            { Title = "No branches found or no source folder configured" };
        }

        /// <summary>
        /// Get the items to display. On initial call, fetch the branches from the local source folder.
        /// </summary>
        /// <returns>List of branches</returns>
        public override IListItem[] GetItems()
        {
            if (string.IsNullOrEmpty(ExtensionSettings.Instance.SourceFolder))
            {
                EmptyContent = new CommandItem(new NoOpCommand())
                { Title = "No source folder configured" };
                UpdateItems([]);
                return [];
            }


            IListItem[] localItems = [];

            lock (_resultsLock)
            {
                if (items.Count == 0)
                    _ = GetBranches();

                return items.ToArray();
            }
        }

        /// <summary>
        /// Update the fetched items thread-safe and raise the event for changed items.
        /// </summary>
        /// <param name="newItems">List of new items to update</param>
        private void UpdateItems(List<IListItem> newItems)
        {
            ExtensionHost.LogMessage($"GORT: UpdateItems 1: {newItems.Count}");
            lock (_resultsLock)
            {
                items = newItems;
            }

            RaiseItemsChanged(items.Count);
        }

        /// <summary>
        /// Fetch the branches from the configured source folder.
        /// </summary>
        /// <returns>Async task</returns>
        private async Task GetBranches()
        {
            var localUrl = ExtensionSettings.Instance.SourceFolder;
            if (string.IsNullOrEmpty(localUrl))
                return;

            IsLoading = true;
            var branches = await BranchManager.GetLocalBranches(localUrl);
            var branchItems = BranchesToList(branches);

            if (branchItems.Count == 0)
            {
                EmptyContent = new CommandItem(new AnonymousCommand(() =>
                {
                    _ = GetBranches();
                })
                { Result = CommandResult.KeepOpen() })
                { Title = "No branches found!" };
                UpdateItems([]);
            } else
            {
                UpdateItems([.. branchItems, fetchListItem]);
            }

            IsLoading = false;
        }

        /// <summary>
        /// Convert a list of branches into a list of BranchListItems
        /// </summary>
        /// <param name="branches">List of branches to convert</param>
        /// <returns>Converted list of BranchListItems</returns>
        private static List<IListItem> BranchesToList(IEnumerable<string> branches)
        {
            List<IListItem> branchItems = [];

            foreach (var branch in branches)
            {
                branchItems.Add(new BranchListItem(branch));
            }

            return branchItems;
        }
    }
}
