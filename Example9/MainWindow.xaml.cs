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
            var pg = path.Data as PathGeometry;
            pg.Figures.Add(new PathFigure
            {
                StartPoint = _start.Value,
                IsClosed = false,
                IsFilled = false,
                Segments =
                {
                    new LineSegment {IsStroked = false}
                }
            });

        }

        private void Canvas_OnMouseDown_ellipse(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) _start = Mouse.GetPosition((UIElement)sender);
            var pg = path.Data as GeometryGroup;
            pg.Children.Add(new EllipseGeometry {Center = _start.Value});
        }

        private void Canvas_OnMouseDown_rect(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) _start = Mouse.GetPosition((UIElement)sender);
            var pg = path.Data as GeometryGroup;
            pg.Children.Add(new RectangleGeometry());
        }

        private void Canvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement) sender);
            var pg = path.Data as PathGeometry;
            var fig = pg.Figures.Last();
            fig.StartPoint = _start.Value;
            var seg = fig.Segments.Last() as LineSegment;
            seg.Point = p;
            _start = null;
        }

        private void Canvas_OnMouseUp_ellipse(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var gg = path.Data as GeometryGroup;
            var pg = gg.Children.Last() as EllipseGeometry;
            pg.RadiusX = Math.Abs(p.X - _start.Value.X);
            pg.RadiusY = Math.Abs(p.Y - _start.Value.Y);
            
            _start = null;
        }

        private void Canvas_OnMouseUp_rect(object sender, MouseButtonEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var gg = path.Data as GeometryGroup;
            var pg = gg.Children.Last() as RectangleGeometry;
            pg.Rect = new Rect(_start.Value, p);

            _start = null;
        }

        private void Canvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var pg = path.Data as PathGeometry;
            var figs = pg.Figures.Last();
            var seg = figs.Segments.Last() as LineSegment;
            seg.Point = p;
            seg.IsStroked = true;
        }

        private void Canvas_OnMouseMove_ellipse(object sender, MouseEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var gg = path.Data as GeometryGroup;
            var pg = gg.Children.Last() as EllipseGeometry;
            pg.RadiusX = Math.Abs(p.X - _start.Value.X);
            pg.RadiusY = Math.Abs(p.Y - _start.Value.Y);
            
        }

        private void Canvas_OnMouseMove_rect(object sender, MouseEventArgs e)
        {
            if (_start == null) return;
            var p = Mouse.GetPosition((UIElement)sender);
            var gg = path.Data as GeometryGroup;
            var pg = gg.Children.Last() as RectangleGeometry;
            pg.Rect = new Rect(_start.Value, p); 
            
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            var gg = path.Data as GeometryGroup;
            gg.Children.Clear();
        }
    }
}
