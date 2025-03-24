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
            AddDataBindings();
        }

        /// <summary>
        /// Adds data bindings to the input-fields in the form.
        /// </summary>
        private void AddDataBindings()
        {
            keywordTextBox.DataBindings.Add("Text", listBoxSource, "shortName", false, DataSourceUpdateMode.OnPropertyChanged, "");
            nameTextBox.DataBindings.Add("Text", listBoxSource, "name", false, DataSourceUpdateMode.OnPropertyChanged, "");
            portTextBox.DataBindings.Add("Text", listBoxSource, "port", false, DataSourceUpdateMode.OnPropertyChanged, "");
            useHttpsCheckBox.DataBindings.Add("Checked", listBoxSource, "useHttps", false, DataSourceUpdateMode.OnPropertyChanged, false);
            remoteServerUrlTextBox.DataBindings.Add("Text", listBoxSource, "remoteServerUrl", false, DataSourceUpdateMode.OnPropertyChanged, "");
            exePathTextBox.DataBindings.Add("Text", listBoxSource, "exePath", false, DataSourceUpdateMode.OnPropertyChanged, "");
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
                Debug.WriteLine("Error while loading project: ", ex);
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
    }
}
