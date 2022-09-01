using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests
{
    public sealed class Command01SingleTests
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
        
        /*
         * Single.
         */

        [Test]
        public void SingleTest()
        {
            var count = 0;
            var param01Received = 0;
            Command01.OnExecute += param01 =>
            {
                count++;
                param01Received = param01;
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
        }
        
        [Test]
        public void SingleMultipleBindingsTest()
        {
            var count = 0;
            var param = new List<int>();
            Command01.OnExecute += param01 =>
            {
                count++;
                param.Add(param01);
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param[0]);
            Assert.AreEqual(10, param[1]);
        }

        [Test]
        public void SingleMultipleDispatchesTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingleOnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().OnComplete(CommandTestEvent.Event01Complete);
            _binder.Bind(CommandTestEvent.Event01Complete).To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCopy);
        }

        [Test]
        public void SingleOnFailTest()
        {
            var count = 0;
            var countException = 0;

            Command01Fail.OnExecute += param01 => { count++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void SingleExceptionTest()
        {
            var countException = 0;
            var countCatch = 0;

            Command01Exception.OnExecute += param01 => { countException++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Exception>();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
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

            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

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
            var paramLast = 0;
            Command01.OnExecute += param01 =>
            {
                count++;
                paramLast = param01;
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, paramLast);
        }
    }
}