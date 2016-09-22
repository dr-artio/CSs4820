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

namespace Example9
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point? _start = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) _start = Mouse.GetPosition((UIElement) sender);
        }

        private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement) sender);
            var pg = path.Data as PathGeometry;
            var figs = pg.Figures.First();
            figs.StartPoint = _start.Value;
            figs.Segments.Add(new LineSegment {Point = p});
        }

        private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var pg = path.Data as PathGeometry;
            var figs = pg.Figures.First();
            var seg = figs.Segments.Last() as LineSegment;
            seg.Point = p;
        }

        private void Canvas_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_start == null) return;
            var pg = path.Data as PathGeometry;
            var figs = pg.Figures.First();
            figs.Segments.RemoveAt(figs.Segments.Count - 1);
            _start = null;
        }
    }
}
