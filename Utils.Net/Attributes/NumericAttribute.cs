using System;

namespace Utils.Net.Attributes
{
    /// <summary>
    /// Specifies that property is of numeric type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NumericAttribute : Attribute
    {
        /// <summary>
        /// Gets the string format used to convert value to <see cref="string"/>.
        /// </summary>
        public string StringFormat { get; }

        /// <summary>
        /// Gets the minimum value allowed.
        /// </summary>
        public double Minimum { get; }

        /// <summary>
        /// Gets the maximum value allowed.
        /// </summary>
        public double Maximum { get; }

        /// <summary>
        /// Gets the increment/decrement step of the value.
        /// </summary>
        public double Step { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericAttribute"/> class.
        /// </summary>
        /// <param name="stringFormat">The string format used to convert value to <see cref="string"/></param>
        /// <param name="minimum">The minimum value allowed.</param>
        /// <param name="maximum">The maximum value allowed.</param>
        /// <param name="step">The increment/decrement step of the value.</param>
        public NumericAttribute(
            string stringFormat = "G", 
            double minimum = double.NegativeInfinity, 
            double maximum = double.PositiveInfinity, 
            double step = 1.0)
        {
            StringFormat = stringFormat;
            Minimum = minimum;
            Maximum = maximum;
            Step = step;
        }
    }
}
