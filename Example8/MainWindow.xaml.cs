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

namespace Example8
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

        private void Circle_mouse_down(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Circle hit");
        }

        private void Image_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            var di = image != null ? image.Source as DrawingImage : null;
            var gd = di != null ? di.Drawing as GeometryDrawing : null;
            if (gd != null) gd.Brush = Brushes.DarkOrchid;
        }

        private void Image_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var image = sender as Image;
            var di = image != null ? image.Source as DrawingImage : null;
            var gd = di != null ? di.Drawing as GeometryDrawing : null;
            if (gd != null) gd.Brush = Brushes.Aqua;
        }

        private void Path_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var sh = sender as Shape;
            var brush = new LinearGradientBrush(Colors.Black, Colors.White, 0);
            if (sh != null) sh.Fill = brush;
        }

        private void Path_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var sh = sender as Shape;
            if (sh != null) sh.Fill = Brushes.BlueViolet;
        }
    }
}
