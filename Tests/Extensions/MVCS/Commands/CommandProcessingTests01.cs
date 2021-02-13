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
    public sealed class CommandProcessingTests01
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command01.OnExecute = null;
            Command01Copy.OnExecute = null;

            var binder = new CommandBinder
            {
                InjectionBinder = new InjectionBinder()
            };
            
            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing
            {
                CommandBinder = binder
            };
        }
        
        [Test]
        public void CommandExecutionTest()
        {
            var executed = false;
            var param01Received = 0;
            Command01.OnExecute += param01 =>
            {
                executed = true;
                param01Received = param01;
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.IsTrue(executed);
            Assert.AreEqual(10, param01Received);
        }
        
        [Test]
        public void MultipleBindingsExecutionTest()
        {
            var count = 0;
            var param = new List<int>();
            Command01.OnExecute += param01 =>
            {
                count++;
                param.Add(param01);
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param[0]);
            Assert.AreEqual(10, param[1]);
        }
        
        [Test]
        public void MultipleCommandsExecutionTest()
        {
            var count = 0;
            var param = new List<int>();
            Command01.OnExecute += param01 =>
            {
                count++;
                param.Add(param01);
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param[0]);
            Assert.AreEqual(10, param[1]);
        }
        
        [Test]
        public void SequenceExecutionTest()
        {
            var count = 0;
            var param = new List<int>();
            
            Command01.OnExecute += param01 =>
            {
                count++;
                param.Add(param01);
            };
            
            Command01Copy.OnExecute += param01 =>
            {
                count++;
                param.Add(param01);
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param[0]);
            Assert.AreEqual(10, param[1]);
        }
        
        [Test]
        public void MultipleCommandsExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command01.OnExecute += param01 => { indexes.Add(0); };
            Command01Copy.OnExecute += param01 => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void SequenceExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command01.OnExecute += param01 => { indexes.Add(0); };
            Command01Copy.OnExecute += param01 => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void OnceTest()
        {
            var count = 0;
            var paramLast = 0;
            Command01.OnExecute += param01 =>
            {
                count++;
                paramLast = param01;
            };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, paramLast);
        }
        
        [Test]
        public void OnceMultipleTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void OnceSequenceTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleExecutionTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleMultipleExecutionTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void MultipleSequenceExecutionTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Retaining.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retaining>().To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void SequenceRetainTest()
        {
            var count = 0;
            Command01.OnExecute += param02 => { count++; };
            Command01Retaining.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retaining>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command01Retaining.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Retaining>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command01Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void RetainReleaseOnceTest()
        {
            var indexes = new List<int>();
            Command01Retaining.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Retaining>().To<Command01>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command01Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void FailTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Fail.OnExecute += param01 => { count++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Fail>().To<Command01>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void FailSequenceTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };
            Command01Fail.OnExecute += param01 => { count++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Fail>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void CleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command01Cleanable.OnExecute += param01 => { count++; };
            Command01Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
        }
        
        [Test]
        public void MultipleCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command01Cleanable.OnExecute += param01 => { count++; };
            Command01Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Cleanable>();
            _binder.Bind(CommandTestEvent.Event01).To<Command01Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(4, count);
            Assert.AreEqual(4, countCleaning);
        }
        
        [Test]
        public void SequenceCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command01Cleanable.OnExecute += param01 => { count++; };
            Command01Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Cleanable>().To<Command01Cleanable>().To<Command01Cleanable>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(3, count);
            Assert.AreEqual(3, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(6, count);
            Assert.AreEqual(6, countCleaning);
        }

        [Test]
        public void ParametersTest()
        {
            var values = new List<int>();
            Command01.OnExecute += param01 => { values.Add(param01); }; 
            Command01Copy.OnExecute += param01 => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
        
        [Test]
        public void ParametersInSequenceTest()
        {
            var values = new List<int>();
            Command01.OnExecute += param01 => { values.Add(param01); }; 
            Command01Copy.OnExecute += param01 => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
    }
}