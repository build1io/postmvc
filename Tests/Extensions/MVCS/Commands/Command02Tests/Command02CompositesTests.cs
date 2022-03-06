using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests
{
    public sealed class Command02CompositesTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command02.OnExecute = null;
            Command02Copy.OnExecute = null;
            Command02DoubleDeinit.OnExecute = null;
            Command02DoubleDeinit.OnPostConstruct = null;
            Command02DoubleDeinit.OnPreDestroy = null;
            Command02Exception.OnExecute = null;
            Command02Fail.OnExecute = null;
            Command02Retain.OnExecute = null;
            Command02Retain.OnFail = null;
            Command02RetainExceptionInstant.OnExecute = null;
            Command02RetainFailInstant.OnExecute = null;
            Command02RetainReleaseInstant.OnExecute = null;
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

            Command02.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .And(CommandTestEvent.Event02Copy01)
                   .Bind(CommandTestEvent.Event02Copy02)
                   .To<Command02>();

            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);
            Assert.AreEqual(1, count);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy01, 0, string.Empty);
            Assert.AreEqual(2, count);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy02, 0, string.Empty);
            Assert.AreEqual(3, count);
        }

        [Test]
        public void ParallelExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;

            Command02.OnExecute += (param01, param02) => { count00++; };
            Command02Copy.OnExecute += (param01, param02) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .And(CommandTestEvent.Event02Copy01)
                   .Bind(CommandTestEvent.Event02Copy02)
                   .To<Command02>()
                   .To<Command02Copy>()
                   .InParallel();

            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy01, 0, string.Empty);

            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy02, 0, string.Empty);

            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }

        [Test]
        public void SequenceExecutionsTest()
        {
            var count00 = 0;
            var count00Copy = 0;

            Command02.OnExecute += (param01, param02) => { count00++; };
            Command02Copy.OnExecute += (param01, param02) => { count00Copy++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .And(CommandTestEvent.Event02Copy01)
                   .Bind(CommandTestEvent.Event02Copy02)
                   .To<Command02>()
                   .To<Command02Copy>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);

            Assert.AreEqual(1, count00);
            Assert.AreEqual(1, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy01, 0, string.Empty);

            Assert.AreEqual(2, count00);
            Assert.AreEqual(2, count00Copy);

            _dispatcher.Dispatch(CommandTestEvent.Event02Copy02, 0, string.Empty);

            Assert.AreEqual(3, count00);
            Assert.AreEqual(3, count00Copy);
        }
    }
}