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

        private void ToolsListBoxOnSelectedValueChanged(object sender, EventArgs e)
        {
            int selectedItemIdx = toolsListBox.SelectedIndex;
            if (selectedItemIdx < 0)
                return;

            ToolConfig selectedTool = project.GetToolConfigs("").ElementAt(selectedItemIdx);
            keywordTextBox.Text = selectedTool.shortName;
            nameTextBox.Text = selectedTool.name;
            portTextBox.Text = selectedTool.port.ToString();
            remoteServerUrlTextBox.Text = selectedTool.remoteServerUrl;
            exePathTextBox.Text = selectedTool.exePath;
        }
    }
}
