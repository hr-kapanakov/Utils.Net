using System.Collections.Specialized;
using System.Windows;

namespace Utils.Net.Interactivity.Triggers
{
    /// <summary>
    /// Represents an object that can invoke Actions conditionally.
    /// </summary>
    /// <remarks>
    /// This is an infrastructure class. 
    /// Trigger authors should derive from <see cref="Trigger{T}"/> instead of this class.
    /// </remarks>
    public abstract class Trigger : AttachedObject
    {
        /// <summary>
        /// Identifies the <see cref="Actions"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register(
                "ShadowActions", 
                typeof(FreezableCollection<TriggerActions.TriggerAction>), 
                typeof(Trigger));

        /// <summary>
        /// Gets the actions associated with this trigger.
        /// </summary>
        public FreezableCollection<TriggerActions.TriggerAction> Actions
        {
            get
            {
                var actions = (FreezableCollection<TriggerActions.TriggerAction>)GetValue(ActionsProperty);
                if (actions == null)
                {
                    actions = new FreezableCollection<TriggerActions.TriggerAction>();
                    ((INotifyCollectionChanged)actions).CollectionChanged += 
                        (_, e) => Actions_CollectionChanged(AssociatedObject, e);
                    SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }


        private static void Actions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(sender is DependencyObject dependencyObject))
            {
                return;
            }

            if (e.NewItems != null)
            {
                foreach (AttachedObject item in e.NewItems)
                {
                    item.Attach(dependencyObject);
                }
            }

            if (e.OldItems != null)
            {
                foreach (AttachedObject item in e.OldItems)
                {
                    item.Detach();
                }
            }
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            foreach (var action in Actions)
            {
                action.Attach(AssociatedObject);
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, 
        /// but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            foreach (var action in Actions)
            {
                action.Detach();
            }
        }


        /// <summary>
        /// Invoke all actions associated with this trigger.
        /// </summary>
        /// <param name="parameter">
        /// The parameter to the action. 
        /// If the action does not require a parameter, the parameter may be set to a null reference.
        /// </param>
        /// <remarks>Derived classes should call this to fire the trigger.</remarks>
        protected void InvokeActions(object parameter)
        {
            foreach (var action in Actions)
            {
                action.CallInvoke(parameter);
            }
        }
    }

    /// <summary>
    /// Represents an object that can invoke actions conditionally.
    /// </summary>
    /// <typeparam name="T">The type to which this trigger can be attached.</typeparam>
    /// <remarks>
    /// TriggerBase is the base class for controlling actions. 
    /// Override OnAttached() and OnDetaching() to hook and unhook handlers on the AssociatedObject. 
    /// You may constrain the types that a derived TriggerBase may be attached to by specifying the generic parameter. 
    /// Call InvokeActions() to fire all Actions associated with this TriggerBase.
    /// </remarks>
    public abstract class Trigger<T> : Trigger where T : DependencyObject
    {
        /// <summary>
        /// Gets the object to which this <see cref="Trigger{T}"/> is attached.
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
