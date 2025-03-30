using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ToolConfigEditor
{
    public partial class EditListBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private List<string> _values = [];

        [ListBindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string[] ListValues
        {
            get
            {
                return [.. _values];
            }
            set
            {
                if (value == null)
                    _values = [];
                else
                    _values = [.. value];

                listBoxSource.DataSource = _values;
                UpdateListSource();
            }
        }

        BindingSource listBoxSource = [];


        public EditListBox()
        {
            InitializeComponent();
            entryListBox.DataSource = listBoxSource;
        }

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private void AddEntryButtonOnClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Add new entry: " + newEntryTextBox.Text);

        }

        private void RemoveEntryButtonOnClick(object sender, EventArgs e)
        {
            string? currentEntry = listBoxSource.Current?.ToString();
            if (string.IsNullOrEmpty(currentEntry))
            {
                Debug.WriteLine("Remove not possible, nothing selected!");
                return;
            }

            Debug.WriteLine("Remove entry: " + currentEntry);
            _values.RemoveAt(listBoxSource.Position);
            UpdateListSource();

        }

        private void UpdateListSource()
        {
            listBoxSource.ResetBindings(true);
            NotifyPropertyChanged(nameof(ListValues));
        }
    }
}
