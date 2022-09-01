using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Events.EventDispatcher
{
    public sealed class OnceListenerAddedDuringInvocationTest
    {
        private IEventDispatcher _dispatcher;
        private int              _counter;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new MVCS.Events.Impl.EventDispatcher();
            _counter = 0;
        }

        /*
         * Event00.
         */

        [Test]
        public void OnceListenerAddedDuringInvocationTest00()
        {
            _dispatcher.AddListenerOnce(TestEvent.Event00, EventListener00_01);
            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(2, _counter);
        }

        private void EventListener00_01()
        {
            _dispatcher.AddListenerOnce(TestEvent.Event00, EventListener00_02);
            _counter++;
        }

        private void EventListener00_02()
        {
            _counter++;
        }

        /*
         * Event01.
         */

        [Test]
        public void OnceListenerAddedDuringInvocationTest01()
        {
            _dispatcher.AddListenerOnce(TestEvent.Event01, EventListener01_01);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);

            Assert.AreEqual(2, _counter);
        }

        private void EventListener01_01(int param01)
        {
            _dispatcher.AddListenerOnce(TestEvent.Event01, EventListener01_02);
            _counter++;
        }

        private void EventListener01_02(int param01)
        {
            _counter++;
        }

        /*
         * Event02.
         */

        [Test]
        public void OnceListenerAddedDuringInvocationTest02()
        {
            _dispatcher.AddListenerOnce(TestEvent.Event02, EventListener02_01);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);

            Assert.AreEqual(2, _counter);
        }

        private void EventListener02_01(int param01, string param02)
        {
            _dispatcher.AddListenerOnce(TestEvent.Event02, EventListener02_02);
            _counter++;
        }

        private void EventListener02_02(int param01, string param02)
        {
            _counter++;
        }

        /*
         * Event03.
         */

        [Test]
        public void OnceListenerAddedDuringInvocationTest03()
        {
            _dispatcher.AddListenerOnce(TestEvent.Event03, EventListener03_01);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(2, _counter);
        }

        private void EventListener03_01(int param01, string param02, bool param03)
        {
            _dispatcher.AddListenerOnce(TestEvent.Event03, EventListener03_02);
            _counter++;
        }

        private void EventListener03_02(int param01, string param02, bool param03)
        {
            _counter++;
        }
    }
}