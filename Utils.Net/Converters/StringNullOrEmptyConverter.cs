using System;
using System.Globalization;
using System.Windows.Data;

namespace Utils.Net.Converters
{
    /// <summary>
    /// Represents the converter that converts a <see cref="string"/> to <see cref="bool"/> by calling string.IsNullOrEmpty function.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class StringNullOrEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="string"/> to <see cref="bool"/> by calling string.IsNullOrEmpty function.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Returns true if the string value is null or empty; otherwise false.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string);
        }

        /// <summary>
        /// Converter cannot be used in back direction.
        /// </summary>
        /// <param name="value">Value parameter is not used.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Throws <see cref="InvalidOperationException"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("The converter can be used only OneWay.");
        }
    }
}
