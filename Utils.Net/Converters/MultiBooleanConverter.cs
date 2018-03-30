using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Utils.Net.Converters
{
    /// <summary>
    /// Represents the converter that converts multi boolean to single one.
    /// </summary>
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class MultiBooleanConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts multi <see cref="bool"/> values to a single boolean.
        /// </summary>
        /// <param name="values">Multiple values to be converted.</param>
        /// <param name="targetType">TargetType parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>True if all of the value are true; otherwise false.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.OfType<bool>().All(b => b);
        }

        /// <summary>
        /// Converter cannot be used in back direction.
        /// </summary>
        /// <param name="value">Value parameter is not used.</param>
        /// <param name="targetTypes">TargetTypes parameter is not used.</param>
        /// <param name="parameter">This parameter is not used.</param>
        /// <param name="culture">Culture parameter is not used.</param>
        /// <returns>Returns NotImplementedException</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("The converter can be used only OneWay.");
        }
    }
}
