using CommonLib.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ToolDevHelpersCmdPal.Pages
{
    internal sealed partial class ToolListPage : ListPage
    {
        private List<IListItem> items = [];
        private ToolConfigProject? project;
        private readonly Lock _resultsLock = new();


        public override IListItem[] GetItems()
        {
            IsLoading = true;
            if (project == null)
            {
                string? projectFile = ExtensionSettings.Instance.ToolConfigFile;
                // No project-file set
                if (string.IsNullOrEmpty(projectFile))
                {
                    EmptyContent = new CommandItem(new NoOpCommand()) { Title = "No tool project configured in settings!" };
                    return UpdateItems([]);
                }

                if (!File.Exists(projectFile))
                {
                    EmptyContent = new CommandItem(new AnonymousCommand(() =>
                    {
                        // Create template project
                        ToolConfigProject.SaveTemplateProject(projectFile);
                        // Reload project
                        UpdateItems(GetItems().ToList(), true);
                    }) { Result = CommandResult.KeepOpen() }) 
                    { 
                        Title = $"Configured project {projectFile} does not exist. Press ENTER to create a template project.",
                        MoreCommands = [
                            new CommandContextItem(new AnonymousCommand(() => {
                                UpdateItems(GetItems().ToList(), true);
                            }) { Result = CommandResult.KeepOpen() }) { Title = "Reload project" }
                        ]
                    };
                    return UpdateItems([]);
                }

                EmptyContent = new CommandItem(new AnonymousCommand(() => {
                    GetItems();
                }) { Result = CommandResult.KeepOpen() })
                { Title = "Reload project" };
                try
                {
                    project = ToolConfigProject.ReadToolConfigProject(projectFile);
                }
                catch (Exception ex)
                {
                    ExtensionHost.LogMessage($"Exception while reading tool project: {ex.ToString()}");
                    return UpdateItems([]);
                }
                if (project == null)
                {
                    return UpdateItems([]);
                }
            }

            var toolItems = ToolsToList(project.GetToolConfigs());
            return UpdateItems(toolItems);
        }

        /// <summary>
        /// Convert a list of branches into a list of BranchListItems
        /// </summary>
        /// <param name="branches">List of branches to convert</param>
        /// <returns>Converted list of BranchListItems</returns>
        private static List<IListItem> ToolsToList(IEnumerable<ToolConfig> tools)
        {
            List<IListItem> toolItems = [];

            foreach (var tool in tools)
            {
                toolItems.Add(new ToolListItem(tool));
            }

            return toolItems;
        }

        /// <summary>
        /// Update the fetched items thread-safe and raise the event for changed items.
        /// </summary>
        /// <param name="newItems">List of new items to update</param>
        private IListItem[] UpdateItems(List<IListItem> newItems, bool raiseEvent = false)
        {
            IsLoading = false;
            lock (_resultsLock)
            {
                items = newItems;
            }

            if (raiseEvent)
            {
                RaiseItemsChanged(items.Count);
            }

            return [..items];
        }
    }
}
