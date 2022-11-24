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
    public sealed class Command03TriggerConditionsTests
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
         * Condition.
         */

        [Test]
        public void ConditionTrueTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            var param03Received = new CommandData(1);
            
            Command03.OnExecute += (p01, p02, p03) =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition((param01, param02, param03) =>
                    {
                        param01Received = param01;
                        param02Received = param02;
                        param03Received = param03;
                        return true;
                    })
                   .To<Command03>();

            var data = new CommandData(2);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", data);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("12345", param02Received);
            Assert.AreEqual(data, param03Received);
        }
        
        [Test]
        public void ConditionFalseTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            var param03Received = new CommandData(1);
            
            Command03.OnExecute += (p01, p02, p03) =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition((param01, param02, param03) =>
                    {
                        param01Received = param01;
                        param02Received = param02;
                        param03Received = param03;
                        return false;
                    })
                   .To<Command03>();

            var data = new CommandData(2);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", data);

            Assert.AreEqual(0, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("12345", param02Received);
            Assert.AreEqual(data, param03Received);
        }
        
        [Test]
        public void MultipleConditions()
        {
            var count = 0;
            var data = new CommandData(2);
            
            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", data);
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleConditionsBreak01()
        {
            var count = 0;
            var data = new CommandData(2);
            
            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return false;
                    })
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", data);
            
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleConditionsBreak02()
        {
            var count = 0;
            var data = new CommandData(2);
            
            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition((_, _, _) =>
                    {
                        count++;
                        return false;
                    })
                   .To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", data);
            
            Assert.AreEqual(2, count);
        }
        
        /*
         * Values.
         */
        
        [Test]
        public void ValueTest()
        {
            var count = 0;
            Command03.OnExecute += (_, _, _) => { count++; };

            var dataRequired = new CommandData(1); 
            
            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition(10, "12345", dataRequired)
                   .To<Command03>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "12345", dataRequired);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "123456", dataRequired);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "123456", dataRequired);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "123456", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "12345", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", dataRequired);
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleValuesTest()
        {
            var count = 0;
            Command03.OnExecute += (_, _, _) => { count++; };

            var dataRequired01 = new CommandData(1); 
            var dataRequired02 = new CommandData(2); 
            
            _binder.Bind(CommandTestEvent.Event03)
                   .TriggerCondition(10, "12345", dataRequired01)
                   .TriggerCondition(11, "1112345", dataRequired02)
                   .To<Command03>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "12345", dataRequired01);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "123456", dataRequired01);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "123456", dataRequired01);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "123456", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 20, "12345", null);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "12345", dataRequired01);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 11, "1112345", dataRequired02);
            Assert.AreEqual(1, count);
        }
    }
}