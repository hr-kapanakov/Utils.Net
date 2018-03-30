using System.Windows;

namespace Utils.Net.Interactivity.Behaviors
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <remarks>
    /// This is an infrastructure class. 
    /// Behavior authors should derive from <see cref="Behavior{T}"/> instead of from this class.
    /// </remarks>
    public abstract class Behavior : AttachedObject
    {
    }

    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <typeparam name="T">The type the <see cref="AttachedObject"/> can be attached to.</typeparam>
    /// <remarks>
    /// Behavior is the base class for providing attachable state and commands to an object. 
    /// The types the Behavior can be attached to can be controlled by the generic parameter. 
    /// Override OnAttached() and OnDetaching() methods to hook and unhook any necessary handlers from the AssociatedObject.
    /// </remarks>
    public abstract class Behavior<T> : Behavior where T : DependencyObject
    {
        /// <summary>
        /// Gets the object to which this <see cref="Behavior{T}"/> is attached.
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
