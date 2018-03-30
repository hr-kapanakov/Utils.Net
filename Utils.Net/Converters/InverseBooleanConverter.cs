using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Utils.Net.Converters
{
    /// <summary>
    /// Represents the converter that inverse boolean value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="bool"/> value to its inverse.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Returns true if the set value is false; otherwise false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        /// <summary>
        /// Converts a <see cref="bool"/> value to its inverse.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> value to convert.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Returns true if the set value is false; otherwise false.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
