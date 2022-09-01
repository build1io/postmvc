using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Events.EventDispatcher
{
    public sealed class OnceListenerTests
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
        public void OnceListenerTest00()
        {
            void Listener() => _counter++;

            _dispatcher.AddListenerOnce(TestEvent.Event00, Listener);
            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void OnceListenerTest01()
        {
            void Listener(int param01) => _counter++;

            _dispatcher.AddListenerOnce(TestEvent.Event01, Listener);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);

            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void OnceListenerTest02()
        {
            void Listener(int param01, string param02) => _counter++;

            _dispatcher.AddListenerOnce(TestEvent.Event02, Listener);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);

            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void OnceListenerTest03()
        {
            void Listener(int param01, string param02, bool param03) => _counter++;

            _dispatcher.AddListenerOnce(TestEvent.Event03, Listener);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(1, _counter);
        }
    }
}