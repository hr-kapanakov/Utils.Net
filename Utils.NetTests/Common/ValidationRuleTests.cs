using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Common.Tests
{
    [TestClass]
    public class ValidationRuleTests
    {
        [TestMethod]
        public void ValidateTest()
        {
            var testValidationRule = new ValidationRule
            {
                Validator = new BindingProxy
                {
                    Data = (ValidationRule.ValidationDelegate)TestValidation
                }
            };

            var result = testValidationRule.Validate("test", null);
            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.ErrorContent);

            result = testValidationRule.Validate(123, null);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(result.ErrorContent, string.Empty);
        }


        private bool TestValidation(object value, out string message)
        {
            message = string.Empty;
            return value is string;
        }
    }
}
