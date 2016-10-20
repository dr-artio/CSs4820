using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public MainWindow()
        // Constructor for Window object
        {
            InitializeComponent();

            // Numbered buttons
            foreach (int i in ViewModel.ButtonIds)
            {
                var b = new Button { FontSize = 20};
                b.SetBinding(ButtonBase.ContentProperty, string.Format("Buttons[{0}].Content", i));
                b.SetBinding(ButtonBase.IsEnabledProperty, string.Format("Buttons[{0}].IsEnabled", i));
                b.Tag = i;
                b.Click += ButtonClick;
                Pane.Children.Add(b);
            }
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;
            ViewModel.Click((int)b.Tag);
        }

        private void ShufflePane(object sender, RoutedEventArgs e)
        {
            ViewModel.Shuffle();
        }

        private void ResetPane(object sender, RoutedEventArgs e)
        {
            ViewModel.Reset();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            var focus = Keyboard.FocusedElement as UIElement;
                    
            switch (e.Key)
            {
                case Key.Tab:
                {
                    var direction = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
                    var tr =
                        new TraversalRequest(!direction
                            ? FocusNavigationDirection.Previous
                            : FocusNavigationDirection.Next);
                    e.Handled = true;
                    if (focus != null) focus.MoveFocus(tr);
                    break;
                   }
                case Key.G:
                    ViewModel.Shuffle();
                    break;

            }

        }
    }
}
