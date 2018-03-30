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
    /// Represents the converter that executes consecutive two converters.
    /// </summary>
    public class CombiningConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the first converter to be executed.
        /// </summary>
        public IValueConverter Converter1 { get; set; }

        /// <summary>
        /// Gets or sets the second converter to be executed.
        /// </summary>
        public IValueConverter Converter2 { get; set; }


        /// <summary>
        /// Converts a value by passing it consecutive to the set converters.
        /// </summary>
        /// <param name="value">Value to be converted.</param>
        /// <param name="targetType">TargetType parameter is send to set converters.</param>
        /// <param name="parameter">This parameter is send to set converters.</param>
        /// <param name="culture">Culture parameter is send to set converters.</param>
        /// <returns>Converted value from the consecutive execution of the set converters.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object convertedValue = Converter1.Convert(value, targetType, parameter, culture);
            return Converter2.Convert(convertedValue, targetType, parameter, culture);
        }

        /// <summary>
        /// Converts back a value by passing it consecutive to the set converters in back order.
        /// </summary>
        /// <param name="value">Value to be converted back.</param>
        /// <param name="targetType">TargetType parameter is send to set converters.</param>
        /// <param name="parameter">This parameter is send to set converters.</param>
        /// <param name="culture">Culture parameter is send to set converters.</param>
        /// <returns>Converted value back from the reversed consecutive execution of the set converters.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object convertedValue = Converter2.ConvertBack(value, targetType, parameter, culture);
            return Converter1.ConvertBack(convertedValue, targetType, parameter, culture);
        }
    }
}
