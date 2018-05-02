using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utils.Net.Common.Tests
{
    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void RelayCommandTest()
        {
            var validate = false;
            var command = new RelayCommand<bool>(b => validate = b, b => validate != b);
            command.CanExecuteChanged += Command_CanExecuteChanged; // for code coverage

            Assert.IsTrue(command.CanExecute(!validate));
            command.Execute(true);

            Assert.IsFalse(command.CanExecute(validate));
            Assert.IsTrue(validate);

            command.CanExecuteChanged -= Command_CanExecuteChanged; // for code coverage
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            Assert.Fail();
        }
    }
}
