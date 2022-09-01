using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests
{
    public sealed class Command00CompositesTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command00.OnExecute = null;
            Command00Copy.OnExecute = null;
            Command00DoubleDeinit.OnExecute = null;
            Command00DoubleDeinit.OnPostConstruct = null;
            Command00DoubleDeinit.OnPreDestroy = null;
            Command00Exception.OnExecute = null;
            Command00Fail.OnExecute = null;
            Command00Retain.OnExecute = null;
            Command00Retain.OnFail = null;
            Command00RetainCopy.OnExecute = null;
            Command00RetainCopy.OnFail = null;
            Command00RetainExceptionInstant.OnExecute = null;
            Command00RetainFailInstant.OnExecute = null;
            Command00RetainReleaseInstant.OnExecute = null;
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
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00)
                   .And(CommandTestEvent.Event00Copy01)
                   .Bind(CommandTestEvent.Event00Copy02)
                   .To<Command00>();

            _dispatcher.Dispatch(CommandTestEvent.Event00);
            Assert.AreEqual(1, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy01);
            Assert.AreEqual(2, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy02);
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void ParallelExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;
            
            Command00.OnExecute += () => { count00++; };
            Command00Copy.OnExecute += () => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event00)
                   .And(CommandTestEvent.Event00Copy01)
                   .Bind(CommandTestEvent.Event00Copy02)
                   .To<Command00>()
                   .To<Command00Copy>()
                   .InParallel();

            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy01);
            
            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy02);
            
            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
        
        [Test]
        public void SequenceExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;
            
            Command00.OnExecute += () => { count00++; };
            Command00Copy.OnExecute += () => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event00)
                   .And(CommandTestEvent.Event00Copy01)
                   .Bind(CommandTestEvent.Event00Copy02)
                   .To<Command00>()
                   .To<Command00Copy>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy01);
            
            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00Copy02);
            
            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
    }
}