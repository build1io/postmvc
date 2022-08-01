using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests
{
    public sealed class Command00TriggerConditionsTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;
        
        [SetUp]
        public void SetUp()
        {
            Command00.OnExecute = null;
            Command00Copy.OnExecute = null;
            Command00DoubleDeinit.OnExecute = null;
            Command00DoubleDeinit.OnPostConstruct = null;
            Command00DoubleDeinit.OnPreDestroy = null;
            Command00Exception.OnExecute = null;
            Command00Fail.OnExecute = null;
            Command00Retain.OnExecute = null;
            Command00Retain.OnFail = null;
            Command00RetainExceptionInstant.OnExecute = null;
            Command00RetainFailInstant.OnExecute = null;
            Command00RetainReleaseInstant.OnExecute = null;
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
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00)
                   .TriggerCondition(() => true)
                   .To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void ConditionFalseTest()
        {
            var count = 0;
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00)
                   .TriggerCondition(() => false)
                   .To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, count);
        }
    }
}