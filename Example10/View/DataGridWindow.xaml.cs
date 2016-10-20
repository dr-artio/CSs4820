using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Example10.Annotations;

namespace Example10.View
{
    /// <summary>
    /// Interaction logic for DataGridWindow.xaml
    /// </summary>
    public partial class DataGridWindow : Window
    {
        public DataGridWindow()
        {
            InitializeComponent();
        }

        private void DataGridWindow_OnClosing(object sender, CancelEventArgs e)
        {
            DataGridViewModel.People.ResetBindings();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DataGridViewModel.SortByAge();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName != "Gender")
                e.Cancel = true;
        }
    }

}
