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
                _values.Clear();
                if (value != null)
                    _values.AddRange(value);

                UpdateListSource();
            }
        }

        BindingSource listBoxSource = [];

        /// <summary>
        /// Constructor for EditListBox. Initializes the BindingSource with the value list.
        /// </summary>
        public EditListBox()
        {
            InitializeComponent();
            listBoxSource.DataSource = _values;
            entryListBox.DataSource = listBoxSource;
        }

        /// <summary>
        /// Invokes a PropertyChanged event
        /// </summary>
        /// <param name="PropertyName"></param>
        private void NotifyPropertyChanged([CallerMemberName] string? PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// OnClick-handler for the AddEntryButton.
        /// Adds a new entry to the list of values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEntryButtonOnClick(object sender, EventArgs e)
        {
            Debug.WriteLine("Add new entry: " + newEntryTextBox.Text);
            if (string.IsNullOrWhiteSpace(newEntryTextBox.Text) || _values.Contains(newEntryTextBox.Text))
                return;

            _values.Add(newEntryTextBox.Text);
            UpdateListSource();
        }

        /// <summary>
        /// OnClick-handler for the RemoveEntryButton.
        /// Removes the currently selected entry from the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Update the BindingSource data.
        /// </summary>
        private void UpdateListSource()
        {
            listBoxSource.ResetBindings(true);
            NotifyPropertyChanged(nameof(ListValues));
            entryListBox.ClearSelected();
        }

        /// <summary>
        /// Handler if the selection changes in the EntryListBox.
        /// Fills the text-box with the currently selected entry.
        /// Clears the text-box if nothing is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntryListBoxOnSelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"Selected index changed: ${e}");
            int selectedItemIdx = entryListBox.SelectedIndex;
            if (selectedItemIdx >= 0)
                newEntryTextBox.Text = entryListBox.SelectedItem!.ToString();
            else
                newEntryTextBox.Text = "";
        }
    }
}
