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
    public sealed class CommandProcessingTests03
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command03.OnExecute = null;
            Command03Copy.OnExecute = null;
            Command03Fail.OnExecute = null;
            CommandException.OnExecute = null;

            var binder = new CommandBinder();
            
            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing(binder);
            
            binder.InjectionBinder = new InjectionBinder();
            binder.Dispatcher = _dispatcher;
        }
        
        /*
         * Single.
         */
        
        [Test]
        public void SingleCommandExecutionTest()
        {
            var executed = false;
            var param01Received = 0;
            var param02Received = string.Empty;
            var param03Received = new CommandData(1);
            Command03.OnExecute += (param01, param02, param03) =>
            {
                executed = true;
                param01Received = param01;
                param02Received = param02;
                param03Received = param03;
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", new CommandData(2));

            Assert.IsTrue(executed);
            Assert.AreEqual(10, param01Received);
            Assert.AreEqual(" ", param02Received);
            Assert.AreEqual(2, param03Received.id);
        }

        [Test]
        public void SingleCommandsMultipleDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, " ", null);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void SingleCommandOnCompleteEventTest()
        {
            var count = 0;
            var countCopy = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, string.Empty, null);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, countCopy);
        }
        
        [Test]
        public void SingleCommandOnFailEventTest()
        {
            var count = 0;
            var countException = 0;
            
            Command03Fail.OnExecute += (param01, param02, param03) => { count++; };
            CommandException.OnExecute += e => { countException++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandException>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, string.Empty, null);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(0, countException);
        }
        
        /*
         * Multiple Bindings.
         */
        
        [Test]
        public void MultipleBindingsExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            var params03 = new List<CommandData>();
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
                params03.Add(param03);
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _binder.Bind(CommandTestEvent.Event03).To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", new CommandData(1));

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
            Assert.AreEqual(1, params03[0].id);
            Assert.AreEqual(1, params03[1].id);
        }
        
        /*
         * Multiple Commands.
         */
        
        [Test]
        public void MultipleCommandsExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            var params03 = new List<CommandData>();
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
                params03.Add(param03);
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", new CommandData(1));

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
            Assert.AreEqual(1, params03[0].id);
            Assert.AreEqual(1, params03[1].id);
        }
        
        [Test]
        public void MultipleCommandsMultipleDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, " ", null);

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void MultipleOnCompleteEventTest()
        {
            var count = 0;
            var countCopy = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, string.Empty, null);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(0, countCopy);
        }
        
        [Test]
        public void MultipleOnFailEventTest()
        {
            var count = 0;
            var countFail = 0;
            var countException = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            CommandException.OnExecute += e => { countException++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandException>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, string.Empty, null);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(0, countException);
        }
        
        [Test]
        public void MultipleFailingOnFailEventTest()
        {
            var countFail = 0;
            var countException = 0;
            
            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            CommandException.OnExecute += e => { countException++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandException>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, string.Empty, null);
            
            Assert.AreEqual(2, countFail);
            Assert.AreEqual(0, countException);
        }
        
        /*
         * Sequences.
         */
        
        [Test]
        public void SequenceExecutionTest()
        {
            var count = 0;
            var params01 = new List<int>();
            var params02 = new List<string>();
            var params03 = new List<CommandData>();
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
                params03.Add(param03);
            };
            
            Command03Copy.OnExecute += (param01, param02, param03) =>
            {
                count++;
                params01.Add(param01);
                params02.Add(param02);
                params03.Add(param03);
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", new CommandData(1));

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, params01[0]);
            Assert.AreEqual(10, params01[1]);
            Assert.AreEqual(" ", params02[0]);
            Assert.AreEqual(" ", params02[1]);
            Assert.AreEqual(1, params03[0].id);
            Assert.AreEqual(1, params03[1].id);
        }
        
        /*
         * Execution Order.
         */
        
        [Test]
        public void MultipleCommandsExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void SequenceExecutionOrderTest()
        {
            var indexes = new List<int>();
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void SequenceMultipleDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, " ", null);

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void SequenceOnCompleteEventTest()
        {
            var count = 0;
            var countCopy = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>().OnComplete(CommandTestEvent.Event03Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, string.Empty, null);
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void SequenceOnFailEventTest()
        {
            var count = 0;
            var countFail = 0;
            var countException = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            CommandException.OnExecute += e => { countException++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandException>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, string.Empty, null);
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countException);
        }
        
        /*
         * Once.
         */
        
        [Test]
        public void OnceTest()
        {
            var count = 0;
            var param01Last = 0;
            var param02Last = string.Empty;
            var param03Last = new CommandData(1);
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                param01Last = param01;
                param02Last = param02;
                param03Last = param03;
            };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "  ", new CommandData(2));

            Assert.AreEqual(1, count);
            Assert.AreEqual(10, param01Last);
            Assert.AreEqual(" ", param02Last);
            Assert.AreEqual(null, param03Last);
        }
        
        [Test]
        public void OnceMultipleTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, " ", null);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void OnceSequenceTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, " ", null);

            Assert.AreEqual(2, count);
        }
        
        /*
         * Retain.
         */
        
        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Retaining.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retaining>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void SequenceRetainTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Retaining.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retaining>().To<Command03>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);

            Assert.AreEqual(1, count);
        }
        
        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command03Retaining.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Retaining>().To<Command03>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command03Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void RetainReleaseOnceTest()
        {
            var indexes = new List<int>();
            Command03Retaining.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Retaining>().To<Command03>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);
            
            Command03Retaining.Instance.ReleaseImpl();
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "  ", null);
            
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        /*
         * Fail.
         */
        
        [Test]
        public void FailTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { count++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(3, count);
        }
        
        [Test]
        public void FailSequenceTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { count++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().To<Command03>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(2, count);
        }

        /*
         * Parameters.
         */
        
        [Test]
        public void ParametersTest()
        {
            var values = new List<int>();
            Command03.OnExecute += (param01, param02, param03) => { values.Add(param01); }; 
            Command03Copy.OnExecute += (param01, param02, param03) => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
        
        [Test]
        public void ParametersInSequenceTest()
        {
            var values = new List<int>();
            Command03.OnExecute += (param01, param02, param03) => { values.Add(param01); }; 
            Command03Copy.OnExecute += (param01, param02, param03) => { values.Add(param01); };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, " ", null);
            
            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
    }
}