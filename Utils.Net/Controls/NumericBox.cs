using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Utils.Net.Controls
{
    /// <summary>
    /// The control provides a <see cref="TextBox"/> with button spinners that allow incrementing and decrementing<para/>
    /// double values by using the spinner buttons, keyboard up/down arrows, or mouse wheel.
    /// </summary>
    [TemplatePart(Name = "PART_ContentHost", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_UpButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_DownButton", Type = typeof(Button))]
    public class NumericBox : System.Windows.Controls.TextBox
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(NumericBox),
                new PropertyMetadata(new CornerRadius(5), OnCornerRadiusChanged));

        /// <summary>
        /// Gets or sets the corner radius of the <see cref="Border"/>.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }


        /// <summary>
        /// Gets the text contents of the text box.
        /// </summary>
        /// <remarks>Used to hide the base property.</remarks>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new string Text
        {
            get => base.Text;
            private set => base.Text = value;
        }


        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(0.0, OnValueChanged));

        /// <summary>
        /// Gets or sets the current value shown in the <see cref="NumericBox"/>.
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="StringFormat"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register(
                nameof(StringFormat),
                typeof(string),
                typeof(NumericBox),
                new PropertyMetadata("G", OnValueChanged));

        /// <summary>
        /// Gets or sets the string format used to convert <see cref="Value"/> to <see cref="string"/>.
        /// </summary>
        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="IsValid"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register(
                nameof(IsValid),
                typeof(bool),
                typeof(NumericBox),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets a value indicating whether the <see cref="Value"/> is a valid number.
        /// </summary>
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Minimum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(double.NegativeInfinity, OnValueChanged));

        /// <summary>
        /// Gets or sets the minimum <see cref="Value"/> allowed.
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Maximum"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(double.PositiveInfinity, OnValueChanged));

        /// <summary>
        /// Gets or sets the maximum <see cref="Value"/> allowed.
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }


        /// <summary>
        /// Identifies the <see cref="Step"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(
                nameof(Step),
                typeof(double),
                typeof(NumericBox),
                new PropertyMetadata(1.0));

        /// <summary>
        /// Gets or sets the increment/decrement step of the <see cref="Value"/>.
        /// </summary>
        public double Step
        {
            get => (double)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        #endregion


        private Button upButton;
        private Button UpButton
        {
            get => upButton;
            set
            {
                if (upButton != null)
                {
                    upButton.Click -= UpButton_Click;
                }

                upButton = value;

                if (upButton != null)
                {
                    upButton.Click += UpButton_Click;
                }
            }
        }

        private Button downButton;
        private Button DownButton
        {
            get => downButton;
            set
            {
                if (downButton != null)
                {
                    downButton.Click -= DownButton_Click;
                }

                downButton = value;

                if (downButton != null)
                {
                    downButton.Click += DownButton_Click;
                }
            }
        }


        static NumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericBox), new FrameworkPropertyMetadata(typeof(NumericBox)));
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox"/> class.
        /// </summary>
        public NumericBox()
        {
            base.Text = "0";
        }
        

        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpButton = GetTemplateChild("PART_UpButton") as Button;
            DownButton = GetTemplateChild("PART_DownButton") as Button;
            RefreshButtonsStyles();
        }

        /// <summary>
        /// Called when one or more of the dependency properties that exist on the element<para/>
        /// have had their effective values changed.
        /// </summary>
        /// <param name="e">Arguments for the associated event.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == BorderThicknessProperty)
            {
                RefreshButtonsStyles();
            }
        }

        private void RefreshButtonsStyles()
        {
            if (UpButton != null)
            {
                var borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(0.0, CornerRadius.TopRight, 0.0, 0.0)));
                UpButton.Resources[typeof(Border)] = borderStyle;
                UpButton.Margin = new Thickness(0, -BorderThickness.Top, -BorderThickness.Right, 0);
            }

            if (DownButton != null)
            {
                var borderStyle = new Style(typeof(Border));
                borderStyle.Setters.Add(new Setter(Border.CornerRadiusProperty, new CornerRadius(0.0, 0.0, CornerRadius.BottomRight, 0.0)));
                DownButton.Resources[typeof(Border)] = borderStyle;
                DownButton.Margin = new Thickness(0, 0, -BorderThickness.Right, -BorderThickness.Bottom);
            }
        }
        
        /// <summary>
        /// Is called when content in this editing control changes.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            var decimalSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            if (Text.EndsWith(decimalSeparator))
            {
                IsValid = false;
                return;
            }

            if (!double.TryParse(base.Text, out var value))
            {
                IsValid = false;
                return;
            }
            Value = value;
            IsValid = true;
        }

        /// <summary>
        /// Is called when a System.Windows.UIElement.MouseWheel event is routed to this<para/>
        /// class (or to a class that inherits from this class).
        /// </summary>
        /// <param name="e">The mouse wheel arguments that are associated with this event.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            int deltaLines = e.Delta / Mouse.MouseWheelDeltaForOneLine;
            Value += Step * deltaLines;
        }

        /// <summary>
        /// Invoked whenever an unhandled System.Windows.Input.Keyboard.KeyUp attached routed<para/>
        /// event reaches an element derived from this class in its route. Implement this<para/>
        /// method to add class handling for this event.
        /// </summary>
        /// <param name="e">Provides data about the event.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Key.Up)
            {
                Value += Step;
            }
            else if (e.Key == Key.Down)
            {
                Value -= Step;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }


        private static void OnCornerRadiusChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is NumericBox numericBox)
            {
                numericBox.RefreshButtonsStyles();
            }
        }

        private static void OnValueChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is NumericBox numericBox)
            {
                numericBox.Value = Math.Min(numericBox.Value, numericBox.Maximum);
                numericBox.Value = Math.Max(numericBox.Value, numericBox.Minimum);
                numericBox.Text = numericBox.Value.ToString(numericBox.StringFormat);
            }
        }
    }
}
