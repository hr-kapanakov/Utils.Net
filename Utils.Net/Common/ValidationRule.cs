using System.Globalization;
using System.Windows.Controls;

namespace Utils.Net.Common
{
    /// <summary>
    /// Provides a way to create a delegate rule in order to check the validity of user input.
    /// </summary>
    public class ValidationRule : System.Windows.Controls.ValidationRule
    {
        /// <summary>
        /// Represents the method that will handle validation of the set value.
        /// </summary>
        /// <param name="value">Value to be validated.</param>
        /// <param name="message">Message that will be returned if the validation failed.</param>
        /// <returns>True if the value is valid; otherwise false</returns>
        public delegate bool ValidationDelegate(object value, out string message);

        /// <summary>
        /// Gets or sets <see cref="BindingProxy"/> to the <see cref="ValidationDelegate"/>.
        /// </summary>
        public BindingProxy Validator { get; set; }


        /// <summary>
        /// Performs validation checks on a value.
        /// </summary>
        /// <param name="value">The value from the binding target to check.</param>
        /// <param name="cultureInfo">The culture to use in this rule.</param>
        /// <returns>A <see cref="ValidationResult"/> object.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = Validator.As<ValidationDelegate>().Invoke(value, out string message);
            if (result)
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, message);
            }
        }
    }
}
