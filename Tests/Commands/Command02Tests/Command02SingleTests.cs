using System.Collections.Generic;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests
{
    public sealed class Command02SingleTests
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
        
        /*
         * Single.
         */

        [Test]
        public void SingleTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                param01Received = param01;
                param02Received = param02;
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("1", param02Received);
        }
        
        [Test]
        public void SingleMultipleBindingsTest()
        {
            var count = 0;
            var param01List = new List<int>();
            var param02List = new List<string>();
            
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                param01List.Add(param01);
                param02List.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param01List[0]);
            Assert.AreEqual(10, param01List[1]);
            Assert.AreEqual("1", param02List[0]);
            Assert.AreEqual("1", param02List[1]);
        }

        [Test]
        public void SingleMultipleDispatchesTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SingleOnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().OnComplete(CommandTestEvent.Event02Complete);
            _binder.Bind(CommandTestEvent.Event02Complete).To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCopy);
        }

        [Test]
        public void SingleOnFailTest()
        {
            var count = 0;
            var countException = 0;

            Command02Fail.OnExecute += (param01, param02) => { count++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void SingleExceptionTest()
        {
            var countException = 0;
            var countCatch = 0;

            Command02Exception.OnExecute += (param01, param02) => { countException++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Exception>();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
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

            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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
            
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                param01Last = param01;
                param02Last = param02;
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Last);
            Assert.AreEqual("1", param02Last);
        }
    }
}