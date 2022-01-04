using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace Andromeda
{
    /// <summary>
    ///     Subscribes all events and methods annotated with [Observes] upon enable and unsubscribes them upon disable.
    ///     It also defines an interface for raising an event.
    ///     Methods annotated with [Observes] must return void and contain one and only one concrete IEvent as argument.
    /// </summary>
    public abstract class EventOrientedBehaviour : MonoBehaviour
    {
        [AttributeUsage(AttributeTargets.Method)]
        protected class ObservesAttribute : Attribute { }

        private static readonly IEventManager EventManager = new EventBusEventManager();

        protected static void Raise<T>(T @event) where T : IEvent
        {
            EventManager.Raise(@event);
        }

        private void OnEnable()
        {
            SubscribeToObservingEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromObservingEvents();
        }

        private void SubscribeToObservingEvents()
        {
            CallGenericEventMethodForEachObservingEvent(nameof(IEventManager.Subscribe));
        }

        private void UnsubscribeFromObservingEvents()
        {
            CallGenericEventMethodForEachObservingEvent(nameof(IEventManager.Unsubscribe));
        }

        private void CallGenericEventMethodForEachObservingEvent(string name)
        {
            MethodInfo method = EventManager.GetType().GetMethod(name);
            foreach ((Type eventType, Delegate @delegate) in GetObservingEvents())
            {
                MethodInfo genericMethod = method!.MakeGenericMethod(eventType);
                genericMethod.Invoke(EventManager, new object[] { @delegate });
            }
        }

        private IEnumerable<(Type eventType, Delegate action)> GetObservingEvents()
        {
            Type thisType = GetType();
            MethodInfo[] methods = GetMethodsAnnotatedWith(thisType, typeof(ObservesAttribute));

            ICollection<(Type, Delegate)> events = new List<(Type, Delegate)>(methods.Length);
            foreach (MethodInfo method in methods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                Assert.AreEqual(1, parameters.Length);

                Type eventType = parameters[0].ParameterType;
                Assert.IsTrue(eventType.GetInterfaces().Any(t => t == typeof(IEvent)));

                Delegate @delegate = method.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), this);
                events.Add((eventType, @delegate));
            }

            return events;
        }

        private static MethodInfo[] GetMethodsAnnotatedWith(IReflect type, Type attributeType)
        {
            return type
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.CustomAttributes.Any(a => a.AttributeType == attributeType))
                .ToArray();
        }
    }
}