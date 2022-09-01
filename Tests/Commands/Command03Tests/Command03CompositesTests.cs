using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command03Tests
{
    public sealed class Command03CompositesTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command03.OnExecute = null;
            Command03Copy.OnExecute = null;
            Command03DoubleDeinit.OnExecute = null;
            Command03DoubleDeinit.OnPostConstruct = null;
            Command03DoubleDeinit.OnPreDestroy = null;
            Command03Exception.OnExecute = null;
            Command03Fail.OnExecute = null;
            Command03Retain.OnExecute = null;
            Command03Retain.OnFail = null;
            Command03RetainExceptionInstant.OnExecute = null;
            Command03RetainFailInstant.OnExecute = null;
            Command03RetainReleaseInstant.OnExecute = null;
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

            Command03.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03)
                   .And(CommandTestEvent.Event03Copy01)
                   .Bind(CommandTestEvent.Event03Copy02)
                   .To<Command03>();

            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, string.Empty, null);
            Assert.AreEqual(1, count);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy01, 0, string.Empty, null);
            Assert.AreEqual(2, count);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy02, 0, string.Empty, null);
            Assert.AreEqual(3, count);
        }

        [Test]
        public void ParallelExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;

            Command03.OnExecute += (param01, param02, param03) => { count00++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event03)
                   .And(CommandTestEvent.Event03Copy01)
                   .Bind(CommandTestEvent.Event03Copy02)
                   .To<Command03>()
                   .To<Command03Copy>()
                   .InParallel();

            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, string.Empty, null);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy01, 0, string.Empty, null);

            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy02, 0, string.Empty, null);

            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }

        [Test]
        public void SequenceExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;

            Command03.OnExecute += (param01, param02, param03) => { count00++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event03)
                   .And(CommandTestEvent.Event03Copy01)
                   .Bind(CommandTestEvent.Event03Copy02)
                   .To<Command03>()
                   .To<Command03Copy>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, string.Empty, null);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy01, 0, string.Empty, null);

            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event03Copy02, 0, string.Empty, null);

            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
    }
}