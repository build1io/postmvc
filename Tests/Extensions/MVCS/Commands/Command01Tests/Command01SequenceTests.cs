using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests
{
    public sealed class Command01SequenceTests
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
         * Sequences.
         */

        [Test]
        public void ExecutionTest()
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
            Assert.AreEqual(10, param[0], param[1]);
        }

        [Test]
        public void OrderTest()
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
        public void MultipleDispatchesTest()
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
        public void ParallelDispatchesTest()
        {
            var count = 0;
            Command01.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01>().InSequence();
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(4, count);
        }

        [Test]
        public void OnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Copy.OnExecute += param01 => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01>().OnComplete(CommandTestEvent.Event01Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event01Complete).To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countException = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Fail.OnExecute += param01 => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countException);
        }
        
        [Test]
        public void FailFailTest()
        {
            var countFail = 0;
            var countFailHandled = 0;

            Command01Fail.OnExecute += param01 => { countFail++; };
            CommandFailHandler.OnExecute += e => { countFailHandled++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Fail>().To<Command01Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countFailHandled);
        }

        [Test]
        public void ExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCatch = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Exception.OnExecute += param01 => { countException++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Exception>().To<Command01>().InSequence();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
            }
            catch
            {
                countCatch++;
            }

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCatch);
        }
        
        [Test]
        public void ExceptionOnFailTest()
        {
            var count = 0;
            var countException = 0;
            var countFail = 0;
            
            Command01.OnExecute += param01 => { count++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countFail++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Exception>().To<Command01>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countFail);
        }
        
        [Test]
        public void ExceptionFailFailTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command01Fail.OnExecute += param01 => { countFail++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Exception>().To<Command01Fail>().To<Command01Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, countFail);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }
        
        [Test]
        public void FailExceptionFailTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command01Fail.OnExecute += param01 => { countFail++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Fail>().To<Command01Exception>().To<Command01Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(0, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }
        
        [Test]
        public void FailFailExceptionTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command01Fail.OnExecute += param01 => { countFail++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Fail>().To<Command01Fail>().To<Command01Exception>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(0, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }
        
        [Test]
        public void SuccessFailSuccessTest()
        {
            var count = 0;
            var countFails = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Fail.OnExecute += param01 => { countFails++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Fail>().To<Command01>().OnFail(CommandTestEvent.EventFail).InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFails);
        }
        
        [Test]
        public void SuccessExceptionSuccessTest()
        {
            var count = 0;
            var countException = 0;
            var countExceptionsHandled = 0;

            Command01.OnExecute += param01 => { count++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Exception>().To<Command01>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandled);
        }

        /*
         * Once.
         */
        
        [Test]
        public void OnceTest()
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
        public void OnceFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countCopy = 0;
            
            Command01.OnExecute += param01 => { count++; };
            Command01Fail.OnExecute += param01 => { countFail++; };
            Command01Copy.OnExecute += param01 => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Fail>().To<Command01Copy>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(0, countCopy);
        }
        
        [Test]
        public void OnceExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCopy = 0;
            var countCatch = 0;
            
            Command01.OnExecute += param01 => { count++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            Command01Copy.OnExecute += param01 => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Exception>().To<Command01Copy>().InSequence().Once();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            }
            catch
            {
                countCatch++;
            }
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(0, countCopy);
            Assert.AreEqual(1, countCatch);
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 5);
            }
            catch
            {
                countCatch++;
            }
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countException);
            Assert.AreEqual(0, countCopy);
            Assert.AreEqual(2, countCatch);
        }
        
        [Test]
        public void OnceExceptionOnFailTest()
        {
            var count = 0;
            var countException = 0;
            var countCopy = 0;
            
            Command01.OnExecute += param01 => { count++; };
            Command01Exception.OnExecute += param01 => { countException++; };
            Command01Copy.OnExecute += param01 => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01Exception>().To<Command01Copy>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);
            _dispatcher.Dispatch(CommandTestEvent.Event01, 5);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(0, countCopy);
        }
        
        [Test]
        public void OnceRetainReleaseTest()
        {
            var indexes = new List<int>();
            Command01Retain.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>().To<Command01>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);

            Command01Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void OnceRetainFailTest()
        {
            var indexes = new List<int>();
            Command01Retain.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };
            Command01Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += param01 => { indexes.Add(3); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>().To<Command01>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, indexes.Count);
            Assert.AreEqual(0, indexes[0]);

            Command01Retain.Instance.FailImpl();

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(2, indexes[1]);
            Assert.AreEqual(3, indexes[2]);

            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(2, indexes[1]);
            Assert.AreEqual(3, indexes[2]);
        }
        
        /*
         * Retain / Release.
         */
        
        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command01.OnExecute += param02 => { count++; };
            Command01Retain.OnExecute += param01 => { count++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command01Retain.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>().To<Command01>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);

            Command01Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void RetainFailTest()
        {
            var indexes = new List<int>();
            
            Command01Retain.OnExecute += param01 => { indexes.Add(0); };
            Command01Retain.OnFail += () => { indexes.Add(1); };
            Command01.OnExecute += param01 => { indexes.Add(2); };
            CommandFailHandler.OnExecute += param01 => { indexes.Add(3); };
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>().To<Command01>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, indexes.Count);
            Assert.AreEqual(0, indexes[0]);

            Command01Retain.Instance.FailImpl();
            
            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(3, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantTest()
        {
            var indexes = new List<int>();
            
            Command01RetainReleaseInstant.OnExecute += param01 => { indexes.Add(0); };
            Command01.OnExecute += param01 => { indexes.Add(1); };
            Command01Copy.OnExecute += param01 => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01RetainReleaseInstant>().To<Command01>().OnComplete(CommandTestEvent.Event01Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event01Complete).To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantInTheEndTest()
        {
            var indexes = new List<int>();
            
            Command01.OnExecute += param01 => { indexes.Add(0); };
            Command01RetainReleaseInstant.OnExecute += param01 => { indexes.Add(1); };
            Command01Copy.OnExecute += param01 => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01RetainReleaseInstant>().OnComplete(CommandTestEvent.Event01Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event01Complete).To<Command01Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailInstantTest()
        {
            var indexes = new List<int>();
            
            Command01.OnExecute += param01 => { indexes.Add(0); };
            Command01RetainFailInstant.OnExecute += param01 => { indexes.Add(1); };
            CommandFailHandler.OnExecute += param01 => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01RetainFailInstant>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainExceptionInstantTest()
        {
            var indexes = new List<int>();
            
            Command01.OnExecute += param01 => { indexes.Add(0); };
            Command01RetainExceptionInstant.OnExecute += param01 => { indexes.Add(1); };
            CommandFailHandler.OnExecute += param01 => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01RetainExceptionInstant>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        /*
         * Break.
         */
         
        [Test]
        public void RetainBreakTest()
        {
            var countRetain = 0;
            var count = 0;
            var countBreak = 0;
            
            Command01Retain.OnExecute += (param01) => { countRetain++; };
            Command01.OnExecute += (param01) => { count++; };
            Command01Copy.OnExecute += (param01) => { countBreak++; };

            _binder.Bind(CommandTestEvent.Event01)
                   .To<Command01Retain>()
                   .To<Command01>()
                   .OnBreak(CommandTestEvent.Event01Complete)
                   .InSequence();

            _binder.Bind(CommandTestEvent.Event01Complete)
                   .To<Command01Copy>();
            
            Assert.AreEqual(0, countRetain);
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, countBreak);
            
            _dispatcher.Dispatch(CommandTestEvent.Event01, 10);

            Assert.AreEqual(1, countRetain);
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, countBreak);

            Command01Retain.Instance.BreakImpl();
            
            Assert.AreEqual(1, countRetain);
            Assert.AreEqual(0, count);
            Assert.AreEqual(1, countBreak);
        }
        
        /*
         * Parameters.
         */
        
        [Test]
        public void ParametersTest()
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