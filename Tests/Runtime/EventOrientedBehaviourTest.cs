using NUnit.Framework;
using UnityEngine;

namespace Andromeda.Tests.Runtime
{
    public class EventOrientedBehaviourTest
    {
        [Test]
        public void Raise_SomeObserversAreEnabledOnScene_ObserversReact()
        {
            GameObject gameObject = new GameObject();
            TestRaiser raiser = gameObject.AddComponent<TestRaiser>();
            TestObserver observer = gameObject.AddComponent<TestObserver>();
            Assert.IsFalse(observer.EventRaised);

            raiser.RaiseTestEvent();

            Assert.IsTrue(observer.EventRaised);
        }

        private struct TestEvent : IEvent { }

        private class TestRaiser : EventOrientedBehaviour
        {
            public void RaiseTestEvent()
            {
                Raise(new TestEvent());
            }
        }

        private class TestObserver : EventOrientedBehaviour
        {
            public bool EventRaised { get; private set; }

            [Observes]
            private void OnEventRaised(TestEvent @event)
            {
                EventRaised = true;
            }
        }
    }
}