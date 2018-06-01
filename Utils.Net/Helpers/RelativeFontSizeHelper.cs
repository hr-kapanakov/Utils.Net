using System;
using System.Windows;

namespace Utils.Net.Helpers
{
    /// <summary>
    /// Helper class which allow setting a relative FontSize.
    /// </summary>
    public sealed class RelativeFontSizeHelper
    {
        #region Attached Dependency Properties

        /// <summary>
        /// Identifies the attached RelativeFontSize dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeFontSizeProperty =
            DependencyProperty.RegisterAttached(
                "RelativeFontSize",
                typeof(double),
                typeof(RelativeFontSizeHelper),
                new PropertyMetadata((double)0, RelativeFontSizeChanged));

        /// <summary>
        /// Get value of the <see cref="RelativeFontSizeProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object from which the value will be get.</param>
        /// <returns>Value of the <see cref="RelativeFontSizeProperty"/> dependency property.</returns>
        public static double GetRelativeFontSize(DependencyObject obj)
        {
            return (double)obj.GetValue(RelativeFontSizeProperty);
        }

        /// <summary>
        /// Set value to the <see cref="RelativeFontSizeProperty"/> dependency property.
        /// </summary>
        /// <param name="obj">Dependency object to which the value will be set.</param>
        /// <param name="value">Value which will be set to the <see cref="RelativeFontSizeProperty"/> dependency property.</param>
        public static void SetRelativeFontSize(DependencyObject obj, double value)
        {
            obj.SetValue(RelativeFontSizeProperty, value);
        }

        #endregion


        private static void RelativeFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fontSizeProp = d.GetType().GetProperty("FontSize", typeof(double));

            if (fontSizeProp == null || e.Property != RelativeFontSizeProperty)
            {
                return;
            }

            var fontSize = (double)fontSizeProp.GetValue(d);
            fontSizeProp.SetValue(d, Math.Max(fontSize + (double)e.NewValue, 0));
        }
    }
}
