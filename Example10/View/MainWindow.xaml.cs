using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Example10.ViewModel;

namespace Example10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += OnViewModelOnPropertyChanged;
            Types.ItemsSource = ViewModel.Types;
        }

        private void OnViewModelOnPropertyChanged(object o, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case "Parameter":
                    Type.Content = ViewModel.Parameter;
                    break;
                case "GeneratedValue":
                    GeneratedValue.Text = ViewModel.GeneratedValue;
                    break;
                case "SelectedIndex":
                    Types.SelectedIndex = ViewModel.SelectedIndex;
                    break;
                case "Length":
                    Length.Text = ViewModel.Length.ToString();
                    break;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Update();
        }

        private void Types_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedIndex = Types.SelectedIndex;
        }

        private void Length_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var l = Length.Text;
            var len = 0;
            if (GeneratedValue != null && int.TryParse(l, out len))
            {
                ViewModel.Length = len;
            }
        }
    }
}
