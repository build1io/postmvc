using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Flow02Tests
{
    public sealed class Flow02Tests
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
            Command02RetainCopy.OnExecute = null;
            Command02RetainCopy.OnFail = null;
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
        public void ExecutionText()
        {
            var count = 0;
            var order = new List<int>(2);
            
            var value01 = new List<int>(2);
            var value02 = new List<string>(2);
            
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                order.Add(0);
                value01.Add(param01);
                value02.Add(param02);
            };
            Command02Copy.OnExecute += (param01, param02) =>
            {
                count++;
                order.Add(1);
                value01.Add(param01);
                value02.Add(param02);
            };
            
            _binder.Flow<int, string>()
                   .To<Command02>()
                   .To<Command02Copy>()
                   .InSequence()
                   .Execute(10, "12345");
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, order[0]);
            Assert.AreEqual(1, order[1]);
            Assert.AreEqual(10, value01[0]);
            Assert.AreEqual(10, value01[1]);
            Assert.AreEqual("12345", value02[0]);
            Assert.AreEqual("12345", value02[1]);
        }
        
        [Test]
        public void CompleteTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            var param02 = string.Empty;
            
            _dispatcher.AddListener(TestEvent.Event02, (p01, p02) =>
            {
                count++;
                param01 = p01;
                param02 = p02;
            });
            
            _binder.Flow<int, string>()
                   .To<Command02>()
                   .To<Command02Copy>()
                   .OnComplete(TestEvent.Event02)
                   .InSequence()
                   .Execute(10, "12345");
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01);
            Assert.AreEqual("12345", param02);
        }

        [Test]
        public void BreakTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            var param02 = string.Empty;
            
            _dispatcher.AddListener(TestEvent.Event02, (p01, p02) =>
            {
                count++;
                param01 = p01;
                param02 = p02;
            });
            
            _binder.Flow<int, string>()
                   .To<Command02Retain>()
                   .To<Command02Copy>()
                   .OnBreak(TestEvent.Event02)
                   .InSequence()
                   .Execute(10, "12345");
            
            Assert.AreEqual(0, count);
            
            Command02Retain.Instance.BreakImpl();
            
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
            
            _binder.Flow<int, string>()
                   .To<Command02Retain>()
                   .To<Command02Copy>()
                   .OnFail(TestEvent.EventFail)
                   .InSequence()
                   .Execute(10, "12345");
            
            Assert.AreEqual(0, count);
            
            Command02Retain.Instance.FailImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(Command02Retain.Exception, exception);
        }
    }
}