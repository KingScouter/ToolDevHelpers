using CommonLib.Models;
using System.Diagnostics;

namespace ToolConfigEditor
{
    public partial class MainWindow : Form
    {
        private ToolConfigProject project;

        public MainWindow()
        {
            InitializeComponent();

            project = new ToolConfigProject();
            project.AddToolConfig(new()
            {
                name = "Tool 1",
                shortName = "t1",
                exePath = "path/to/exe",
                useHttps = true,
                port = 9090
            });

            project.AddToolConfig(new()
            {
                name = "What a tool",
                shortName = "wat",
                exePath = "path/to/exe2",
                useHttps = true,
                port = 8080
            });

            project.AddToolConfig(new()
            {
                name = "Another tool",
                shortName = "at",
                exePath = "path/to/exe3",
                useHttps = false,
                port = 7070
            });

            foreach (var elem in project.GetToolConfigs(""))
            {
                toolsListBox.Items.Add(elem.name);
            }
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

            if (isEnabled)
            {
                ToolConfig selectedTool = project.GetToolConfigs("").ElementAt(selectedItemIdx);
                keywordTextBox.Text = selectedTool.shortName;
                nameTextBox.Text = selectedTool.name;
                portTextBox.Text = selectedTool.port.ToString();
                remoteServerUrlTextBox.Text = selectedTool.remoteServerUrl;
                exePathTextBox.Text = selectedTool.exePath;
            }
            else
            {
                keywordTextBox.Text = "";
                nameTextBox.Text = "";
                portTextBox.Text = "";
                remoteServerUrlTextBox.Text = "";
                exePathTextBox.Text = "";
            }



        }
    }
}
