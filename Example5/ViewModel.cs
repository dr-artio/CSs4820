using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Example51.Annotations;

namespace Example5
{
    class ViewModel : INotifyPropertyChanged
    {
        private const int SIZE = 4;
        private const int SHUFFLE_DEPTH = 200;
        private readonly Dictionary<int, State> _buttons = new Dictionary<int, State>();
        private readonly List<int> _keys = new List<int>();
        private int _empty;
        private int l = SIZE*SIZE - 1;
        private bool _isShuffling = false;
        private readonly Stopwatch _stopwatch;

        public ViewModel()
        {
            _stopwatch = new Stopwatch();
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

        public bool IsOrdered
        {
            get { return !_isShuffling && _buttons.All(x => x.Key == x.Value.Key); }
        }

        public string Elapsed
        {
            get { return _stopwatch.Elapsed.ToString(@"mm\:ss"); }
        }

        public bool IsStarted
        {
            get { return _stopwatch.ElapsedMilliseconds > 0; }
        }

        public bool IsResetEnabled
        {
            get { return !IsOrdered; }
        }

        public bool IsAnimationInProgress { get; set; }

        public void Click(int index)
        {
            if (index == _empty || (IsOrdered && !_isShuffling)) return;
            int xi, yi;
            GetCoordinates(index, out xi, out yi);
            int xj, yj;
            GetCoordinates(_empty, out xj, out yj);

            

            if (Math.Abs(xi - xj) + Math.Abs(yi - yj) == 1)
            {
                SwapStates(index);
                OnPropertyChanged("IsOrdered");
                OnPropertyChanged("IsResetEnabled");
                
                if (IsOrdered) _stopwatch.Stop();
            }
            ResetDirs();
        }

        public IEnumerable<int> IdsToShift(int index)
        {
            if (index == _empty || (IsOrdered && !_isShuffling)) yield break;
            int xi, yi;
            GetCoordinates(index, out xi, out yi);
            int xj, yj;
            GetCoordinates(_empty, out xj, out yj);

            var cmpy = yi.CompareTo(yj);
            var cmpx = xi.CompareTo(xj);
            if (cmpy == 0 || cmpx == 0)
            {
                for (int x = xj + cmpx, y = yj + cmpy; x != xi+cmpx || y != yi+cmpy; x += cmpx, y += cmpy)
                {
                    yield return FromCoordinates(x, y);
                }
            }
        }

        private void SwapStates(int index)
        {
            var state = _buttons[_empty];
            _buttons[_empty] = _buttons[index];
            _buttons[index] = state;
            _empty = index;
        }

        public void Reset()
        {
            _stopwatch.Reset();
            foreach (var id in ButtonIds)
            {;
                _buttons[id] = new State {Content = (id+1).ToString(), Key = id};
            }
            _empty = l;
            _buttons[l].Content = string.Empty;
            ResetDirs();
            OnPropertyChanged("Buttons");
            OnPropertyChanged("IsResetEnabled");
            OnPropertyChanged("IsOrdered");
        }

        public void ResetDirs()
        {
            int ex, ey;
            GetCoordinates(_empty, out ex, out ey);
            foreach (var i in ButtonIds)
            {
                int x, y;
                GetCoordinates(i, out x, out y);
                if (!IsOrdered)
                {
                    _buttons[i].Xdir = ex.CompareTo(x);
                    _buttons[i].Ydir = ey.CompareTo(y); 
                }
                else
                {
                    _buttons[i].Xdir = 0;
                    _buttons[i].Ydir = 0; 
                }
            }
            IsAnimationInProgress = false;
            OnPropertyChanged("Buttons");
        }

        public void Shuffle()
        {
            _isShuffling = true;
            var rnd = new Random();
            for (int i = 0; i < SHUFFLE_DEPTH; ++i)
            {
                int j = rnd.Next(_buttons.Count);
                Click(j);
            }
            OnPropertyChanged("Buttons");
            _isShuffling = false;
            _stopwatch.Restart();
        }

        public void Refresh()
        {
            OnPropertyChanged("Buttons");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GetCoordinates(int index, out int x, out int y)
        {
            x = index % SIZE;
            y = index / SIZE;
        }

        private int FromCoordinates(int x, int y)
        {
            return y*SIZE + x;
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
        public Visibility Visibility { get { return string.IsNullOrEmpty(Content) ? Visibility.Hidden : Visibility.Visible; } }
        public int Key { get; set; }
        public int Xdir { get; set; }
        public int Ydir { get; set; }
    }
}
