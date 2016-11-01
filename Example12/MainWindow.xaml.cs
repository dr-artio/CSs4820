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
                    var m = running_text.Margin;
                    m.Left  = 300.0;
                    running_text.Content = text.Text;
                    while (m.Left > -running_text.Width)
                    {
                        Thread.Sleep((int)Math.Round(1 * speed.Value));
                        m.Left -= 5;
                        running_text.Margin = m;
                        Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
                    }

                    break;
            }
        }
    }
}
