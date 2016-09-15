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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example7
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

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnGotFocus(object sender, RoutedEventArgs e)
        {
            label.Content = "Got focus";
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            label.Content = e.Key.ToString();
            if (e.Key == Key.Left)
            {
                
            }
        }

        private void TextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var ci = textBox.CaretIndex;
            textBox.Text = string.Join("", textBox.Text.Where(char.IsDigit));
            textBox.CaretIndex = ci;
        }

        private void Address_KeyDown(object sender, KeyEventArgs e)
        {
            var s = ((TextBox) sender).Text;
            if (!s.StartsWith("http://"))
                s = "http://" + s;
            switch (e.Key)
            {
                case Key.Return:
                    Browser.Navigate(s);
                    break;
            }
        }
    }
}
