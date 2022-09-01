using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Events.EventDispatcher
{
    public sealed class RemoveListenerTests
    {
        private IEventDispatcher _dispatcher;
        private int              _counter;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new Extensions.MVCS.Events.Impl.EventDispatcher();
            _counter = 0;
        }

        [Test]
        public void RemoveListenerTest00()
        {
            void Listener() => _counter++;

            _dispatcher.AddListener(TestEvent.Event00, Listener);
            Assert.AreEqual(true, _dispatcher.ContainsListener(TestEvent.Event00, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(1, _counter);

            _dispatcher.RemoveListener(TestEvent.Event00, Listener);
            Assert.AreEqual(false, _dispatcher.ContainsListener(TestEvent.Event00, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void RemoveListenerTest01()
        {
            void Listener(int param01) => _counter++;

            _dispatcher.AddListener(TestEvent.Event01, Listener);
            Assert.AreEqual(true, _dispatcher.ContainsListener(TestEvent.Event01, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            Assert.AreEqual(1, _counter);

            _dispatcher.RemoveListener(TestEvent.Event01, Listener);
            Assert.AreEqual(false, _dispatcher.ContainsListener(TestEvent.Event01, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void RemoveListenerTest02()
        {
            void Listener(int param01, string param02) => _counter++;

            _dispatcher.AddListener(TestEvent.Event02, Listener);
            Assert.AreEqual(true, _dispatcher.ContainsListener(TestEvent.Event02, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            Assert.AreEqual(1, _counter);

            _dispatcher.RemoveListener(TestEvent.Event02, Listener);
            Assert.AreEqual(false, _dispatcher.ContainsListener(TestEvent.Event02, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            Assert.AreEqual(1, _counter);
        }

        [Test]
        public void RemoveListenerTest03()
        {
            void Listener(int param01, string param02, bool param03) => _counter++;

            _dispatcher.AddListener(TestEvent.Event03, Listener);
            Assert.AreEqual(true, _dispatcher.ContainsListener(TestEvent.Event03, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            Assert.AreEqual(1, _counter);

            _dispatcher.RemoveListener(TestEvent.Event03, Listener);
            Assert.AreEqual(false, _dispatcher.ContainsListener(TestEvent.Event03, Listener));
            
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            Assert.AreEqual(1, _counter);
        }
    }
}