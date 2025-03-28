namespace ToolConfigEditor
{
    public partial class EditListBox : UserControl
    {
        BindingSource listBoxSource = new();

        public EditListBox()
        {
            InitializeComponent();
        }

        public void SetDataSource(object source, string dataMember)
        {
            listBoxSource.DataSource = source;
            listBoxSource.DataMember = dataMember;
            entryListBox.DataSource = listBoxSource;
        }
    }
}
