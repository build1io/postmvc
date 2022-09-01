using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
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
        
        /*
         * Values.
         */
        
        [Test]
        public void ValuesTest()
        {
            var count = 0;
            Command01.OnExecute += p01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .TriggerValue(10)
                   .To<Command01>();
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 20);
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            Assert.AreEqual(1, count);
        }
    }
}