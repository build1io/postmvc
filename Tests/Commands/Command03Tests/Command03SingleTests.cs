using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command03Tests
{
    public sealed class Command03SingleTests
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
        
        /*
         * Single.
         */

        [Test]
        public void SingleTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            var param03Received = new CommandData(1);
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                param01Received = param01;
                param02Received = param02;
                param03Received = param03;
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("1", param02Received);
            Assert.AreEqual(null, param03Received);
        }
        
        [Test]
        public void SingleMultipleBindingsTest()
        {
            var count = 0;
            var param01List = new List<int>();
            var param02List = new List<string>();
            var param03List = new List<CommandData>();
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                param01List.Add(param01);
                param02List.Add(param02);
                param03List.Add(param03);
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param01List[0]);
            Assert.AreEqual(10, param01List[1]);
            Assert.AreEqual("1", param02List[0]);
            Assert.AreEqual("1", param02List[1]);
            Assert.AreEqual(null, param03List[0]);
            Assert.AreEqual(null, param03List[1]);
        }

        [Test]
        public void SingleMultipleDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingleOnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCopy);
        }

        [Test]
        public void SingleOnFailTest()
        {
            var count = 0;
            var countException = 0;

            Command03Fail.OnExecute += (param01, param02, param03) => { count++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void SingleExceptionTest()
        {
            var countException = 0;
            var countCatch = 0;

            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Exception>();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
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

            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

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
            var param01Last = 0;
            var param02Last = string.Empty;
            var param03Last = new CommandData(1);
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                param01Last = param01;
                param02Last = param02;
                param03Last = param03;
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Last);
            Assert.AreEqual("1", param02Last);
            Assert.AreEqual(null, param03Last);
        }
    }
}