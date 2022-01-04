using System;

namespace Andromeda
{
    /**
     * An event manager can raise an event to previously subscribed actions.
     */
    public interface IEventManager
    {
        void Raise<T>(T @event) where T : IEvent;
        void Subscribe<T>(Action<T> action) where T : IEvent;
        void Unsubscribe<T>(Action<T> action) where T : IEvent;
    }
}