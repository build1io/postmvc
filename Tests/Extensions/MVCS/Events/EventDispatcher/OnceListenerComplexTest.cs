using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events.EventDispatcher
{
    public sealed class OnceListenerComplexTest
    {
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            _dispatcher = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventDispatcher();
        }

        [Test]
        public void OnceListenerTest00()
        {
            ListenerClass00.Reset();

            var listener01 = new ListenerClass00(_dispatcher);
            var listener02 = new ListenerClass00(_dispatcher);

            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(2, ListenerClass00.Count);
        }

        [Test]
        public void OnceListenerTest01()
        {
            ListenerClass01.Reset();

            var listener01 = new ListenerClass01(_dispatcher);
            var listener02 = new ListenerClass01(_dispatcher);

            _dispatcher.Dispatch(TestEvent.Event01, 0);
            _dispatcher.Dispatch(TestEvent.Event01, 0);

            Assert.AreEqual(2, ListenerClass01.Count);
        }
        
        [Test]
        public void OnceListenerTest02()
        {
            ListenerClass02.Reset();

            var listener01 = new ListenerClass02(_dispatcher);
            var listener02 = new ListenerClass02(_dispatcher);

            _dispatcher.Dispatch(TestEvent.Event02, 0, null);
            _dispatcher.Dispatch(TestEvent.Event02, 0, null);

            Assert.AreEqual(2, ListenerClass02.Count);
        }
        
        [Test]
        public void OnceListenerTest03()
        {
            ListenerClass03.Reset();

            var listener01 = new ListenerClass03(_dispatcher);
            var listener02 = new ListenerClass03(_dispatcher);

            _dispatcher.Dispatch(TestEvent.Event03, 0, null, false);
            _dispatcher.Dispatch(TestEvent.Event03, 0, null, false);

            Assert.AreEqual(2, ListenerClass03.Count);
        }
    }

    class ListenerClass00
    {
        public static int Count { get; private set; }

        public ListenerClass00(IEventDispatcher dispatcher)
        {
            dispatcher.AddListenerOnce(TestEvent.Event00, OnOnce);
        }

        private       void OnOnce() { Count += 1; }
        public static void Reset()  { Count = 0; }
    }

    class ListenerClass01
    {
        public static int Count { get; private set; }

        public ListenerClass01(IEventDispatcher dispatcher)
        {
            dispatcher.AddListenerOnce(TestEvent.Event01, OnOnce);
        }

        private       void OnOnce(int value) { Count += 1; }
        public static void Reset()           { Count = 0; }
    }

    class ListenerClass02
    {
        public static int Count { get; private set; }

        public ListenerClass02(IEventDispatcher dispatcher)
        {
            dispatcher.AddListenerOnce(TestEvent.Event02, OnOnce);
        }

        private       void OnOnce(int value1, string value2) { Count += 1; }
        public static void Reset()                           { Count = 0; }
    }

    class ListenerClass03
    {
        public static int Count { get; private set; }

        public ListenerClass03(IEventDispatcher dispatcher)
        {
            dispatcher.AddListenerOnce(TestEvent.Event03, OnOnce);
        }

        private       void OnOnce(int value1, string value2, bool value3) { Count += 1; }
        public static void Reset()                                        { Count = 0; }
    }
}