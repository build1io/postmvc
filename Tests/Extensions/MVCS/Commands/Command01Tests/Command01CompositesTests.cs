using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests
{
    public sealed class Command01CompositesTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command01.OnExecute = null;
            Command01Copy.OnExecute = null;
            Command01DoubleDeinit.OnExecute = null;
            Command01DoubleDeinit.OnPostConstruct = null;
            Command01DoubleDeinit.OnPreDestroy = null;
            Command01Exception.OnExecute = null;
            Command01Fail.OnExecute = null;
            Command01Retain.OnExecute = null;
            Command01Retain.OnFail = null;
            Command01RetainExceptionInstant.OnExecute = null;
            Command01RetainFailInstant.OnExecute = null;
            Command01RetainReleaseInstant.OnExecute = null;
            CommandFailHandler.OnExecute = null;

            var binder = new CommandBinder();

            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing(binder);

            binder.InjectionBinder = new InjectionBinder();
            binder.Dispatcher = _dispatcher;
        }
        
        [Test]
        public void SingleExecutionsTest()
        {
            var count = 0;
            
            Command01.OnExecute += (param01) => { count++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .And(CommandTestEvent.Event01Copy01)
                   .Bind(CommandTestEvent.Event01Copy02)
                   .To<Command01>();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
            Assert.AreEqual(1, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy01, 0);
            Assert.AreEqual(2, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy02, 0);
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void ParallelExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;
            
            Command01.OnExecute += (param01) => { count00++; };
            Command01Copy.OnExecute += (param01) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .And(CommandTestEvent.Event01Copy01)
                   .Bind(CommandTestEvent.Event01Copy02)
                   .To<Command01>()
                   .To<Command01Copy>()
                   .InParallel();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy01, 0);
            
            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy02, 0);
            
            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
        
        [Test]
        public void SequenceExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;
            
            Command01.OnExecute += (param01) => { count00++; };
            Command01Copy.OnExecute += (param01) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .And(CommandTestEvent.Event01Copy01)
                   .Bind(CommandTestEvent.Event01Copy02)
                   .To<Command01>()
                   .To<Command01Copy>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy01, 0);
            
            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01Copy02, 0);
            
            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
    }
}