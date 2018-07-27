using System.Windows;
using System.Windows.Controls;

namespace Utils.Net.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.Primitives.ToggleButton"/>.
    /// </summary>
    /// <remarks>
    /// Allow change of the <see cref="CornerRadius"/> of the button.
    /// </remarks>
    [TemplatePart(Name = "border", Type = typeof(Border))]
    public class ToggleButton : System.Windows.Controls.Primitives.ToggleButton
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ToggleButton),
                new PropertyMetadata(new CornerRadius(5), OnCornerRadiusChanged));

        /// <summary>
        /// Gets or sets the corner radius of the <see cref="Border"/>.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        private Border border;


        /// <summary>
        /// Is called when a control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            border = GetTemplateChild("border") as Border;
            if (border != null)
            {
                border.CornerRadius = CornerRadius;
            }
        }


        private static void OnCornerRadiusChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var button = (ToggleButton)target;
            if (button.border != null)
            {
                button.border.CornerRadius = button.CornerRadius;
            }
        }
    }
}
