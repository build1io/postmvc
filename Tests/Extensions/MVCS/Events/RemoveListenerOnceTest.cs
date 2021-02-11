using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events
{
    public sealed class RemoveListenerOnceTest
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
        public void RemoveListenerOnceTest00()
        {
            void Listener() => _counter++;
            _dispatcher.AddListenerOnce(TestEvent.Event00, Listener);
            _dispatcher.RemoveListener(TestEvent.Event00, Listener);
            _dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(0, _counter);
        }

        [Test]
        public void RemoveListenerOnceTest01()
        {
            void Listener(int param01) => _counter++;
            _dispatcher.AddListenerOnce(TestEvent.Event01, Listener);
            _dispatcher.RemoveListener(TestEvent.Event01, Listener);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            Assert.AreEqual(0, _counter);
        }

        [Test]
        public void RemoveListenerOnceTest02()
        {
            void Listener(int param01, string param02) => _counter++;
            _dispatcher.AddListenerOnce(TestEvent.Event02, Listener);
            _dispatcher.RemoveListener(TestEvent.Event02, Listener);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            Assert.AreEqual(0, _counter);
        }

        [Test]
        public void RemoveListenerOnceTest03()
        {
            void Listener(int param01, string param02, bool param03) => _counter++;
            _dispatcher.AddListenerOnce(TestEvent.Event03, Listener);
            _dispatcher.RemoveListener(TestEvent.Event03, Listener);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            Assert.AreEqual(0, _counter);
        }
    }
}