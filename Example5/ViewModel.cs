using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example5
{
    class ViewModel
    {
        public const int SIZE = 4;
        private readonly Dictionary<int, State> _buttons = new Dictionary<int, State>();
        
        private int l = SIZE*SIZE - 1;

        public ViewModel()
        {
            for (int i = 0; i < l; ++i)
            {
                _buttons.Add(i, new State{ Content = (i+1).ToString()});
            }
            _buttons.Add(l, new State());
        }

        public Dictionary<int, State> Buttons
        {
            get { return _buttons; }
        }

        public IEnumerable<int> ButtonIds
        {
            get { return _buttons.Keys; }            
        }
    }

    class State
    {
        public string Content { get; set; }
        public bool IsEnabled { get { return !string.IsNullOrEmpty(Content); } }
    }
}
