using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Events.EventDispatcher
{
    public sealed class ListenersInvocationOrderTest
    {
        private IEventDispatcher _dispatcher;
        private int              _counter;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new MVCS.Events.Impl.EventDispatcher();
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