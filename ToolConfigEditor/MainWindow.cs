using CommonLib.Models;
using System.Diagnostics;
using System.Text.Json;

namespace ToolConfigEditor
{
    public partial class MainWindow : Form
    {
        private string filename = "";
        private ToolConfigProject project = new();
        private BindingSource listBoxSource;
        private readonly System.Windows.Forms.Timer statusClearTimer;

        public MainWindow(string? projectFile)
        {
            InitializeComponent();

            statusClearTimer = new()
            {
                Interval = 5000 // Set the interval to 5000 milliseconds (5 seconds)
            };
            statusClearTimer.Tick += StatusClearTimer_Tick;

            if (!string.IsNullOrEmpty(projectFile))
                project = LoadProject(projectFile);

            listBoxSource = new()
            {
                DataSource = project.GetToolConfigs()
            };

            toolsListBox.DataSource = listBoxSource;
            toolsListBox.DisplayMember = "name";
            toolsListBox.ValueMember = "shortName";
            AddDataBindings();
        }

        /// <summary>
        /// OnValidating-handler for the KeywordTextBox.
        /// Checks that the value is unique in the list of tool-configs,
        /// otherwise resets the value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeywordTextBoxOnValidating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sender is TextBox control && toolsListBox.SelectedItem is ToolConfig selectedConfig)
            {
                var newValue = control.Text;
                var oldValue = selectedConfig.shortName;

                if (!project.HasToolConfigKey(newValue) || newValue == oldValue)
                    return;

                SetStatusText($"Tool with shortname \"{newValue}\" already exists!");
                e.Cancel = true;
                control.Text = oldValue;
            }
            //Debug.WriteLine(sender.ToString(), e.ToString());
        }

        /// <summary>
        /// Adds data bindings to the input-fields in the form.
        /// </summary>
        private void AddDataBindings()
        {
            keywordTextBox.DataBindings.Add("Text", listBoxSource, "shortName", false, DataSourceUpdateMode.OnValidation, "");
            nameTextBox.DataBindings.Add("Text", listBoxSource, "name", false, DataSourceUpdateMode.OnValidation, "");
            portTextBox.DataBindings.Add("Text", listBoxSource, "port", false, DataSourceUpdateMode.OnValidation, "");
            useHttpsCheckBox.DataBindings.Add("Checked", listBoxSource, "useHttps", false, DataSourceUpdateMode.OnValidation, false);
            remoteServerUrlTextBox.DataBindings.Add("Text", listBoxSource, "remoteServerUrl", false, DataSourceUpdateMode.OnValidation, "");
            exePathTextBox.DataBindings.Add("Text", listBoxSource, "exePath", false, DataSourceUpdateMode.OnValidation, "");
            editListBox1.DataBindings.Add("ListValues", listBoxSource, "additionalPages", false, DataSourceUpdateMode.OnValidation);
        }

        /// <summary>
        /// Removes the data bindings from the input-fields in the form.
        /// </summary>
        private void RemoveBindings()
        {
            keywordTextBox.DataBindings.Clear();
            nameTextBox.DataBindings.Clear();
            portTextBox.DataBindings.Clear();
            useHttpsCheckBox.DataBindings.Clear();
            remoteServerUrlTextBox.DataBindings.Clear();
            exePathTextBox.DataBindings.Clear();

            keywordTextBox.Text = "";
            nameTextBox.Text = "";
            portTextBox.Text = "";
            useHttpsCheckBox.Checked = false;
            remoteServerUrlTextBox.Text = "";
            exePathTextBox.Text = "";

            SetFormEnabled(false);
        }

        /// <summary>
        /// OnSelectedValueChanged handler for the ToolsListBox. Fills the form-fields
        /// with the values of the selected tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolsListBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            int selectedItemIdx = toolsListBox.SelectedIndex;
            bool isEnabled = selectedItemIdx >= 0;

