using System;
using System.Reflection;
using System.Windows;

namespace Utils.Net.Interactivity.Triggers
{
    /// <summary>
    /// A trigger that listens for a specified event on its source and fires when that event is fired.
    /// </summary>
    public class EventTrigger : Trigger
    {
        /// <summary>
        /// Identifies the <see cref="EventName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EventNameProperty = 
            DependencyProperty.Register(
                nameof(EventName), 
                typeof(string), 
                typeof(EventTrigger), 
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnEventNameChanged)));

        /// <summary>
        /// Gets or sets the name of the event to listen for.
        /// </summary>
        public string EventName
        {
            get => (string)GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }
        

        private MethodInfo eventHandlerMethodInfo;


        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        public EventTrigger()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventTrigger"/> class.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        public EventTrigger(string eventName)
        {
            EventName = eventName;
        }


        private static void OnEventNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var eventTrigger = sender as EventTrigger;
            eventTrigger.UnregisterEvent(eventTrigger.AssociatedObject, (string)e.OldValue);
            eventTrigger.RegisterEvent(eventTrigger.AssociatedObject, (string)e.NewValue);
        }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            RegisterEvent(AssociatedObject, EventName);
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, 
        /// but before it has actually occurred.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            UnregisterEvent(AssociatedObject, EventName);
        }

        private void RegisterEvent(object obj, string eventName)
        {
            if (obj == null)
            {
                return;
            }

            var @event = obj.GetType().GetRuntimeEvent(eventName);
            if (@event == null)
            {
                throw new ArgumentException("EventTrigger cannot find EventName");
            }
            else if (!IsValidEvent(@event))
            {
                throw new ArgumentException("EventTriggerBase invalid event");
            }
            else
            {
                eventHandlerMethodInfo = 
                    typeof(EventTrigger).GetMethod("OnEventImpl", BindingFlags.Instance | BindingFlags.NonPublic);
                @event.AddEventHandler(
                    obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
            }
        }

        private static bool IsValidEvent(EventInfo eventInfo)
        {
            var eventHandlerType = eventInfo.EventHandlerType;
            if (!typeof(Delegate).GetTypeInfo().IsAssignableFrom(eventHandlerType.GetTypeInfo()))
            {
                return false;
            }
            var parameters = eventHandlerType.GetTypeInfo().GetDeclaredMethod("Invoke").GetParameters();
            if (parameters.Length == 2 && 
                typeof(object).GetTypeInfo().IsAssignableFrom(parameters[0].ParameterType.GetTypeInfo()))
            {
                return typeof(object).GetTypeInfo().IsAssignableFrom(parameters[1].ParameterType.GetTypeInfo());
            }
            else
            {
                return false;
            }
        }

        private void UnregisterEvent(object obj, string eventName)
        {
            if (obj == null)
            {
                return;
            }

            var type = obj.GetType();
            if (eventHandlerMethodInfo == null)
            {
                return;
            }
            var @event = type.GetEvent(eventName);
            @event.RemoveEventHandler(
                obj, Delegate.CreateDelegate(@event.EventHandlerType, this, eventHandlerMethodInfo));
            eventHandlerMethodInfo = null;
        }


        private void OnEventImpl(object sender, object eventArgs)
        {
            OnEvent(eventArgs);
        }

        /// <summary>
        /// Called when the event associated with this <see cref="EventTrigger"/> is fired. 
        /// By default, this will invoke all actions on the trigger.
        /// </summary>
        /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Override this to provide more granular control over when actions associated with this trigger will be invoked.
        /// </remarks>
        protected virtual void OnEvent(object eventArgs)
        {
            InvokeActions(eventArgs);
        }
    }
}
