using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
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
using System.Windows.Threading;
using System.Xaml;
using System.Xml;
using Example5;

namespace Example51
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
                var b = new GameButton();
                //var lb = new Label {Margin = new Thickness(10)};
                //var b = new Button {FontSize = 25, Content = new Viewbox {Child = lb}};

                b.SetBinding(GameButton.CellContentProperty, string.Format("Buttons[{0}].Content", i));
                b.SetBinding(GameButton.MyVisibilityProperty, string.Format("Buttons[{0}].IsEnabled", i));
                b.SetBinding(GameButton.XdirProperty, string.Format("Buttons[{0}].Xdir", i));
                b.SetBinding(GameButton.YdirProperty, string.Format("Buttons[{0}].Ydir", i));
                b.Tag = i;
                b.Click += ButtonClick;
                b.Anim.Completed += (sender, args) => ViewModel.Refresh();
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

        private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOrdered" && ViewModel.IsOrdered && ViewModel.IsStarted)
            {
                MessageBox.Show(string.Format("Congratulations! Elapsed time: {0}", ViewModel.Elapsed));
            }
            int index;
            if (int.TryParse(e.PropertyName, out index))
            {
                var b = (GameButton)Pane.Children[index];
                b.Str.Begin();
            }
        }
    }
}
