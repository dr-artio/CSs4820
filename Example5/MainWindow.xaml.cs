using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Example5
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Contructor for MAinWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler for button clicks. Switch flag of annimation to true
        /// to prevent starting another animation and retrive list of
        /// buttons to move. Then start animation of all buttons in the 
        /// list. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var b = (Button)sender;     
            
            if (ViewModel.IsAnimationInProgress) return;
            
            foreach (var i in ViewModel.IdsToShift((int) b.Tag))
            {
                var ci = Pane.Children[i] as GameButton;
                ci.BeginTransalation();
                ViewModel.IsAnimationInProgress = true;
            }
            
        }

        
        /// <summary>
        /// Handler for shuffling (starting new game)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Tabulation behavior override (not very relevant)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Property changed handler. It shows congratulations message box when game if finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsOrdered" && ViewModel.IsOrdered && ViewModel.IsStarted)
            {
                MessageBox.Show(string.Format("Congratulations! Elapsed time: {0}", ViewModel.Elapsed));
            }
        }

        private void LoadedWindow(object sender, RoutedEventArgs e)
        {
            // Numbered buttons
            foreach (int i in ViewModel.ButtonIds)
            {
                var b = new GameButton();

                b.BLabel.SetBinding(ContentProperty, string.Format("Buttons[{0}].Content", i));
                b.SetBinding(VisibilityProperty, string.Format("Buttons[{0}].Visibility", i));
                b.SetBinding(GameButton.XdirProperty, string.Format("Buttons[{0}].Xdir", i));
                b.SetBinding(GameButton.YdirProperty, string.Format("Buttons[{0}].Ydir", i));
                b.Tag = i;
                b.Click += ButtonClick;
                b.Anim.Completed += (s, args) => ViewModel.Click(i);
                Pane.Children.Add(b);
            }
        }
    }
}
