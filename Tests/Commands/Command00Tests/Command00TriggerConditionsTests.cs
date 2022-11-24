using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests
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

        [Test]
        public void MultipleConditions()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event00)
                   .TriggerCondition(() =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition(() =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleConditionsBreak01()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event00)
                   .TriggerCondition(() =>
                    {
                        count++;
                        return false;
                    })
                   .TriggerCondition(() =>
                    {
                        count++;
                        return true;
                    })
                   .To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void MultipleConditionsBreak02()
        {
            var count = 0;
            
            _binder.Bind(CommandTestEvent.Event00)
                   .TriggerCondition(() =>
                    {
                        count++;
                        return true;
                    })
                   .TriggerCondition(() =>
                    {
                        count++;
                        return false;
                    })
                   .To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
        }
    }
}