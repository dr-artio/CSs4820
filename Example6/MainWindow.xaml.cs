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

namespace Example6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _relativeCoord;
        private bool isLabelMoving = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Label_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Label) sender).Foreground = Brushes.Red;
            // Store coordinates relative to label
            _relativeCoord = Mouse.GetPosition((IInputElement)sender);
            isLabelMoving = true;
        }

        private void Label_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.Black;
            isLabelMoving = false;
        }

        private void Label_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ((Label)sender).Foreground = Brushes.Black;
            Label_OnMouseMove(sender, e);
        }

        private void Label_OnMouseEnter(object sender, MouseEventArgs e)
        {
//            if (e.LeftButton == MouseButtonState.Pressed)
//            {
//                Label_OnMouseDown(sender, null);
//            }
        }

        private void Label_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isLabelMoving)
            {
                Point absCoord = Mouse.GetPosition(this);
                // change coordinates of a label to keep relative coordinates the same
                var th = new Thickness
                {
                    Top = absCoord.Y - _relativeCoord.Y,
                    Left = absCoord.X - _relativeCoord.X
                };
                th.Top = Math.Max(0, th.Top);
                th.Left = Math.Max(0, th.Left);
                th.Top = Math.Min(ActualHeight - label.ActualHeight - 30, th.Top);
                th.Left = Math.Min(ActualWidth - label.ActualWidth, th.Left);
                label.Margin = th;
            }
        }
    }
}
