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
    public sealed class Command00SingleTests
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
         * Single.
         */

        [Test]
        public void SingleTest()
        {
            var count = 0;
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void SingleMultipleBindingsTest()
        {
            var count = 0;
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingleMultipleDispatchesTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingleOnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().OnComplete(CommandTestEvent.Event00Complete);
            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCopy);
        }

        [Test]
        public void SingleOnFailTest()
        {
            var count = 0;
            var countException = 0;

            Command00Fail.OnExecute += () => { count++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void SingleExceptionTest()
        {
            var countException = 0;
            var countCatch = 0;

            Command00Exception.OnExecute += () => { countException++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Exception>();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                countCatch++;
            }

            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCatch);
        }
        
        [Test]
        public void SingleExceptionOnFailTest()
        {
            var countException = 0;
            var countExceptionsHandled = 0;

            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandled);
        }
        
        /*
         * Once.
         */
        
        [Test]
        public void OnceTest()
        {
            var count = 0;
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
        }
    }
}