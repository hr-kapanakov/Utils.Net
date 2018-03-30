using System;
using System.Globalization;
using System.Windows.Data;

namespace Utils.Net.Converters
{
    /// <summary>
    /// Represents the converter that converts String values to Uri values.
    /// </summary>
    [ValueConversion(typeof(string), typeof(Uri))]
    public class StringToUriConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="string"/> value to a <see cref="Uri"/> value.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to convert.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Uri object if value is not null or empty string; otherwise null.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            return string.IsNullOrEmpty(input) ?
                null : new Uri(input, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Converts a <see cref="Uri"/> value to a <see cref="string"/> value.
        /// </summary>
        /// <param name="value">A <see cref="Uri"/> value.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>String object if value is not null; otherwise null.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as Uri;
            return input == null ?
                string.Empty : input.ToString();
        }
    }
}
