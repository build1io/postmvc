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
    public sealed class Command01TriggerConditionsTests
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
         * Condition.
         */

        [Test]
        public void ConditionTrueTest()
        {
            var count = 0;
            var param01Received = 0;
            
            Command01.OnExecute += p01 =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(param01 =>
                    {
                        param01Received = param01;
                        return true;
                    })
                   .To<Command01>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Received);
        }
        
        [Test]
        public void ConditionFalseTest()
        {
            var count = 0;
            var param01Received = 0;
            
            Command01.OnExecute += p01 =>
            {
                count++;
            };

            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(param01 =>
                    {
                        param01Received = param01;
                        return false;
                    })
                   .To<Command01>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, count);
            Assert.AreEqual(10, param01Received);
        }
        
        [Test]
        public void MultipleConditions()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleConditionsBreak01()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return false;
                    })
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleConditionsBreak02()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition(_ =>
                    {
                        count++;
                        return false;
                    })
                   .To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(2, count);
        }
        
        /*
         * Values.
         */
        
        [Test]
        public void ValueTest()
        {
            var count = 0;
            Command01.OnExecute += _ => { count++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(10)
                   .To<Command01>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 20);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleValuesTest()
        {
            var count = 0;
            Command01.OnExecute += _ => { count++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerCondition(10)
                   .TriggerCondition(11)
                   .To<Command01>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 20);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 11);
            Assert.AreEqual(1, count);
        }
    }
}