using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Example11.Annotations;

namespace Example11.ViewModel
{
    class ProcessViewModel : INotifyPropertyChanged
    {
        private int _progress = 0;
        private Random _rnd = new Random();

        public ProcessViewModel()
        {
            Task.Run(() =>
            {
                while (_progress < 100)
                {
                    Thread.Sleep(_rnd.Next(2000));
                    _progress += _rnd.Next(30);
                    OnPropertyChanged("Progress");
                } 
            });
        }
        
        public int Progress
        {
            get { return _progress; }
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
