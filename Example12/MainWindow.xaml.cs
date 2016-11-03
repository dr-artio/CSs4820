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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Example12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Action EmptyDelegate = delegate () { };
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Return:
                    //Canvas.SetLeft(running_text, 300);
                    //running_text.Content = text.Text;
                    //while (Canvas.GetLeft(running_text) > -running_text.ActualWidth)
                    //{
                    //    var l = Canvas.GetLeft(running_text);
                    //    Thread.Sleep((int)Math.Round(1 * speed.Value));
                    //    Canvas.SetLeft(running_text, l-1);
                    //    Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                    //}
                    var sb = FindResource("anim") as Storyboard;
                    var an = sb.Children.First() as DoubleAnimation;
                    an.To = -running_text.ActualWidth;
                    an.Duration = new Duration(new TimeSpan(0, 0, (int)speed.Value));
                    sb.Begin(this);

                    break;
            }
        }
    }
}
