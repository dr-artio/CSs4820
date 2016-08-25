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

namespace Example3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rnd;
   
        public MainWindow()
        {
            InitializeComponent();
            rnd = new Random();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void generate_Click(object sender, RoutedEventArgs e)
        {
            if (TypeComboBox.SelectionBoxItem != null)
            {
                string val = MaxValueTextBox.Text;
                int max;
                if (int.TryParse(val, out max) && max > 0)
                {
                    if (TypeComboBox.Text == "Int")
                        TextBoxRandom.Text = rnd.Next(max).ToString();
                    else if (TypeComboBox.Text == "Double")
                        TextBoxRandom.Text = (rnd.NextDouble() * max).ToString();
                    else if (TypeComboBox.Text == "String")
                    {
                        TextBoxRandom.Text = GenerateRandomString(max);
                    }
                }
            }
        }

        private void TypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            generate.IsEnabled = true;
            if (TypeComboBox.SelectedItem == stringType)
            {
                label.Content = "The Length:";
            }
            else
            {
                label.Content = "Max Value:";
            }
        }

        private string GenerateRandomString(int length)
        {
            if (length <= 0) return "";
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; ++i)
            {
                char a = (char)rnd.Next('A', 'z');
                sb.Append(a);
            }
            return sb.ToString();
        }
    }
}
