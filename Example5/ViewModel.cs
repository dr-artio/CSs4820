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
        // Number of buttons in the row
        private const int SIZE = 4;
        // Number of random clicks for shuffling
        private const int SHUFFLE_DEPTH = 2000;
        // Collection of states bound to buttons
        private readonly Dictionary<int, State> _buttons = new Dictionary<int, State>();
        // list of ids (indecies) of buttons
        private readonly List<int> _keys = new List<int>();
        // index of empy button
        private int _empty;
        // default index of empty button. The last element.
        private int l = SIZE*SIZE - 1;
        // flag for shuffling to prevent any events from raising while game is starting
        private bool _isShuffling = false;
        // Time measurment object to report time when game is finished
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Constructor initializes list of ids and reset states
        /// </summary>
        public ViewModel()
        {
            _stopwatch = new Stopwatch();
            for (int i = 0; i < l+1; ++i)
            {   
                _keys.Add(i);
            }
            Reset();
        }

        /// <summary>
        /// Property to be bound with list of buttnt in grid
        /// </summary>
        public Dictionary<int, State> Buttons
        {
            get { return _buttons; }
        }

        /// <summary>
        /// Property to access list of ids
        /// </summary>
        public IEnumerable<int> ButtonIds
        {
            get { return _keys; }            
        }

        /// <summary>
        /// Property to check if current ordering is proper. False if flag IsShuffling
        /// is true.
        /// </summary>
        public bool IsOrdered
        {
            get { return !_isShuffling && _buttons.All(x => x.Key == x.Value.Key); }
        }

        /// <summary>
        /// Property to display elapsed time when game is finished
        /// </summary>
        public string Elapsed
        {
            get { return _stopwatch.Elapsed.ToString(@"mm\:ss"); }
        }

        /// <summary>
        /// Property is true when pane is shuffled and game is active
        /// </summary>
        public bool IsStarted
        {
            get { return _stopwatch.ElapsedMilliseconds > 0; }
        }

        /// <summary>
        /// Property negation of isStarted for binding to reset menuitem
        /// </summary>
        public bool IsResetEnabled
        {
            get { return !IsOrdered; }
        }

        /// <summary>
        /// Auto-property flag to indicate that animation is active.
        /// Used to prevend bugus behavior when several animations 
        /// started simulteniously.
        /// </summary>
        public bool IsAnimationInProgress { get; set; }

        /// <summary>
        /// Method to try swapping states with empty button 
        /// </summary>
        /// <param name="index">Index of clicked button</param>
        public void Click(int index)
        {
            if (index == _empty || (IsOrdered && !_isShuffling)) return;
            int xi, yi;
            GetCoordinates(index, out xi, out yi);
            int xj, yj;
            GetCoordinates(_empty, out xj, out yj);

            // check if clicked button is neigbor of empty button
            if (Math.Abs(xi - xj) + Math.Abs(yi - yj) == 1)
            {
                // Swap states and send update to window
                SwapStates(index);
                OnPropertyChanged("IsOrdered");
                OnPropertyChanged("IsResetEnabled");
                
                if (IsOrdered) _stopwatch.Stop();
            }
            // update diractions of animation for GameButton controls
            ResetDirs();
        }

        /// <summary>
        /// Generator for indecies of buttons to be moved
        /// when button is clicked. If clicked button is not 
        /// adjuscent to empty button but in same row or column 
        /// we can move row or column all button between 
        /// empty button and clicked one
        /// </summary>
        /// <param name="index">index of clicked button</param>
        /// <returns>Iterator over buttons to move</returns>
        public IEnumerable<int> IdsToShift(int index)
        {
            // empty iterator in case if we clicked on empty button or order is correct 
            // and we are not shuffling. Iterator will be empty if we click on button not in
            // the same row and column with empty button
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

        /// <summary>
        /// Swap stated in the dictionary
        /// </summary>
        /// <param name="index"></param>
        private void SwapStates(int index)
        {
            var state = _buttons[_empty];
            _buttons[_empty] = _buttons[index];
            _buttons[index] = state;
            _empty = index;
        }

        /// <summary>
        /// Reset states and send updates to window 
        /// </summary>
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

        /// <summary>
        /// Reset direction of animations for all buttons
        /// They depend on position of empty button. So 
        /// all directions are toward empty button
        /// </summary>
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

        /// <summary>
        /// Randomly shuffle buttons 
        /// 
        /// </summary>
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

        
        /// <summary>
        /// INotifyPropertyChanged implementation
        /// </summary>
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

    /// <summary>
    /// State object for buttons
    /// </summary>
    class State
    {   
        // Content to display on the button
        public string Content { get; set; }
        // Invisible if button is empty
        public Visibility Visibility { get { return string.IsNullOrEmpty(Content) ? Visibility.Hidden : Visibility.Visible; } }
        // Id of corresponding state
        public int Key { get; set; }
        // horizontal direction of animation (-1 left 1 right 0 no move)
        public int Xdir { get; set; }
        // vertival direction of animation (-1 up 1 down 0 no move) 
        public int Ydir { get; set; }
    }
}