            SetFormEnabled(isEnabled);
            copyToolMenuItem.Enabled = isEnabled;
            deleteToolMenuItem.Enabled = isEnabled;
        }

        /// <summary>
        /// Sets the enabled-state for the input-fields in the form.
        /// </summary>
        /// <param name="isEnabled">Flag if the input-fiels should be enabled or not.</param>
        private void SetFormEnabled(bool isEnabled)
        {
            keywordTextBox.Enabled = isEnabled;
            nameTextBox.Enabled = isEnabled;
            portTextBox.Enabled = isEnabled;
            remoteServerUrlTextBox.Enabled = isEnabled;
            useHttpsCheckBox.Enabled = isEnabled;
            exePathTextBox.Enabled = isEnabled;
        }

        /// <summary>
        /// OnClick-handler for the OpenMenuItem. Opens a file dialog and loads a project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenMenuItemOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Tool Configuration (*.json)|*.json";
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    project = LoadProject(openFileDialog.FileName);
                    listBoxSource.DataSource = project.GetToolConfigs();
                    SetStatusText($"Project {openFileDialog.FileName} loaded successfully");
                }
            }
            catch (Exception ex)
            {
                SetStatusText($"Error while loading project: {ex.Message}");
                Debug.WriteLine($"Error while loading project: {ex.Message}");
            }
        }

        /// <summary>
        /// Load a project from a file. If file cannot be found, creates a new project instead.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private ToolConfigProject LoadProject(string filename)
        {
            var loadedProject = ToolConfigProject.ReadToolConfigProject(filename);
            if (loadedProject == null)
            {
                loadedProject = new ToolConfigProject();
            }
            else
            {
                this.filename = filename;
            }

            return loadedProject;
        }

        /// <summary>
        /// OnClick-handler for the SaveAs-menu-item.
        /// Save the project at the existing location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveMenuItemOnClick(object sender, EventArgs e)
        {
            SaveProject(false);
        }

        /// <summary>
        /// OnClick-handler for the SaveAs-menu-item.
        /// Open file dialog and save the project at the selected location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsMenuItemOnClick(object sender, EventArgs e)
        {
            SaveProject(true);
        }

        /// <summary>
        /// Save the current project.
        /// </summary>
        /// <param name="saveAs">True if a Save-Dialog should be opened to select where to save to, otherwise false</param>
        private void SaveProject(bool saveAs)
        {
            try
            {
                string saveFilename = filename;

                if (saveFilename == null)
                    saveAs = true;

                if (saveAs)
                {
                    SaveFileDialog saveFileDialog = new();
                    saveFileDialog.Filter = "Tool Configuration (*.json)|*.json";
                    var dialogResult = saveFileDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                        saveFilename = saveFileDialog.FileName;
                }

                if (string.IsNullOrEmpty(saveFilename))
                {
                    SetStatusText("No file selected to save!");
                    return;
                }

                StreamWriter sw = new StreamWriter(saveFilename);
                sw.Write(JsonSerializer.Serialize(project));
                sw.Close();

                SetStatusText($"Saved project to {saveFilename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// OnClick-handler for the NewMenuItem. Creates a new project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewMenuItemOnClick(object sender, EventArgs e)
        {
            RemoveBindings();
            project = new ToolConfigProject();
            listBoxSource = new()
            {
                DataSource = project.GetToolConfigs()
            };

            toolsListBox.DataSource = listBoxSource;
            toolsListBox.DisplayMember = "name";
            AddDataBindings();
        }

        /// <summary>
        /// Sets the status bar text. Starts a timer to clear the text after a certain time.
        /// </summary>
        /// <param name="text"></param>
        private void SetStatusText(string text)
        {
            statusBarLabel.Text = text;
            statusClearTimer.Stop();
            statusClearTimer.Start();
        }

        /// <summary>
        /// Clears the status bar after 5 seconds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusClearTimer_Tick(object? sender, EventArgs e)
        {
            statusBarLabel.Text = "";
            statusClearTimer.Stop();
        }

        /// <summary>
        /// Add a new empty tool config to the project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToolMenuItemOnClick(object sender, EventArgs e)
        {
            AddToolConfig(new ToolConfig()
            {
                shortName = "",
                name = "",
                exePath = ""
            }, true);

            listBoxSource.MoveLast();
            SetStatusText("Added new tool");
        }

        /// <summary>
        /// Copy the selected tool config.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyToolMenuItemOnClick(object sender, EventArgs e)
        {
            if (listBoxSource.Current is ToolConfig selectedElem)
            {
                ToolConfig newConfig = new(selectedElem);
                newConfig.shortName += "_copy";
                newConfig.name += " (Copy)";
                if (AddToolConfig(newConfig, false))
                    SetStatusText($"Copied tool {selectedElem.name}");
                else
                    SetStatusText($"Tool \"{selectedElem.shortName}\" already exists!");
            }
        }

        /// <summary>
        /// Add a new tool config to the project and update the list-box.
        /// </summary>
        /// <param name="config">Tool config to add</param>
        /// <param name="generateName">Flag if a shortname and name should be automatically get generated for the
        /// new config.</param>
        /// <returns>True if the tool got added successfully, otherwise false.</returns>
        private bool AddToolConfig(ToolConfig config, bool generateName)
        {
            var isAdded = project.AddToolConfig(config, generateName);
            if (isAdded)
                listBoxSource.DataSource = project.GetToolConfigs();
            return isAdded;
        }

        /// <summary>
        /// OnClick-handler for the DeleteToolMenuItem.
        /// Removes the currently selected tool from the project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteToolMenuItemOnClick(object sender, EventArgs e)
        {
            ToolConfig? currentEntry = listBoxSource.Current as ToolConfig;
            if (currentEntry == null)
            {
                Debug.WriteLine("Remove not possible, nothing selected!");
                return;
            }

            SetStatusText($"Tool \"{currentEntry.name}\" removed");
            Debug.WriteLine("Remove entry: " + currentEntry.name);
            project.RemoveToolConfig(currentEntry.shortName);
            listBoxSource.DataSource = project.GetToolConfigs();
        }
    }
}
