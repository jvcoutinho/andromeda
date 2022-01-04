using NUnit.Framework;

namespace Andromeda.Tests.Runtime
{
    [TestFixture(typeof(EventBusEventManager))]
    public class EventManagerTest<T> where T : IEventManager, new()
    {
        private struct TestEvent : IEvent { }

        [Test]
        public void Raise_ActionIsSubscribed_FiresAction()
        {
            bool eventRaised = false;

            void Action(TestEvent @event)
            {
                eventRaised = true;
            }

            T eventManager = new T();
            eventManager.Subscribe<TestEvent>(Action);

            eventManager.Raise(new TestEvent());

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void Raise_ActionIsNotSubscribed_DoesNotFireAction()
        {
            bool eventRaised = false;

            void Action(TestEvent @event)
            {
                eventRaised = true;
            }

            T eventManager = new T();
            eventManager.Subscribe<TestEvent>(Action);
            eventManager.Unsubscribe<TestEvent>(Action);

            eventManager.Raise(new TestEvent());

            Assert.IsFalse(eventRaised);
        }
    }
}