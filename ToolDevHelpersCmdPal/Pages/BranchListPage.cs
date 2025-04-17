using CommonLib.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class BranchListPage : ListPage
    {
        private List<IListItem> items;

        private ListItem fetchListItem;

        public BranchListPage()
        {
            items = [];
            Title = "Now it starts";

            fetchListItem = new(new AnonymousCommand(() =>
            {
                _ = GetBranches();
            })
            { Name = "Fetch branches", Icon = new IconInfo("\uE72C") });

            _ = GetBranches();
        }

        public override IListItem[] GetItems()
        {
            return items.ToArray();
        }

        private async Task GetBranches()
        {
            var localUrl = ExtensionSettings.Instance.SourceFolder;
            if (string.IsNullOrEmpty(localUrl))
            {
                items = [];
                EmptyContent = new CommandItem(new AnonymousCommand(() =>
                {
                    _ = GetBranches();
                })
                {  Result = CommandResult.KeepOpen() })
                { Title = "No source folder configured" };
                RaiseItemsChanged(0);
                return;
            }

            IsLoading = true;
            var branches = await BranchManager.GetLocalBranches(localUrl);
            var branchItems = BranchesToList(branches);

            items = [.. branchItems, fetchListItem];
            RaiseItemsChanged(items.Count);
            IsLoading = false;
        }

        private static List<IListItem> BranchesToList(IEnumerable<string> branches)
        {
            List<IListItem> branchItems = [];

            foreach (var branch in branches)
            {
                var cmd = new NoOpCommand() { Name = branch };
                branchItems.Add(new ListItem(cmd));
            }

            return branchItems;
        }
    }
}
