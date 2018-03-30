using System.Windows;

namespace Utils.Net.Interactivity.TriggerActions
{
    /// <summary>
    /// Represents an attachable object that encapsulates a unit of functionality.
    /// </summary>
    /// <remarks>This is an infrastructure class. Action authors should derive from <see cref="TriggerAction{T}"/> instead of this class.</remarks>
    public abstract class TriggerAction : AttachedObject
    {
        /// <summary>
        /// Identifies the <see cref="IsEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty = 
            DependencyProperty.Register(
                nameof(IsEnabled), 
                typeof(bool), 
                typeof(TriggerAction), 
                new FrameworkPropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether this action will run when invoked.
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        internal void CallInvoke(object parameter)
        {
            if (IsEnabled)
            {
                Invoke(parameter);
            }
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">
        /// The parameter to the action. 
        /// If the action does not require a parameter, the parameter may be set to a null reference.
        /// </param>
        protected abstract void Invoke(object parameter);
    }

    /// <summary>
    /// Represents an attachable object that encapsulates a unit of functionality.
    /// </summary>
    /// <typeparam name="T">The type to which this action can be attached.</typeparam>
    public abstract class TriggerAction<T> : TriggerAction where T : DependencyObject
    {
        /// <summary>
        /// Gets the object to which this <see cref="TriggerAction{T}"/> is attached.
        /// </summary>
        protected new T AssociatedObject => (T)base.AssociatedObject;

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">The object to attach to.</param>
        public void Attach(T dependencyObject)
        {
            base.Attach(dependencyObject);
        }
    }
}
