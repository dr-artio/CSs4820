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
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModel()
        {
            _generators.Add(new Generator(GenerateRandomString, "The length:", "String"));
            _generators.Add(new Generator(GenerateRandomFloat, "The max value:", "Double"));
            _generators.Add(new Generator(GenerateRandomInt, "The max value:", "Integer"));
        }

        // Private fields
        private readonly List<Generator> _generators = new List<Generator>(3);
        private int _selectedIndex;
        private int _length;
        private readonly Random _rnd = new Random();
        
        
        // Public interface
        public string Value { get; set; }

        public int Length
        {
            get { return _length; }
            set
            {
                _length = value;
                // Fire property changed event manually
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
                // Fire prameter changed to update label 
                // and new GeneratedValue
                OnPropertyChanged("Parameter");
                OnPropertyChanged("GeneratedValue");
            }
        }

        public void Update()
        {
            OnPropertyChanged("GeneratedValue");
        }

        /// <summary>
        /// List of possible types we can generate random value
        /// </summary>
        public List<string> Types { get { return _generators.Select(x=> x.Type).ToList(); } }

        public string Parameter
        {
            get {
                return SelectedIndex >= 0 ? _generators[SelectedIndex].Parameter : string.Empty;
            }
        }


        /// <summary>
        /// Read-only property holding random value
        /// </summary>
        public string GeneratedValue
        {
            get
            {
                return SelectedIndex >= 0 && Length > 0 ? _generators[SelectedIndex].Generate(Length) : string.Empty;
            }
        }

        // Methods to generate random values of different types
        private string GenerateRandomString(int length)
        {
            if (length <= 0) return "";
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                char a = (char)_rnd.Next('A', 'Z');
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


        /// <summary>
        /// Inner class to hold necessary properties
        /// </summary>
        private class Generator
        {
            public Func<int, string> Generate { get; private set; }
            public string Parameter { get; private set; }
            public string Type { get; private set; }

            public Generator(Func<int, string> generate, string parameter, string type)
            {
                Generate = generate;
                Parameter = parameter;
                Type = type;
            }
        }

        /// <summary>
        /// Implementation of interface INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
