using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Example4
{
    /// <summary>
    /// Interaction logic for AnotherWindow.xaml
    /// </summary>
    public partial class AnotherWindow : Window
    {
        public bool IsCancel = false;
        public AnotherWindow()
        {
            InitializeComponent();
        }

        public AnotherWindow(bool b)
        {
            InitializeComponent();
        }

        private void Button_no_OnClick(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            Close();
        }

        private void Button_yes_OnClick(object sender, RoutedEventArgs e)
        {
            IsCancel = false;
            Close();
        }
    }
}
