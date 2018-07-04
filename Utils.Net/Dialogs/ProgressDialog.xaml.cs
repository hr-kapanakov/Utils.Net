using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utils.Net.Dialogs
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : UserControl, IDisposable
    {
        /// <summary>
        /// Gets or sets the message <see cref="string"/> of the <see cref="ProgressDialog"/>.
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
        /// Gets or sets the maximum position of the <see cref="System.Windows.Controls.ProgressBar"/>.
        /// </summary>
        public double Maximum
        {
            get => ProgressBar.Maximum;
            set
            {
                if (value == 0)
                {
                    ProgressBar.IsIndeterminate = true;
                    ProgressBar.Maximum = value;
                }
                else
                {
                    ProgressBar.IsIndeterminate = false;
                    ProgressBar.Maximum = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current position of the <see cref="System.Windows.Controls.ProgressBar"/>.
        /// </summary>
        public double Current
        {
            get => ProgressBar.Value;
            set => ProgressBar.Value = value;
        }

        /// <summary>
        /// Gets or sets the sub-message <see cref="string"/> of the <see cref="ProgressDialog"/>.
        /// </summary>
        public string SubMessage
        {
            get => SubMessageTextBlock.Text;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SubMessageTextBlock.Visibility = Visibility.Collapsed;
                    SubMessageTextBlock.Text = value;
                }
                else
                {
                    SubMessageTextBlock.Visibility = Visibility.Visible;
                    SubMessageTextBlock.Text = value;
                }
            }
        }

        private Window ParentWindow => Parent as Window;

        private bool isProgressCancelled = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
            
            CancelButton.Focus();
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
                CloseDialog(true);
            }
        }


        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog(true);
        }

        private void CloseDialog(bool cancelled)
        {
            isProgressCancelled = cancelled;
            if (ParentWindow != null)
            {
                ParentWindow.Close();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CloseDialog(false);
            }
        }


        /// <summary>
        /// Updates the progress and data displayed in the dialog.
        /// </summary>
        /// <param name="current">Current position of the <see cref="System.Windows.Controls.ProgressBar"/>.</param>
        /// <param name="subMessage">Sub-message that will be shown to the user; default null.</param>
        /// <returns>Returns true if the progress is updated successfully. Returns false if the operation is cancelled</returns>
        public bool UpdateProgress(double current, string subMessage = null)
        {
            Dispatcher.Invoke(() =>
            {
                Current = current;
                SubMessage = subMessage;
            });

            return !isProgressCancelled;
        }

        /// <summary>
        /// Updates the progress and data displayed in the dialog.
        /// </summary>
        /// <param name="current">Current position of the <see cref="System.Windows.Controls.ProgressBar"/>.</param>
        /// <param name="maximum">Maximum position of the <see cref="System.Windows.Controls.ProgressBar"/>.</param>
        /// <param name="subMessage">Sub-message that will be shown to the user; default null.</param>
        /// <returns>Returns true if the progress is updated successfully. Returns false if the operation is cancelled</returns>
        public bool UpdateProgress(double current, double maximum, string subMessage = null)
        {
            Dispatcher.Invoke(() =>
            {
                Current = current;
                Maximum = maximum;
                SubMessage = subMessage;
            });

            return !isProgressCancelled;
        }


        /// <summary>
        /// Start new <see cref="ProgressDialog"/> window.
        /// </summary>
        /// <param name="caption">The caption of the showed dialog.</param>
        /// <param name="maximum">Maximum position of the <see cref="System.Windows.Controls.ProgressBar"/>. Zero for continues progress.</param>
        /// <param name="message">The message that will be shown to the user; default empty.</param>
        /// <returns>Created <see cref="ProgressDialog"/>.</returns>
        public static ProgressDialog Show(string caption, double maximum, string message = "")
        {
            return Show(null, caption, maximum, message);
        }

        /// <summary>
        /// Start new <see cref="ProgressDialog"/> window.
        /// </summary>
        /// <param name="owner">The window which owns this dialog.</param>
        /// <param name="caption">The caption of the showed dialog.</param>
        /// <param name="maximum">Maximum position of the <see cref="System.Windows.Controls.ProgressBar"/>. Zero for continues progress.</param>
        /// <param name="message">The message that will be shown to the user; default empty.</param>
        /// <returns>Created <see cref="ProgressDialog"/>.</returns>
        public static ProgressDialog Show(Window owner, string caption, double maximum, string message = "")
        {
            var content = new ProgressDialog()
            {
                Message = message,
                Maximum = maximum,
                SubMessage = null
            };

            var window = new Window()
            {
                Title = caption,
                Content = content,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.Height,
                Width = 300,
                Owner = owner ?? Application.Current?.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowStyle = WindowStyle.SingleBorderWindow,
                ShowInTaskbar = false,
            };
            
            window.Show();
            return content;
        }
    }
}
