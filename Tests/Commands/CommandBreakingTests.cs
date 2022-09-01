using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands
{
    public sealed class CommandBreakingTests
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
        
        /*
         * Groups.
         */

        [Test]
        public void GroupBreakingTest()
        {
            var count = 0;
            var broken = 0;
            var complete = 0;
            var fails = 0;
            
            Command00Retain.OnExecute += () => { count++; };
            Command00.OnExecute += () => { count++; };
            Command00Complete.OnExecute += () => { complete++; };
            Command00Break.OnExecute += () => { broken++; };
            CommandFailHandler.OnExecute += (e) => { fails++; };

            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Complete>();
            _binder.Bind(CommandTestEvent.Event00Break).To<Command00Break>();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            
            _binder.Bind(CommandTestEvent.Event00)
                   .To<Command00Retain>()
                   .To<Command00>()
                   .OnComplete(CommandTestEvent.Event00Complete)
                   .OnBreak(CommandTestEvent.Event00Break)
                   .OnFail(CommandTestEvent.EventFail)
                   .InParallel();
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            _binder.BreakAll(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            Command00Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(1, broken);
            Assert.AreEqual(0, fails);
        }
        
        [Test]
        public void GroupBreakingWithFailTest()
        {
            var count = 0;
            var broken = 0;
            var complete = 0;
            var fails = 0;
            
            Command00Retain.OnExecute += () => { count++; };
            Command00.OnExecute += () => { count++; };
            Command00Complete.OnExecute += () => { complete++; };
            Command00Break.OnExecute += () => { broken++; };
            CommandFailHandler.OnExecute += (e) => { fails++; };

            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Complete>();
            _binder.Bind(CommandTestEvent.Event00Break).To<Command00Break>();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            
            _binder.Bind(CommandTestEvent.Event00)
                   .To<Command00Retain>()
                   .To<Command00>()
                   .OnComplete(CommandTestEvent.Event00Complete)
                   .OnBreak(CommandTestEvent.Event00Break)
                   .OnFail(CommandTestEvent.EventFail)
                   .InParallel();
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            _binder.BreakAll(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            Command00Retain.Instance.FailImpl();
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(1, broken);
            Assert.AreEqual(0, fails);
        }
        
        /*
         * Sequences.
         */
        
        [Test]
        public void SequenceBreakingTest()
        {
            var count = 0;
            var broken = 0;
            var complete = 0;
            var fails = 0;
            
            Command00Retain.OnExecute += () => { count++; };
            Command00.OnExecute += () => { count++; };
            Command00Complete.OnExecute += () => { complete++; };
            Command00Break.OnExecute += () => { broken++; };
            CommandFailHandler.OnExecute += (e) => { fails++; };

            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Complete>();
            _binder.Bind(CommandTestEvent.Event00Break).To<Command00Break>();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            
            _binder.Bind(CommandTestEvent.Event00)
                   .To<Command00Retain>()
                   .To<Command00>()
                   .OnComplete(CommandTestEvent.Event00Complete)
                   .OnBreak(CommandTestEvent.Event00Break)
                   .OnFail(CommandTestEvent.EventFail)
                   .InSequence();
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            _binder.BreakAll(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            Command00Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(1, broken);
            Assert.AreEqual(0, fails);
        }
        
        [Test]
        public void SequenceBreakingWithFailTest()
        {
            var count = 0;
            var broken = 0;
            var complete = 0;
            var fails = 0;
            
            Command00Retain.OnExecute += () => { count++; };
            Command00.OnExecute += () => { count++; };
            Command00Complete.OnExecute += () => { complete++; };
            Command00Break.OnExecute += () => { broken++; };
            CommandFailHandler.OnExecute += (e) => { fails++; };

            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Complete>();
            _binder.Bind(CommandTestEvent.Event00Break).To<Command00Break>();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            
            _binder.Bind(CommandTestEvent.Event00)
                   .To<Command00Retain>()
                   .To<Command00>()
                   .OnComplete(CommandTestEvent.Event00Complete)
                   .OnBreak(CommandTestEvent.Event00Break)
                   .OnFail(CommandTestEvent.EventFail)
                   .InSequence();
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            _binder.BreakAll(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(0, broken);
            Assert.AreEqual(0, fails);
            
            Command00Retain.Instance.FailImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, complete);
            Assert.AreEqual(1, broken);
            Assert.AreEqual(0, fails);
        }
    }
}