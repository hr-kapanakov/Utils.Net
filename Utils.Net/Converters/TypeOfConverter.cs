using System;
using System.Globalization;
using System.Windows.Data;

namespace Utils.Net.Converters
{
    /// <summary>
    /// Represents the converter that converts a value to it's type.
    /// </summary>
    [ValueConversion(typeof(string), typeof(Type))]
    public class TypeOfConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value to it's type.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Returns type of the value if value is not null; otherwise null.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.GetType();
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
