using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events
{
    public sealed class UnregisteredListenerTests
    {
        private IEventDispatcher _dispatcher;
        private int              _counter;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new EventDispatcher();
            _counter = 0;
        }

        [Test]
        public void RemoveUnregisteredListener()
        {
            try
            {
                void Listener() => _counter++;
                _dispatcher.RemoveListener(TestEvent.Event00, Listener);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }

        [Test]
        public void InvokeUnregisteredListener()
        {
            void Listener() => _counter++;
            _dispatcher.RemoveListener(TestEvent.Event00, Listener);
            _dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(0, _counter);
        }
    }
}