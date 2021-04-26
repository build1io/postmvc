using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events
{
    public sealed class EventMapperTests
    {
        private IEventMapper     _mapper;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            var dispatcher = new EventDispatcherWithCommandProcessing
            {
                CommandBinder = new CommandBinder()
            };

            _dispatcher = dispatcher;
            _mapper = new EventMapper(dispatcher);
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

            _mapper.Map(TestEvent.Event00, Listener00);
            _mapper.Map(TestEvent.Event01, Listener01);
            _mapper.Map(TestEvent.Event02, Listener02);
            _mapper.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

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

            _mapper.MapOnce(TestEvent.Event00, Listener00);
            _mapper.MapOnce(TestEvent.Event01, Listener01);
            _mapper.MapOnce(TestEvent.Event02, Listener02);
            _mapper.MapOnce(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);
            _dispatcher.Dispatch(TestEvent.Event01, int.MinValue);
            _dispatcher.Dispatch(TestEvent.Event02, int.MinValue, string.Empty);
            _dispatcher.Dispatch(TestEvent.Event03, int.MinValue, string.Empty, false);

            Assert.AreEqual(count00, 1);
            Assert.AreEqual(count01, 1);
            Assert.AreEqual(count02, 1);
            Assert.AreEqual(count03, 1);

            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));
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

            _mapper.Map(TestEvent.Event00, Listener00);
            _mapper.Map(TestEvent.Event01, Listener01);
            _mapper.Map(TestEvent.Event02, Listener02);
            _mapper.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _mapper.Unmap(TestEvent.Event00, Listener00);
            _mapper.Unmap(TestEvent.Event01, Listener01);
            _mapper.Unmap(TestEvent.Event02, Listener02);
            _mapper.Unmap(TestEvent.Event03, Listener03);

            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);

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

            _mapper.Map(TestEvent.Event00, Listener00);
            _mapper.Map(TestEvent.Event01, Listener01);
            _mapper.Map(TestEvent.Event02, Listener02);
            _mapper.Map(TestEvent.Event03, Listener03);

            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(true, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _mapper.UnmapAll();

            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event00, Listener00));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event01, Listener01));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event02, Listener02));
            Assert.AreEqual(false, _mapper.ContainsMapInfo(TestEvent.Event03, Listener03));

            _dispatcher.Dispatch(TestEvent.Event00);

            Assert.AreEqual(count00, 0);
            Assert.AreEqual(count01, 0);
            Assert.AreEqual(count02, 0);
            Assert.AreEqual(count03, 0);
        }
    }
}