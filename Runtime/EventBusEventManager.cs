using System;
using System.Collections.Generic;
using UnityEngine;

namespace Andromeda
{
    /**
     * Implementation of event manager with an event bus (dictionary of actions).
     * The key to the dictionary is the concrete type of the event.
     */
    public class EventBusEventManager : IEventManager
    {
        private readonly IDictionary<Type, Delegate> actions;

        public EventBusEventManager(IDictionary<Type, Delegate> actions = null)
        {
            this.actions = actions ?? new Dictionary<Type, Delegate>();
        }

        /**
         * Raises all actions subscribed to the given event.
         */
        public void Raise<T>(T @event) where T : IEvent
        {
            Type eventType = typeof(T);
            if (!actions.TryGetValue(eventType, out Delegate @delegate))
            {
                Debug.LogWarning($"Event {eventType.Name} raised without subscribers.");
                return;
            }

            Action<T> action = (Action<T>)@delegate;
            action?.Invoke(@event);
        }

        /**
         * Subscribes the given action to the given event.
         */
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            Type eventType = typeof(T);
            if (actions.TryGetValue(eventType, out Delegate @delegate))
            {
                actions[eventType] = Delegate.Combine(@delegate, action);
                return;
            }

            actions.Add(eventType, action);
        }

        /**
         * Unsubscribes the given action from the given event.
         */
        public void Unsubscribe<T>(Action<T> action) where T : IEvent
        {
            Type eventType = typeof(T);
            if (!actions.TryGetValue(eventType, out Delegate @delegate)) return;

            Delegate finalDelegate = Delegate.Remove(@delegate, action);
            if (finalDelegate == null) actions.Remove(eventType);
            else actions[eventType] = finalDelegate;
        }
    }
}