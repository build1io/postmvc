using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using Build1.PostMVC.Core.Tests.Events.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Flow03Tests
{
    public sealed class Flow03Tests
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
            Command03RetainCopy.OnExecute = null;
            Command03RetainCopy.OnFail = null;
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
        public void ExecutionText()
        {
            var count = 0;
            var order = new List<int>(2);
            
            var value01 = new List<int>(2);
            var value02 = new List<string>(2);
            var value03 = new List<CommandData>(2);
            
            var data = new CommandData(0);
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                order.Add(0);
                value01.Add(param01);
                value02.Add(param02);
                value03.Add(param03);
            };
            Command03Copy.OnExecute += (param01, param02, param03) =>
            {
                count++;
                order.Add(1);
                value01.Add(param01);
                value02.Add(param02);
                value03.Add(param03);
            };
            
            _binder.Flow<int, string, CommandData>()
                   .To<Command03>()
                   .To<Command03Copy>()
                   .InSequence()
                   .Execute(10, "12345", data);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, order[0]);
            Assert.AreEqual(1, order[1]);
            Assert.AreEqual(10, value01[0]);
            Assert.AreEqual(10, value01[1]);
            Assert.AreEqual("12345", value02[0]);
            Assert.AreEqual("12345", value02[1]);
            Assert.AreEqual(data, value03[0]);
            Assert.AreEqual(data, value03[1]);
        }
        
        [Test]
        public void CompleteTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            var param02 = string.Empty;
            CommandData param03 = null;
            
            _dispatcher.AddListener(CommandTestEvent.Event03, (p01, p02, p03) =>
            {
                count++;
                param01 = p01;
                param02 = p02;
                param03 = p03;
            });

            var data = new CommandData(0);
            
            _binder.Flow<int, string, CommandData>()
                   .To<Command03>()
                   .To<Command03Copy>()
                   .OnComplete(CommandTestEvent.Event03)
                   .InSequence()
                   .Execute(10, "12345", data);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01);
            Assert.AreEqual("12345", param02);
            Assert.AreEqual(data, param03);
        }

        [Test]
        public void BreakTest()
        {
            var count = 0;
            var param01 = int.MinValue;
            var param02 = string.Empty;
            CommandData param03 = null;
            
            _dispatcher.AddListener(CommandTestEvent.Event03, (p01, p02, p03) =>
            {
                count++;
                param01 = p01;
                param02 = p02;
                param03 = p03;
            });
            
            var data = new CommandData(0);
            
            _binder.Flow<int, string, CommandData>()
                   .To<Command03Retain>()
                   .To<Command03Copy>()
                   .OnBreak(CommandTestEvent.Event03)
                   .InSequence()
                   .Execute(10, "12345", data);
            
            Assert.AreEqual(0, count);
            
            Command03Retain.Instance.BreakImpl();
            
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
            
            _binder.Flow<int, string, CommandData>()
                   .To<Command03Retain>()
                   .To<Command03Copy>()
                   .OnFail(TestEvent.EventFail)
                   .InSequence()
                   .Execute(10, "12345", new CommandData(0));
            
            Assert.AreEqual(0, count);
            
            Command03Retain.Instance.FailImpl();
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(Command03Retain.Exception, exception);
        }
    }
}