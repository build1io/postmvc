using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Flow01Tests
{
    public sealed class Flow01Tests
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
            Command01RetainCopy.OnExecute = null;
            Command01RetainCopy.OnFail = null;
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
        public void ExecutionText()
        {
            var count = 0;
            var order = new List<int>(2);
            var value = new List<int>(2);

            Command01.OnExecute += param01 =>
            {
                count++;
                order.Add(0);
                value.Add(param01);
            };
            Command01Copy.OnExecute += param01 =>
            {
                count++;
                order.Add(1);
                value.Add(param01);
            };

            _binder.Flow<int>()
                   .To<Command01>()
                   .To<Command01Copy>()
                   .InSequence()
                   .Execute(10);

            Assert.AreEqual(2, count);
            Assert.AreEqual(0, order[0]);
            Assert.AreEqual(1, order[1]);
            Assert.AreEqual(10, value[0]);
            Assert.AreEqual(10, value[1]);
        }

        [Test]
        public void CompleteTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            
            _dispatcher.AddListener(TestEvent.Event01, p01 =>
            {
                count++;
                param01 = p01;
            });
            
            _binder.Flow<int>()
                   .To<Command01>()
                   .To<Command01Copy>()
                   .OnComplete(TestEvent.Event01)
                   .InSequence()
                   .Execute(10);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01);
        }

        [Test]
        public void BreakTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            
            _dispatcher.AddListener(TestEvent.Event01, p01 =>
            {
                count++;
                param01 = p01;
            });
            
            _binder.Flow<int>()
                   .To<Command01Retain>()
                   .To<Command01Copy>()
                   .OnBreak(TestEvent.Event01)
                   .InSequence()
                   .Execute(10);
            
            Assert.AreEqual(0, count);
            
            Command01Retain.Instance.BreakImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01);
        }

        [Test]
        public void FailTest()
        {
            var count = 0;
            Exception exception = null;

            _dispatcher.AddListener(TestEvent.EventFail, e =>
            {
                count++;
                exception = e;
            });
            
            _binder.Flow<int>()
                   .To<Command01Retain>()
                   .To<Command01Copy>()
                   .OnFail(TestEvent.EventFail)
                   .InSequence()
                   .Execute(10);
            
            Assert.AreEqual(0, count);
            
            Command01Retain.Instance.FailImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(Command01Retain.Exception, exception);
        }
    }
}