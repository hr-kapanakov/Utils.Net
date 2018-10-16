using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Utils.Net.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TextBox"/>.
    /// </summary>
    /// <remarks>
    /// Allow change of the <see cref="CornerRadius"/> of the ComboBox.
    /// Add <see cref="IconSource"/>, <see cref="Hint"/> text and <see cref="Filter"/> functionality.
    /// </remarks>
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    [TemplatePart(Name = "PART_EditableTextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_ToggleButton", Type = typeof(ToggleButton))]
    public class ComboBox : System.Windows.Controls.ComboBox
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ComboBox),
                new PropertyMetadata(new CornerRadius(5), OnCornerRadiusChanged));

        /// <summary>
        /// Gets or sets the corner radius of the <see cref="ComboBox"/>.
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
                typeof(ComboBox));

        /// <summary>
        /// Gets or sets the <see cref="ImageSource"/> of the <see cref="TextBox"/> icon.
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
                typeof(ComboBox));

        /// <summary>
        /// Gets or sets the hint text shown in the <see cref="TextBox"/>.
        /// </summary>
        public string Hint
        {
            get => (string)GetValue(HintProperty);
            set => SetValue(HintProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Filter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(
                nameof(Filter),
                typeof(Func<object, string, bool>),
                typeof(ComboBox));

        /// <summary>
        /// Gets or sets function for items filtering.
        /// </summary>
        public Func<object, string, bool> Filter
        {
            get => (Func<object, string, bool>)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        #endregion

        private TextBox textBox;
        private TextBox TextBox
        {
            get => textBox;
            set
            {
                if (textBox != null)
                {
                    textBox.IsKeyboardFocusedChanged -= TextBox_IsKeyboardFocusedChanged;
                    textBox.TextChanged -= TextBox_TextChanged;
                }

                textBox = value;

                if (textBox != null)
                {
                    textBox.IsKeyboardFocusedChanged += TextBox_IsKeyboardFocusedChanged;
                    textBox.TextChanged += TextBox_TextChanged;
                }
            }
        }

        private ToggleButton toggleButton;
        
        private bool selection = false;


        static ComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBox), new FrameworkPropertyMetadata(typeof(ComboBox)));
        }


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TextBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            toggleButton = GetTemplateChild("PART_ToggleButton") as ToggleButton;
            RefreshCornerRadiuses();
            StaysOpenOnEdit = true;
        }

        /// <summary>
        /// Responds to a <see cref="ComboBox"/> selection change by raising a 
        /// <see cref="Selector.SelectionChanged"/> event.
        /// </summary>
        /// <param name="e">Provides data for <see cref="SelectionChangedEventArgs"/></param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                selection = true;
            }

            base.OnSelectionChanged(e);
        }

        private void RefreshCornerRadiuses()
        {
            if (TextBox != null)
            {
                TextBox.CornerRadius = new CornerRadius(CornerRadius.TopLeft, 0, 0, CornerRadius.BottomLeft);
            }

            if (toggleButton != null)
            {
                if (IsEditable)
                {
                    toggleButton.CornerRadius = new CornerRadius(0, CornerRadius.TopRight, CornerRadius.BottomRight, 0);
                }
                else
                {
                    toggleButton.CornerRadius = CornerRadius;
                }
            }
        }

        private void TextBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (textBox.IsKeyboardFocused)
            {
                IsDropDownOpen = true;
            }
            else if (Hint != null) // clear the text on lost focus to show the hint
            {
                Text = string.Empty;
            }
        }

        // https://gist.github.com/mariodivece/0bbade976aea8d416d52
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsEditable)
            {
                return;
            }

            var searchText = textBox.Text;
            if (selection)
            {
                selection = false;
            }
            else
            {
                if (SelectionBoxItem != null)
                {
                    SelectedItem = null;
                    textBox.Text = searchText;
                }

                if (string.IsNullOrEmpty(searchText))
                {
                    Items.Filter = item => true;
                    SelectedItem = default(object);
                }
                else if (Filter != null)
                {
                    Items.Filter = o => Filter(o, searchText);
                }
                textBox.CaretIndex = textBox.Text.Length;
            }
            IsDropDownOpen = true;
        }


        private static void OnCornerRadiusChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            ((ComboBox)target).RefreshCornerRadiuses();
        }
    }
}
