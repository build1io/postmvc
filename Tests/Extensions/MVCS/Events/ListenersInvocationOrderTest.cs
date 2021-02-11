using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events
{
    public sealed class ListenersInvocationOrderTest
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
        public void OrderTest()
        {
            void Listener01()
            {
                _counter++;
                Assert.AreEqual(1, _counter);
            }

            void Listener02()
            {
                _counter++;
                Assert.AreEqual(2, _counter);
            }

            void Listener03()
            {
                _counter++;
                Assert.AreEqual(3, _counter);
            }

            _dispatcher.AddListener(TestEvent.Event00, Listener01);
            _dispatcher.AddListener(TestEvent.Event00, Listener02);
            _dispatcher.AddListener(TestEvent.Event00, Listener03);
            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(3, _counter);
        }

        [Test]
        public void OrderAfterListenerRemovedTest()
        {
            void Listener01()
            {
                _counter++;
                Assert.AreEqual(1, _counter);
            }

            void Listener02() { }

            void Listener03()
            {
                _counter++;
                Assert.AreEqual(2, _counter);
            }

            _dispatcher.AddListener(TestEvent.Event00, Listener01);
            _dispatcher.AddListener(TestEvent.Event00, Listener02);
            _dispatcher.AddListener(TestEvent.Event00, Listener03);
            _dispatcher.RemoveListener(TestEvent.Event00, Listener02);
            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(2, _counter);
        }
    }
}