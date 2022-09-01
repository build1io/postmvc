using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests
{
    public sealed class Command01AgilityTests
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
        public void SingleCommandExecutionTest()
        {
            var count01 = 0;
            var count00 = 0;
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };
            
            Command00.OnExecute = () =>
            {
                count00++;
            };
            
            _binder.Bind(CommandTestEvent.Event01).To1<Command01>();
            _binder.Bind(CommandTestEvent.Event01).To0<Command00>();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(1, count01);
            Assert.AreEqual(1, count00);
        }
        
        [Test]
        public void ParallelExecutionTest()
        {
            var count01 = 0;
            var count00 = 0;
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };
            
            Command00.OnExecute = () =>
            {
                count00++;
            };

            _binder.Bind(CommandTestEvent.Event01)
                   .To1<Command01>()
                   .To0<Command00>()
                   .To1<Command01>()
                   .To0<Command00>()
                   .InParallel();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(2, count01);
            Assert.AreEqual(2, count00);
        }
        
        [Test]
        public void SequenceExecutionTest()
        {
            var count01 = 0;
            var count00 = 0;
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };
            
            Command00.OnExecute = () =>
            {
                count00++;
            };

            _binder.Bind(CommandTestEvent.Event01)
                   .To1<Command01>()
                   .To0<Command00>()
                   .To1<Command01>()
                   .To0<Command00>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(2, count01);
            Assert.AreEqual(2, count00);
        }
    }
}