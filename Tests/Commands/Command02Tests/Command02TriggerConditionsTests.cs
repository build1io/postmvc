using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests
{
    public sealed class Command02TriggerConditionsTests
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
         * Condition.
         */

        [Test]
        public void ConditionTrueTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            
            Command02.OnExecute += (p01, p02) =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition((param01, param02) =>
                    {
                        param01Received = param01;
                        param02Received = param02;
                        return true;
                    })
                   .To<Command02>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("12345", param02Received);
        }
        
        [Test]
        public void ConditionFalseTest()
        {
            var count = 0;
            var param01Received = 0;
            var param02Received = string.Empty;
            
            Command02.OnExecute += (p01, p02) =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition((param01, param02) =>
                    {
                        param01Received = param01;
                        param02Received = param02;
                        return false;
                    })
                   .To<Command02>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");

            Assert.AreEqual(0, count);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual("12345", param02Received);
        }
        
        [Test]
        public void MultipleConditions()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleConditionsBreak01()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return false;
                    })
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");
            
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleConditionsBreak02()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition((_, _) =>
                    {
                        count++;
                        return false;
                    })
                   .To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");
            
            Assert.AreEqual(2, count);
        }
        
        /*
         * Values.
         */
        
        [Test]
        public void ValueTest()
        {
            var count = 0;
            Command02.OnExecute += (_, _) => { count++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition(10, "12345")
                   .To<Command02>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 20, "12345");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "123456");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 20, "123456");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleValuesTest()
        {
            var count = 0;
            Command02.OnExecute += (_, _) => { count++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .TriggerCondition(10, "12345")
                   .TriggerCondition(11, "1112345")
                   .To<Command02>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 20, "12345");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "123456");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 20, "123456");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "12345");
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 11, "1112345");
            Assert.AreEqual(1, count);
        }
    }
}