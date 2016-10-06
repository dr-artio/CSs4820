using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Example10.Annotations;
using Example10.View;

namespace Example10.ViewModel
{
    class CustomComboBoxVM : INotifyPropertyChanged
    {
        private List<string> _options = new List<string>();
        private string _other = "Other";
        private int _selectedIndex;

        public CustomComboBoxVM()
        {
            _options.Add("Blocked");
            _options.Add("Insufficient Permissions");
            CustomLabel = "Reason";
        }

        public string CustomLabel { get; set; }

        public IEnumerable<string> Options
        {
            get
            {
                foreach (var opt in _options)
                {
                    yield return opt;
                }
                yield return _other;
            }
        }

        public string Reason { get; set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                Reason = IsOtherSelected ? Reason : "";
                OnPropertyChanged("Reason");
                OnPropertyChanged("IsOtherSelected");
            }
        }

        public bool IsOtherSelected
        {
            get { return SelectedIndex > 0 ? SelectedIndex == _options.Count : false; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
