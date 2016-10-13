using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Example11.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue >= 100)
            {
                ProgressBar.Visibility = Visibility.Hidden;
                MyLabel.Content = "Completed!";
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            int _progress = 0;
            var _rnd = new Random();
            Task.Run(() =>
            {
                while (_progress < 100)
                {
                    Thread.Sleep(_rnd.Next(2000));
                    _progress += _rnd.Next(30);
                    
//                    ProcessViewModel.Update(_progress);
                }
            });

        }
    }
}
