using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Example11.Annotations;

namespace Example11.ViewModel
{
    class ProcessViewModel : INotifyPropertyChanged
    {
        private int _progress = 0;
        private Random _rnd = new Random();
        private Brush color;
        private byte _green, _red, _blue;

        public ProcessViewModel()
        {
            Task.Run(() =>
            {
                while (_progress < 100)
                {
                    Thread.Sleep(_rnd.Next(2000));
                    _progress += _rnd.Next(10);
                    OnPropertyChanged("Progress");
                }
            });
            _green = 224;
        }

        public void Update(int progress)
        {
            _progress = progress;
            OnPropertyChanged("Progress");
        }
        
        public int Progress
        {
            get { return _progress; }
        }

        public Brush Color
        {
            get
            {
                return new SolidColorBrush {Color = System.Windows.Media.Color.FromRgb(Red, Green, Blue)};
            }
        }

        public byte Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged();
                OnPropertyChanged("Color");
            }
        }

        public byte Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged();
                OnPropertyChanged("Color");
            }
        }

        public byte Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged();
                OnPropertyChanged("Color");
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
