using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Example10.Annotations;

namespace Example10.ViewModel
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private readonly List<Generator> _generators = new List<Generator>(3);
        private int _selectedIndex;
        private int _length;
        private readonly Random _rnd = new Random();
        public string Value { get; set; }

        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                OnPropertyChanged("GeneratedValue");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("Parameter");
                OnPropertyChanged("GeneratedValue");
            }
        }

        public List<string> Types { get { return types; } }

        public string Parameter
        {
            get {
                return SelectedIndex >= 0 ? _generators[SelectedIndex].Parameter : string.Empty;
            }
        }

        public string GeneratedValue
        {
            get
            {
                return SelectedIndex >= 0 && Length > 0 ? _generators[SelectedIndex].Generate(Length) : string.Empty;
            }
        }

        public List<string> types = new List<string> {"String", "Double", "Integer"};
        


        public ViewModel()
        {
            _generators.Add(new Generator(GenerateRandomString, "The length:"));
            _generators.Add(new Generator(GenerateRandomFloat, "The max value:"));
            _generators.Add(new Generator(GenerateRandomInt, "The max value:"));
        }

        private string GenerateRandomString(int length)
        {
            if (length <= 0) return "";
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                char a = (char)_rnd.Next('A', 'z');
                sb.Append(a);
            }
            return sb.ToString();
        }

        private string GenerateRandomInt(int max)
        {
            return _rnd.Next(max).ToString();
        }

        private string GenerateRandomFloat(int max)
        {
            return (_rnd.NextDouble() * max).ToString(CultureInfo.InvariantCulture);
        }

        private class Generator
        {
            public Func<int, string> Generate { get; private set; }
            public string Parameter { get; private set; }

            public Generator(Func<int, string> generate, string parameter)
            {
                Generate = generate;
                Parameter = parameter;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
