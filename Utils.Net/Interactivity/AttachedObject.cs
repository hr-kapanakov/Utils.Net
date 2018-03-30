using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Utils.Net.Interactivity
{
    /// <summary>
    /// Object that can be attached to another dependency object.
    /// </summary>
    public abstract class AttachedObject : Animatable
    {
        /// <summary>
        /// Gets the associated object.
        /// </summary>
        public DependencyObject AssociatedObject { get; private set; }


        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">The object to attach to.</param>
        public void Attach(DependencyObject dependencyObject)
        {
            if (dependencyObject == AssociatedObject)
            {
                return;
            }

            if (AssociatedObject != null)
            {
                throw new InvalidOperationException("Cannot host attached object multiple times.");
            }

            AssociatedObject = dependencyObject;
            OnAttached();
        }

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            OnDetaching();
            AssociatedObject = null;
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected virtual void OnAttached()
        {
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, 
        /// but before it has actually occurred.
        /// </summary>
        protected virtual void OnDetaching()
        {
        }


        /// <summary>
        /// Creates a new instance of the <see cref="Freezable"/> derived class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            var type = GetType();
            return (Freezable)Activator.CreateInstance(type);
        }
    }
}
