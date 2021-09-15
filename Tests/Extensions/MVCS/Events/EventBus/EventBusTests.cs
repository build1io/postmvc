using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Events.EventBus
{
    public sealed class EventBusTests
    {
        private IEventBus        _bus;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            var dispatcher = new EventDispatcherWithCommandProcessing(new CommandBinder());
            var bus = new Build1.PostMVC.Extensions.MVCS.Events.Impl.EventBus
            {
                Dispatcher = dispatcher
            };

            _dispatcher = dispatcher;
            _bus = bus;
        }

        [Test]
        public void AddAndDispatchTest()
        {
            var count = 0;
            void Handler()
            {
                count++;
            }
            
            _dispatcher.AddListener(TestEvent.Event00, Handler);
            _bus.Add(TestEvent.Event00);
            
            Assert.AreEqual(0, count);
            
            _bus.Dispatch();
            
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void DoubleDispatchTest()
        {
            var count = 0;
            void Handler()
            {
                count++;
            }
            
            _dispatcher.AddListener(TestEvent.Event00, Handler);
            _bus.Add(TestEvent.Event00);
            
            Assert.AreEqual(0, count);
            
            _bus.Dispatch();
            
            Assert.AreEqual(1, count);
            
            _bus.Dispatch();
            
            Assert.AreEqual(1, count);
        }

        [Test]
        public void AddInHandlerTest()
        {
            var count = 0;
            void Handler()
            {
                count++;
                _bus.Add(TestEvent.Event00);
            }
            
            _dispatcher.AddListener(TestEvent.Event00, Handler);
            _bus.Add(TestEvent.Event00);
            
            Assert.AreEqual(0, count);
            
            _bus.Dispatch();
            
            Assert.AreEqual(1, count);
            
            _bus.Dispatch();
            
            Assert.AreEqual(2, count);
        }
    }
}