using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utils.Net.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleInputDialog.xaml
    /// </summary>
    public partial class SimpleInputDialog : UserControl
    {
        /// <summary>
        /// Gets or sets the message <see cref="string"/> for input.
        /// </summary>
        public string Message
        {
            get => MessageTextBlock.Text;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    MessageTextBlock.Visibility = Visibility.Collapsed;
                    MessageTextBlock.Text = value;
                }
                else
                {
                    MessageTextBlock.Visibility = Visibility.Visible;
                    MessageTextBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the input of the user.
        /// </summary>
        public string Input
        {
            get => InputTextBox.Text;
            set
            {
                InputTextBox.Text = value;
                InputTextBox.SelectAll();
            }
        }

        private Window ParentWindow => Parent as Window;


        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInputDialog"/> class.
        /// </summary>
        public SimpleInputDialog()
        {
            InitializeComponent();

            InputTextBox.Focus();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Keyboard.KeyDownEvent"/> attached event 
        /// reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                CloseDialog(false);
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog(false);
        }

        private void CloseDialog(bool result)
        {
            if (ParentWindow != null)
            {
                try
                {
                    ParentWindow.DialogResult = result;
                }
                catch
                {
                    ParentWindow.Close();
                }
            }
        }


        /// <summary>
        /// Get <see cref="string"/> as user input by showing <see cref="SimpleInputDialog"/> window.
        /// </summary>
        /// <param name="caption">The caption of the showed window.</param>
        /// <param name="message">The message that will be shown to the user; default empty.</param>
        /// <param name="input">Text that will be suggested to the user as default input; default empty.</param>
        /// <returns>A <see cref="string"/> that user inputted or empty string if canceled.</returns>
        public static string GetText(string caption, string message = "", string input = "")
        {
            return GetText(null, caption, message, input);
        }

        /// <summary>
        /// Get <see cref="string"/> as user input by showing <see cref="SimpleInputDialog"/> window.
        /// </summary>
        /// <param name="owner">The window which owns this dialog.</param>
        /// <param name="caption">The caption of the showed dialog.</param>
        /// <param name="message">The message that will be shown to the user; default empty.</param>
        /// <param name="input">Text that will be suggested to the user as default input; default empty.</param>
        /// <returns>A <see cref="string"/> that user inputted or empty string if canceled.</returns>
        public static string GetText(Window owner, string caption, string message = "", string input = "")
        {
            var simpleInput = new SimpleInputDialog()
            {
                Message = message,
                Input = input
            };
            
            var window = new Window()
            {
                Title = caption,
                Content = simpleInput,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                MaxWidth = 300,
                Owner = owner ?? Application.Current?.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.SingleBorderWindow,
                ShowInTaskbar = false,
            };

            if (window.ShowDialog() == true)
            {
                return simpleInput.Input;
            }
            return string.Empty;
        }
    }
}
