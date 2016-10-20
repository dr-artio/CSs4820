using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Example5.Annotations;

namespace Example5
{
    class ViewModel : INotifyPropertyChanged
    {
        private const int SIZE = 4;
        private const int SHUFFLE_DEPTH = 1000;
        private readonly Dictionary<int, State> _buttons = new Dictionary<int, State>();
        private readonly List<int> _keys = new List<int>();
        private int _empty;
        private int l = SIZE*SIZE - 1;

        public ViewModel()
        {
            for (int i = 0; i < l+1; ++i)
            {   
                _keys.Add(i);
            }
            Reset();
        }

        public Dictionary<int, State> Buttons
        {
            get { return _buttons; }
        }

        public IEnumerable<int> ButtonIds
        {
            get { return _keys; }            
        }

        public void Click(int index)
        {
            int xi, yi;
            GetCoordinates(index, out xi, out yi);
            int xj, yj;
            GetCoordinates(_empty, out xj, out yj);
            if (Math.Abs(xi - xj) + Math.Abs(yi - yj) == 1)
            {
                var state = _buttons[_empty];
                _buttons[_empty] = _buttons[index];
                _buttons[index] = state;
                _empty = index;
                OnPropertyChanged("Buttons");
            }
        }

        public void Reset()
        {
            foreach (var id in ButtonIds)
            {
                _buttons[id] = new State {Content = (id+1).ToString()};
            }
            _empty = l;
            _buttons[l].Content = string.Empty;
            OnPropertyChanged("Buttons");
        }

        public void Shuffle()
        {
            var rnd = new Random();
            for (int i = 0; i < SHUFFLE_DEPTH; ++i)
            {
                int j = rnd.Next(_buttons.Count);
                Click(j);
            }
            OnPropertyChanged("Buttons");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GetCoordinates(int index, out int x, out int y)
        {
            x = index % SIZE;
            y = index / SIZE;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class State
    {
        public string Content { get; set; }
        public bool IsEnabled { get { return !string.IsNullOrEmpty(Content); } }
        public int Key { get; set; }
    }
}
