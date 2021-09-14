using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events.EventMap
{
    public sealed class EventMapTests
    {
        private IEventMap        map;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            var dispatcher = new EventDispatcherWithCommandProcessing(new CommandBinder());

            _dispatcher = dispatcher;
            map = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventMap(dispatcher, new EventMapInfoPool());
        }

        [Test]
        public void MapTest()
        {
            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(TestEvent.Event00, Listener00);
            map.Map(TestEvent.Event01, Listener01);
            map.Map(TestEvent.Event02, Listener02);
            map.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);

            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);

            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 0);

            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);
        }

        [Test]
        public void MapWithSideDispatcherTest()
        {
            var dispatcher = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventDispatcher();

            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(dispatcher, TestEvent.Event00, Listener00);
            map.Map(dispatcher, TestEvent.Event01, Listener01);
            map.Map(dispatcher, TestEvent.Event02, Listener02);
            map.Map(dispatcher, TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            dispatcher.Dispatch(TestEvent.Event00);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);

            dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);

            dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 0);

            dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);
            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);
        }

        [Test]
        public void MapOnceTest()
        {
            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.MapOnce(TestEvent.Event00, Listener00);
            map.MapOnce(TestEvent.Event01, Listener01);
            map.MapOnce(TestEvent.Event02, Listener02);
            map.MapOnce(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event03, Listener03));
        }

        [Test]
        public void MapOnceWithSideDispatcherTest()
        {
            var dispatcher = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventDispatcher();

            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.MapOnce(dispatcher, TestEvent.Event00, Listener00);
            map.MapOnce(dispatcher, TestEvent.Event01, Listener01);
            map.MapOnce(dispatcher, TestEvent.Event02, Listener02);
            map.MapOnce(dispatcher, TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            dispatcher.Dispatch(TestEvent.Event00);
            dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            dispatcher.Dispatch(TestEvent.Event00);
            dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));
        }

        [Test]
        public void UnmapTest()
        {
            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(TestEvent.Event00, Listener00);
            map.Map(TestEvent.Event01, Listener01);
            map.Map(TestEvent.Event02, Listener02);
            map.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            map.Unmap(TestEvent.Event00, Listener00);
            map.Unmap(TestEvent.Event01, Listener01);
            map.Unmap(TestEvent.Event02, Listener02);
            map.Unmap(TestEvent.Event03, Listener03);

            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(count00, 0);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);
        }

        [Test]
        public void UnmapWithSideDispatcherTest()
        {
            var dispatcher = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventDispatcher();

            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(dispatcher, TestEvent.Event00, Listener00);
            map.Map(dispatcher, TestEvent.Event01, Listener01);
            map.Map(dispatcher, TestEvent.Event02, Listener02);
            map.Map(dispatcher, TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            map.Unmap(dispatcher, TestEvent.Event00, Listener00);
            map.Unmap(dispatcher, TestEvent.Event01, Listener01);
            map.Unmap(dispatcher, TestEvent.Event02, Listener02);
            map.Unmap(dispatcher, TestEvent.Event03, Listener03);

            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(count00, 0);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);
        }

        [Test]
        public void UnmapAllTest()
        {
            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(TestEvent.Event00, Listener00);
            map.Map(TestEvent.Event01, Listener01);
            map.Map(TestEvent.Event02, Listener02);
            map.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            map.UnmapAll();

            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(count00, 0);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);
        }

        [Test]
        public void UnmapAllWithSideDispatcherTest()
        {
            var dispatcher = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventDispatcher();

            var count00 = 0;
            var count01 = 0;
            var count02 = 0;
            var count03 = 0;

            void Listener00()                           => count00++;
            void Listener01(int p1)                     => count01++;
            void Listener02(int p1, string p2)          => count02++;
            void Listener03(int p1, string p2, bool p3) => count03++;

            map.Map(dispatcher, TestEvent.Event00, Listener00);
            map.Map(dispatcher, TestEvent.Event01, Listener01);
            map.Map(dispatcher, TestEvent.Event02, Listener02);
            map.Map(dispatcher, TestEvent.Event03, Listener03);

            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(true, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            map.UnmapAll();

            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event00, Listener00));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event01, Listener01));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event02, Listener02));
            Assert.AreEqual(false, map.ContainsMapInfo(dispatcher, TestEvent.Event03, Listener03));

            dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(count00, 0);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);
        }
    }
}