using System;
using System.Windows.Input;

namespace Utils.Net.Common
{
    /// <summary>
    /// Class representing RelayCommand. Implements ICommand interface.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Members

        /// <summary>
        /// Private member representing action to execute.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// Private member representing the can execute condition.
        /// </summary>
        private readonly Func<object, bool> canExecute;

        #endregion

        /// <summary>
        /// Can execute handler for can execute changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }


        /// <summary>
        /// Initializes a new instance of the<see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Can execute condition.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }


        /// <summary>
        /// Checks if a command can be executed.
        /// </summary>
        /// <param name="parameter">Action parameter.</param>
        /// <returns>Returns true if the command can be executed, otherwise returns false.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Executes the command action.
        /// </summary>
        /// <param name="parameter">Action parameter.</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

    /// <summary>
    /// Class representing generic RelayCommand. Implements ICommand interface.
    /// </summary>
    /// <typeparam name="T">The type the command parameter.</typeparam>
    public class RelayCommand<T> : RelayCommand
    {
        /// <summary>
        /// Initializes a new instance of the<see cref="RelayCommand{T}" /> class.
        /// </summary>
        /// <param name="execute">Action to execute.</param>
        /// <param name="canExecute">Can execute condition.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
            : base(o => execute((T)o), o => canExecute == null || canExecute((T)o))
        {
        }
    }
}
