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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Example5
{
    /// <summary>
    /// Interaction logic for GameButton.xaml
    /// </summary>
    public partial class GameButton : UserControl
    {
        /// <summary>
        /// We store shifts vertical and horizontal for animaation
        /// since we need to move button on exactly actual width 
        /// horizontally and actual height vertically. Those values
        /// automatically updated when widow is resized.
        /// </summary>
        private double _horizontalShift;
        private double _verticalShift;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameButton()
        {
            InitializeComponent();
            Translation.Completed += (sender, args) =>
            {
                Cell.BeginAnimation(MarginProperty, null);
                Cell.Margin = new Thickness(0);
            };
        }

        /// <summary>
        /// Dependency properties for binding. It is only relevent for
        /// WPF and use of UserControl for this program
        /// </summary>
        public static readonly DependencyProperty XdirProperty = 
            DependencyProperty.Register(
                "Xdir", typeof(int), typeof(GameButton));

        public static readonly DependencyProperty YdirProperty =
            DependencyProperty.Register(
                "Ydir", typeof(int), typeof(GameButton));

        /// <summary>
        /// Redirectio of click event handler for our control to
        /// click event of button inside our control
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { Cell.Click += value; }
            remove { Cell.Click -= value; }
        }

        /// <summary>
        /// Redirecting Tag property to tag of button inside our control
        /// </summary>
        public new object Tag
        {
            get { return Cell.Tag; }
            set { Cell.Tag = value; }
        }

        /// <summary>
        /// Property to acces animation object
        /// </summary>
        public ThicknessAnimation Anim
        {
            get { return Translation; }
        }

        // Direction preperties
        public int Xdir { get { return (int) GetValue(XdirProperty); } set {SetValue(XdirProperty, value);} }
        public int Ydir { get { return (int) GetValue(YdirProperty); } set {SetValue(YdirProperty, value);} }

        /// <summary>
        /// Resize updates values for shifts for animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameButton_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _horizontalShift = e.NewSize.Width;
            _verticalShift = e.NewSize.Height;
        }

        /// <summary>
        /// Start animation, called when we click on the button
        /// or we ewant to move it.
        /// </summary>
        public void BeginTransalation()
        {
            Translation.To = GetShift(Xdir, Ydir);
            Str.Begin();
        }

        /// <summary>
        /// Get shift for animation "To" value. It is based on vertical 
        /// and horixzontal shifts and direction
        /// </summary>
        /// <param name="xdir">horizontal direction</param>
        /// <param name="ydir">vertical direction</param>
        /// <returns></returns>
        private Thickness GetShift(int xdir, int ydir)
        {
            xdir = Math.Sign(xdir);
            ydir = Math.Sign(ydir);
            if (xdir != 0 && ydir != 0)
                return new Thickness(0);
            return new Thickness(xdir*_horizontalShift, ydir*_verticalShift, -xdir*_horizontalShift, -ydir*_verticalShift);
        }
    }
}
