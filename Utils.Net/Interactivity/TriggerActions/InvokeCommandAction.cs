using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Utils.Net.Interactivity.TriggerActions
{
    /// <summary>
    /// Executes a specified ICommand when invoked.
    /// </summary>
    public class InvokeCommandAction : TriggerAction
    {
        /// <summary>
        /// Identifies the <see cref="Command"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register(
                nameof(Command), 
                typeof(ICommand), 
                typeof(InvokeCommandAction));

        /// <summary>
        /// Identifies the <see cref="CommandParameter"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = 
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object), 
                typeof(InvokeCommandAction));

        /// <summary>
        /// Gets or sets the command this action should invoke.
        /// </summary>
        /// <remarks>This property will take precedence over the CommandName property if both are set.</remarks>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the command parameter. 
        /// This is the value passed to ICommand.CanExecute and ICommand.Execute.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }


        private string commandName;

        /// <summary>
        /// Gets or sets the name of the command this action should invoke.
        /// </summary>
        /// <remarks>This property will be superseded by the Command property if both are set.</remarks>
        public string CommandName
        {
            get
            {
                ReadPreamble();
                return commandName;
            }
            set
            {
                if (CommandName != value)
                {
                    WritePreamble();
                    commandName = value;
                    WritePostscript();
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeCommandAction"/> class.
        /// </summary>
        public InvokeCommandAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeCommandAction"/> class.
        /// </summary>
        /// <param name="commandName">Name of the command this action should invoke. </param>
        public InvokeCommandAction(string commandName)
        {
            CommandName = commandName;
        }


        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">
        /// The parameter to the action. 
        /// If the action does not require a parameter, the parameter may be set to a null reference.
        /// </param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            parameter = CommandParameter ?? parameter;

            var command = ResolveCommand();
            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
        
        private ICommand ResolveCommand()
        {
            ICommand result = null;
            if (Command != null)
            {
                result = Command;
            }
            else if (AssociatedObject != null)
            {
                var type = AssociatedObject.GetType();
                var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var array = properties;
                for (int i = 0; i < array.Length; i++)
                {
                    var propertyInfo = array[i];
                    if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType) && 
                        string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
                    {
                        result = (ICommand)propertyInfo.GetValue(AssociatedObject, null);
                    }
                }
            }
            return result;
        }
    }
}
