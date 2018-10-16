using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Utils.Net.Helpers;
using Utils.Net.Managers;

namespace Utils.Net.Dialogs
{
    /// <summary>
    /// Interaction logic for TutorialDialog.xaml
    /// </summary>
    public partial class TutorialDialog : UserControl
    {
        /// <summary>
        /// Gets the manager of the tutorial.
        /// </summary>
        public TutorialManager Manager { get; }

        /// <summary>
        /// Gets or sets the text of the previous button.
        /// </summary>
        public string PreviousButtonText
        {
            get => PreviousButton.Content.ToString();
            set => PreviousButton.Content = value;
        }

        /// <summary>
        /// Gets or sets the text of the next button.
        /// </summary>
        public string NextButtonText
        {
            get => NextButton.Content.ToString();
            set => NextButton.Content = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the close button should be visible.
        /// </summary>
        public bool IsCloseButtonVisible
        {
            get => CloseButton.Visibility == Visibility.Visible;
            set => CloseButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "Don't show again" checkbox will be visible.
        /// </summary>
        public bool IsCheckboxVisible
        {
            get => CheckBox.Visibility == Visibility.Visible;
            set => CheckBox.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialDialog" /> class.
        /// </summary>
        /// <param name="manager">Manager of the tutorial.</param>
        /// <param name="title">Title of the tutorial frame.</param>
        /// <param name="text">Text of the tutorial frame.</param>
        public TutorialDialog(TutorialManager manager, string title, string text)
        {
            InitializeComponent();

            Manager = manager ?? throw new ArgumentNullException(nameof(manager));
            TitleBlock.Text = title;
            TextBlock.Format(text, true);

            CheckBox.IsChecked = Manager.DontShowAgain;
            Loaded += (_, __) => NextButton.Focus();
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="Keyboard.PreviewKeyDownEvent"/> attached
        /// event reaches an element in its route that is derived from this class.
        /// </summary>
        /// <param name="e">The <see cref="KeyEventArgs"/> that contains the event data.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Manager.Stop();
            }
            else if (e.Key == Key.Left)
            {
                Manager.Previous();
            }
            else if (e.Key == Key.Right)
            {
                Manager.Next();
            }

            base.OnPreviewKeyDown(e);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Stop();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Previous();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            Manager.DontShowAgain = CheckBox.IsChecked.Value;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Next();
        }

        /// <summary>
        /// Show <see cref="TutorialDialog"/> popup.
        /// </summary>
        /// <param name="manager">Manager of the tutorial.</param>
        /// <param name="title">Title of the popup.</param>
        /// <param name="text">Text of the popup.</param>
        /// <param name="placement">Placement mode of the popup.</param>
        /// <param name="placementTarget">Placement target of the popup.</param>
        /// <param name="previousButtonText">Text of the Previous button of the popup; default "Previous".</param>
        /// <param name="nextButtonText">Text of the Next button of the popup; default "Next".</param>
        /// <param name="isCloseButtonVisible">Specifies if Close button of the popup should be visible; default true.</param>
        /// <param name="isCheckboxVisible">Specifies if "Don't show again" checkbox of the popup should be visible; default true.</param>
        /// <returns>Created popup.</returns>
        public static Popup ShowPopup(
            TutorialManager manager,
            string title,
            string text,
            PlacementMode placement,
            UIElement placementTarget,
            string previousButtonText = "Previous",
            string nextButtonText = "Next",
            bool isCloseButtonVisible = true,
            bool isCheckboxVisible = true)
        {
            var dialog = new TutorialDialog(manager, title, text)
            {
                PreviousButtonText = previousButtonText,
                NextButtonText = nextButtonText,
                IsCloseButtonVisible = isCloseButtonVisible,
                IsCheckboxVisible = isCheckboxVisible
            };

            var popup = new Popup
            {
                Child = dialog,
                Width = 300,
                Placement = placement,
                PlacementTarget = placementTarget,
                AllowsTransparency = true,
                IsOpen = true,
            };
            return popup;
        }

        /// <summary>
        /// Show border over the set target element with set color.
        /// </summary>
        /// <param name="target">Target element which will be highlighted.</param>
        /// <param name="color">Color of the border.</param>
        /// <param name="thickness">Thickness of the border.</param>
        /// <returns>Created popup with border.</returns>
        public static Popup ShowBorder(FrameworkElement target, Color color, Thickness thickness)
        {
            var border = new Border
            {
                BorderBrush = new SolidColorBrush(color),
                BorderThickness = thickness,
                CornerRadius = new CornerRadius(5),
                Background = Brushes.Transparent
            };

            var popupBorder = new Popup
            {
                Child = border,
                Placement = PlacementMode.Center,
                PlacementTarget = target,
                Width = target.ActualWidth + 6,
                Height = target.ActualHeight + 6,
                AllowsTransparency = true,
                IsOpen = true,
            };

            return popupBorder;
        }
    }
}
