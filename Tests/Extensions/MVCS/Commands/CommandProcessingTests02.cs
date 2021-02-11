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
    public sealed class CommandProcessingTests02
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command02.OnExecute = null;
            Command02Copy.OnExecute = null;

            var binder = new CommandBinder(new InjectionBinder());
            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing(new EventDispatcher(), binder);
        }
        
        [Test]
        public void CommandExecutionTest()
        {
            var executed = false;
            var param01Received = 0;
            var param02Received = string.Empty;
            Command02.OnExecute += (param01, param02) =>
            {
                executed = true;
                param01Received = param01;
                param02Received = param02;
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.IsTrue(executed);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual(" ", param02Received);
        }
        
        [Test]
        public void MultipleBindingsExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
        }
        
        [Test]
        public void MultipleCommandsExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
        }
        
        [Test]
        public void SequenceExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
            };
            
            Command02Copy.OnExecute += (param01, param02) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
        }
        
        [Test]
        public void MultipleCommandsExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02Copy.OnExecute += (param01, param02) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void SequenceExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02Copy.OnExecute += (param01, param02) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void OnceTest()
        {
            var count = 0;
            var param01Last = 0;
            var param02Last = string.Empty;
            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                param01Last = param01;
                param02Last = param02;
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "  ");

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Last);
            Assert.AreEqual(" ", param02Last);
        }
        
        [Test]
        public void OnceMultipleTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, " ");

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void OnceSequenceTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, " ");

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleExecutionTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, " ");

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void MultipleMultipleExecutionTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, " ");

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void MultipleSequenceExecutionTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, " ");

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Retaining.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retaining>().To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void SequenceRetainTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Retaining.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retaining>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");

            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command02Retaining.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Retaining>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command02Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void RetainReleaseOnceTest()
        {
            var indexes = new List<int>();
            Command02Retaining.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Retaining>().To<Command02>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command02Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "  ");
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void FailTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Fail.OnExecute += (param01, param02) => { count++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Fail>().To<Command02>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void FailSequenceTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Fail.OnExecute += (param01, param02) => { count++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Fail>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void CleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command02Cleanable.OnExecute += (param01, param02) => { count++; };
            Command02Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
        }
        
        [Test]
        public void MultipleCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command02Cleanable.OnExecute += (param01, param02) => { count++; };
            Command02Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Cleanable>();
            _binder.Bind(CommandTestEvent.Event02).To<Command02Cleanable>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(4, count);
            Assert.AreEqual(4, countCleaning);
        }
        
        [Test]
        public void SequenceCleaningTest()
        {
            var count = 0;
            var countCleaning = 0;
            Command02Cleanable.OnExecute += (param01, param02) => { count++; };
            Command02Cleanable.OnCleaning += () => { countCleaning++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Cleanable>().To<Command02Cleanable>().To<Command02Cleanable>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(3, count);
            Assert.AreEqual(3, countCleaning);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(6, count);
            Assert.AreEqual(6, countCleaning);
        }

        [Test]
        public void ParametersTest()
        {
            var values = new List<int>();
            Command02.OnExecute += (param01, param02) => { values.Add(param01); }; 
            Command02Copy.OnExecute += (param01, param02) => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
        
        [Test]
        public void ParametersInSequenceTest()
        {
            var values = new List<int>();
            Command02.OnExecute += (param01, param02) => { values.Add(param01); }; 
            Command02Copy.OnExecute += (param01, param02) => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, " ");
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
    }
}