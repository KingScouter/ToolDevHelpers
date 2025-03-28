using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ToolConfigEditor
{
    public partial class EditListBox : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string[] _values = [];

        [ListBindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string[] ListValues
        {
            get {
                return _values; 
            } 
            set {
                _values = value;

                listBoxSource.DataSource = _values;
                entryListBox.DataSource = listBoxSource;
                NotifyPropertyChanged();
            }
        }

        BindingSource listBoxSource = new();


        public EditListBox()
        {
            InitializeComponent();
        }

        private void NotifyPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
