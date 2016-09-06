using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Example5
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Button> _buttons = new List<Button>();
        private Button _empty;
        private const int SIZE = 4;
        private const int SHUFFLE_DEPTH = 1000;

        public MainWindow()
        // Constructor for Window object
        {
            InitializeComponent();

            // Numbered buttons
            int l = SIZE*SIZE - 1;
            for (int i = 0; i < l; ++i)
            {
                var b = new Button {Content = i+1, FontSize = 20};
                b.Click += ButtonClick;
                _buttons.Add(b);
            }
            // Empty button
            _empty = new Button {Content = "", IsEnabled = false};
            _buttons.Add(_empty);

            FillGrid();
        }

        private void FillGrid()
        {
            // Reset the buttons' positions
            Pane.Children.Clear();
            _buttons.ForEach(b => Pane.Children.Add(b));
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            int i = _buttons.IndexOf((Button) sender);
            int j = _buttons.IndexOf(_empty);
            // Coordinates of button we clicked
            int xi = i%SIZE, yi = i/SIZE;
            // Coordinates of empty button
            int xj = j%SIZE, yj = j/SIZE;

            // Swap buttons if empty and clicked are neigbors
            if (Math.Abs(xi - xj) + Math.Abs(yi - yj) == 1)
            {
                _buttons[j] = _buttons[i];
                _buttons[i] = _empty;
            }
            FillGrid();
        }

        private void ShufflePane(object sender, RoutedEventArgs e)
        {
            var rnd = new Random();
            for (int i = 0; i < SHUFFLE_DEPTH; ++i)
            {
                int j = rnd.Next(_buttons.Count);
                
                _buttons[j].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        class ButtonCmp : IComparer<Button>
        {
            private Button _empty;
            public ButtonCmp(Button empty)
            {
                _empty = empty;
            }
            public int Compare(Button x, Button y)
            {
                if (x == _empty) return 1;
                if (y == _empty) return -1;
                int ix = int.Parse(x.Content.ToString());
                int iy = int.Parse(y.Content.ToString());
                return ix.CompareTo(iy);
            }
        }

        private void ResetPane(object sender, RoutedEventArgs e)
        {
            
            _buttons.Sort(new ButtonCmp(_empty));
//            _buttons = _buttons.OrderBy(
//                b => b == _empty
//                    ? int.MaxValue
//                    : int.Parse(b.Content.ToString())).ToList();
            FillGrid();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
