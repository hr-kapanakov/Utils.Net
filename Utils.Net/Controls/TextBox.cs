using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Utils.Net.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TextBox"/>.
    /// </summary>
    /// <remarks>
    /// Allow change of the <see cref="CornerRadius"/> of the border. 
    /// Add <see cref="IconSource"/>, <see cref="Hint"/> text and clear button.
    /// </remarks>
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(TextBox));

        /// <summary>
        /// Gets or sets the corner radius of the <see cref="Border"/>.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="IconSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register(
                nameof(IconSource),
                typeof(ImageSource),
                typeof(TextBox));

        /// <summary>
        /// Gets or sets the <see cref="ImageSource"/> of the icon.
        /// </summary>
        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Hint"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register(
                nameof(Hint),
                typeof(string),
                typeof(TextBox));

        /// <summary>
        /// Gets or sets the hint text shown in the <see cref="TextBox"/>.
        /// </summary>
        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="ShowClearButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register(
                nameof(ShowClearButton),
                typeof(bool),
                typeof(TextBox));

        /// <summary>
        /// Gets or sets a value indicating whether clean button should be shown.
        /// </summary>
        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        #endregion


        private Button clearButton;
        private Button ClearButton
        {
            get => clearButton;
            set
            {
                if (clearButton != null)
                {
                    clearButton.Click -= ClearButton_Click;
                }

                clearButton = value;

                if (clearButton != null)
                {
                    clearButton.Click += ClearButton_Click;
                }
            }
        }


        static TextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(typeof(TextBox)));
        }


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ClearButton = GetTemplateChild("PART_ClearButton") as Button;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Text = string.Empty;
        }
    }
}
