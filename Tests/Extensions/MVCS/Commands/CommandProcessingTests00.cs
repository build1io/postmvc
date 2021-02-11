using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands
{
    public sealed class CommandProcessingTests00
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command00.OnExecute = null;
            Command00Copy.OnExecute = null;

            var binder = new CommandBinder(new InjectionBinder());
            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing(new EventDispatcher(), binder);
        }

        [Test]
        public void CommandExecutionTest()
        {
            var executed = false;
            Command00.OnExecute += () => { executed = true; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.IsTrue(executed);
        }

        [Test]
        public void MultipleBindingsExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void MultipleCommandsExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SequenceExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void MultipleCommandsExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command00.OnExecute += () => { indexes.Add(0); };
            Command00Copy.OnExecute += () => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }

        [Test]
        public void SequenceExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command00.OnExecute += () => { indexes.Add(0); };
            Command00Copy.OnExecute += () => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }

        [Test]
        public void OnceTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void OnceMultipleTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void OnceSequenceTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void MultipleExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void MultipleMultipleExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(4, count);
        }

        [Test]
        public void MultipleSequenceExecutionTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(4, count);
        }

        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Retaining.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retaining>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void SequenceRetainTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Retaining.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retaining>().To<Command00>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command00Retaining.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retaining>().To<Command00>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command00Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }

        [Test]
        public void RetainReleaseOnceTest()
        {
            var indexes = new List<int>();
            Command00Retaining.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retaining>().To<Command00>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command00Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }

        [Test]
        public void FailTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Fail.OnExecute += () => { count++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Fail>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void FailSequenceTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Fail.OnExecute += () => { count++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Fail>().To<Command00>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
        }

        [Test]
        public void CleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command00Cleanable.OnExecute += () => { count++; };
            Command00Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
        }

        [Test]
        public void MultipleCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command00Cleanable.OnExecute += () => { count++; };
            Command00Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Cleanable>();
            _binder.Bind(CommandTestEvent.Event00).To<Command00Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(4, count);
            Assert.AreEqual(4, countCleaning);
        }

        [Test]
        public void SequenceCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command00Cleanable.OnExecute += () => { count++; };
            Command00Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Cleanable>().To<Command00Cleanable>().To<Command00Cleanable>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(3, count);
            Assert.AreEqual(3, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(6, count);
            Assert.AreEqual(6, countCleaning);
        }
    }
}