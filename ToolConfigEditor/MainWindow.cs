using CommonLib.Models;
using System.Diagnostics;

namespace ToolConfigEditor
{
    public partial class MainWindow : Form
    {
        private ToolConfigProject project = new();
        private readonly BindingSource listBoxSource;

        public MainWindow()
        {
            InitializeComponent();

            listBoxSource = new()
            {
                DataSource = project.GetToolConfigs()
            };

            toolsListBox.DataSource = listBoxSource;
            toolsListBox.DisplayMember = "Name";
            keywordTextBox.DataBindings.Add("Text", listBoxSource, "ShortName", false, DataSourceUpdateMode.OnPropertyChanged, "");
            nameTextBox.DataBindings.Add("Text", listBoxSource, "Name", false, DataSourceUpdateMode.OnPropertyChanged, "");
            portTextBox.DataBindings.Add("Text", listBoxSource, "Port", false, DataSourceUpdateMode.OnPropertyChanged, "");
            remoteServerUrlTextBox.DataBindings.Add("Text", listBoxSource, "RemoteServerUrl", false, DataSourceUpdateMode.OnPropertyChanged, "");
            exePathTextBox.DataBindings.Add("Text", listBoxSource, "ExePath", false, DataSourceUpdateMode.OnPropertyChanged, "");
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

            keywordTextBox.Enabled = isEnabled;
            nameTextBox.Enabled = isEnabled;
            portTextBox.Enabled = isEnabled;
            remoteServerUrlTextBox.Enabled = isEnabled;
            exePathTextBox.Enabled = isEnabled;
        }

        private void OpenMenuItemOnClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Tool Configuration (*.json)|*.json";
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var loadedProject = ToolConfigProject.ReadToolConfigProject(openFileDialog.FileName);
                    if (loadedProject != null)
                    {
                        project = loadedProject;
                        listBoxSource.DataSource = project.GetToolConfigs();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while loading project: ", ex);
            }
        }
    }
}
