using System.Collections.Specialized;
using System.Windows;
using Utils.Net.Interactivity.Behaviors;

namespace Utils.Net.Interactivity
{
    /// <summary>
    /// Static class that owns the Triggers and Behaviors attached properties. 
    /// Handles propagation of AssociatedObject change notifications.
    /// </summary>
    public static class Interaction
    {
        /// <summary>
        /// Identifies the attached Behaviors dependency property.
        /// </summary>
        private static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached(
                "ShadowBehaviors", 
                typeof(FreezableCollection<Behavior>), 
                typeof(Interaction));

        /// <summary>
        /// Gets the Behaviors associated with a specified object.
        /// </summary>
        /// <param name="obj">The object from which to retrieve the Behaviors.</param>
        /// <returns>
        /// A <see cref="FreezableCollection{T}"/> containing the behaviors associated with the specified object.
        /// </returns>
        public static FreezableCollection<Behavior> GetBehaviors(DependencyObject obj)
        {
            var attachedBehaviors = (FreezableCollection<Behavior>)obj.GetValue(BehaviorsProperty);
            if (attachedBehaviors == null)
            {
                attachedBehaviors = new FreezableCollection<Behavior>();
                ((INotifyCollectionChanged)attachedBehaviors).CollectionChanged += 
                    (_, e) => AttachedCollectionChanged(obj, e);
                obj.SetValue(BehaviorsProperty, attachedBehaviors);
            }
            return attachedBehaviors;
        }


        /// <summary>
        /// Identifies the attached Triggers dependency property.
        /// </summary>
        private static readonly DependencyProperty TriggersProperty =
            DependencyProperty.RegisterAttached(
                "ShadowTriggers", 
                typeof(FreezableCollection<Triggers.Trigger>), 
                typeof(Interaction));

        /// <summary>
        /// Gets the Triggers associated with a specified object.
        /// </summary>
        /// <param name="obj">The object from which to retrieve the Triggers.</param>
        /// <returns>A <see cref="FreezableCollection{T}"/> containing the triggers associated with the specified object.</returns>
        public static FreezableCollection<Triggers.Trigger> GetTriggers(DependencyObject obj)
        {
            var attachedTriggers = (FreezableCollection<Triggers.Trigger>)obj.GetValue(TriggersProperty);
            if (attachedTriggers == null)
            {
                attachedTriggers = new FreezableCollection<Triggers.Trigger>();
                ((INotifyCollectionChanged)attachedTriggers).CollectionChanged += 
                    (_, e) => AttachedCollectionChanged(obj, e);
                obj.SetValue(TriggersProperty, attachedTriggers);
            }
            return attachedTriggers;
        }


        private static void AttachedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
    }
}
