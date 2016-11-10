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
        private double horizontalShift;
        private double verticalShift;

        public GameButton()
        {
            InitializeComponent();
            Translation.Completed += (sender, args) =>
            {
                Cell.BeginAnimation(MarginProperty, null);
                Cell.Margin = new Thickness(0);
            };
        }

        public static readonly DependencyProperty CellContentProperty =
            DependencyProperty.Register(
                "CellContent", typeof(object), typeof(GameButton), 
                new PropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MyVisibilityProperty =
            DependencyProperty.Register(
                "MyVisibility", typeof(Visibility), typeof(GameButton));

        public static readonly DependencyProperty XdirProperty = 
            DependencyProperty.Register(
                "Xdir", typeof(int), typeof(GameButton));

        public static readonly DependencyProperty YdirProperty =
            DependencyProperty.Register(
                "Ydir", typeof(int), typeof(GameButton));

        public event RoutedEventHandler Click
        {
            add { Cell.Click += value; }
            remove { Cell.Click -= value; }
        }
        
        public object CellContent
        {
            get { return GetValue(CellContentProperty); }
            set { SetValue(CellContentProperty, value); }
        }

        public Visibility MyVisibility
        {
            get { return (Visibility)GetValue(MyVisibilityProperty); }
            set { SetValue(MyVisibilityProperty, value); }
        }

        public new object Tag
        {
            get { return Cell.Tag; }
            set { Cell.Tag = value; }
        }

        public ThicknessAnimation Anim
        {
            get { return Translation; }
        }

        public Storyboard Story
        {
            get { return Str; }
        }

        public int Xdir { get { return (int) GetValue(XdirProperty); } set {SetValue(XdirProperty, value);} }
        public int Ydir { get { return (int) GetValue(YdirProperty); } set {SetValue(YdirProperty, value);} }

        private void GameButton_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            horizontalShift = e.NewSize.Width;
            verticalShift = e.NewSize.Height;
        }

        public void BeginTransalation()
        {
            Translation.To = GetShift(Xdir, Ydir);
            Str.Begin();
        }

        private Thickness GetShift(int xdir, int ydir)
        {
            xdir = Math.Sign(xdir);
            ydir = Math.Sign(ydir);
            if (xdir != 0 && ydir != 0)
                return new Thickness(0);
            return new Thickness(xdir*horizontalShift, ydir*verticalShift, -xdir*horizontalShift, -ydir*verticalShift);
        }
    }
}
