using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests
{
    public sealed class Command02SequenceTests
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
         * Sequences.
         */

        [Test]
        public void ExecutionTest()
        {
            var count = 0;
            var param01List = new List<int>();
            var param02List = new List<string>();

            Command02.OnExecute += (param01, param02) =>
            {
                count++;
                param01List.Add(param01);
                param02List.Add(param02);
            };

            Command02Copy.OnExecute += (param01, param02) =>
            {
                count++;
                param01List.Add(param01);
                param02List.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param01List[0], param01List[1]);
            Assert.AreEqual("1", param02List[0], param02List[1]);
        }

        [Test]
        public void OrderTest()
        {
            var indexes = new List<int>();
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02Copy.OnExecute += (param01, param02) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void MultipleDispatchesTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void ParallelDispatchesTest()
        {
            var count = 0;
            Command02.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>().InSequence();
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, "1");

            Assert.AreEqual(4, count);
        }

        [Test]
        public void OnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02>().OnComplete(CommandTestEvent.Event02Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event02Complete).To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countException = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countException);
        }
        
        [Test]
        public void FailFailTest()
        {
            var countFail = 0;
            var countFailHandled = 0;

            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            CommandFailHandler.OnExecute += e => { countFailHandled++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Fail>().To<Command02Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countFailHandled);
        }

        [Test]
        public void ExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCatch = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Exception>().To<Command02>().InSequence();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 0, "1");
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
            
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countFail++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Exception>().To<Command02>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, "1");

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

            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Exception>().To<Command02Fail>().To<Command02Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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

            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Fail>().To<Command02Exception>().To<Command02Fail>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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

            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Fail>().To<Command02Fail>().To<Command02Exception>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(0, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }
        
        [Test]
        public void SuccessFailSuccessTest()
        {
            var count = 0;
            var countFails = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Fail.OnExecute += (param01, param02) => { countFails++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Fail>().To<Command02>().OnFail(CommandTestEvent.EventFail).InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFails);
        }
        
        [Test]
        public void SuccessExceptionSuccessTest()
        {
            var count = 0;
            var countException = 0;
            var countExceptionsHandled = 0;

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Exception>().To<Command02>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void OnceFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countCopy = 0;
            
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Fail.OnExecute += (param01, param02) => { countFail++; };
            Command02Copy.OnExecute += (param01, param02) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Fail>().To<Command02Copy>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

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
            
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            Command02Copy.OnExecute += (param01, param02) => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Exception>().To<Command02Copy>().InSequence().Once();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
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
                _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");
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
            
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Exception.OnExecute += (param01, param02) => { countException++; };
            Command02Copy.OnExecute += (param01, param02) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Exception>().To<Command02Copy>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");
            _dispatcher.Dispatch(CommandTestEvent.Event02, 5, "1");

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(0, countCopy);
        }
        
        [Test]
        public void OnceRetainReleaseTest()
        {
            var indexes = new List<int>();
            Command02Retain.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>().To<Command02>().InSequence().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);

            Command02Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void OnceRetainFailTest()
        {
            var indexes = new List<int>();
            Command02Retain.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };
            Command02Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>().To<Command02>().OnFail(CommandTestEvent.EventFail).InSequence().Once();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, indexes.Count);
            Assert.AreEqual(0, indexes[0]);

            Command02Retain.Instance.FailImpl();

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(2, indexes[1]);
            Assert.AreEqual(3, indexes[2]);

            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Retain.OnExecute += (param01, param02) => { count++; };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, count);
        }

        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            Command02Retain.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>().To<Command02>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes.Count);

            Command02Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }
        
        [Test]
        public void RetainFailTest()
        {
            var indexes = new List<int>();
            
            Command02Retain.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02Retain.OnFail += () => { indexes.Add(1); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>().To<Command02>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(1, indexes.Count);
            Assert.AreEqual(0, indexes[0]);

            Command02Retain.Instance.FailImpl();
            
            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(3, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantTest()
        {
            var indexes = new List<int>();
            
            Command02RetainReleaseInstant.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02.OnExecute += (param01, param02) => { indexes.Add(1); };
            Command02Copy.OnExecute += (param01, param02) => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02RetainReleaseInstant>().To<Command02>().OnComplete(CommandTestEvent.Event02Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event02Complete).To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantInTheEndTest()
        {
            var indexes = new List<int>();
            
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02RetainReleaseInstant.OnExecute += (param01, param02) => { indexes.Add(1); };
            Command02Copy.OnExecute += (param01, param02) => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02RetainReleaseInstant>().OnComplete(CommandTestEvent.Event02Complete).InSequence();
            _binder.Bind(CommandTestEvent.Event02Complete).To<Command02Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailInstantTest()
        {
            var indexes = new List<int>();
            
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02RetainFailInstant.OnExecute += (param01, param02) => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02RetainFailInstant>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainExceptionInstantTest()
        {
            var indexes = new List<int>();
            
            Command02.OnExecute += (param01, param02) => { indexes.Add(0); };
            Command02RetainExceptionInstant.OnExecute += (param01, param02) => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02RetainExceptionInstant>().OnFail(CommandTestEvent.EventFail).InSequence();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

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
            
            Command02Retain.OnExecute += (param01, param02) => { countRetain++; };
            Command02.OnExecute += (param01, param02) => { count++; };
            Command02Copy.OnExecute += (param01, param02) => { countBreak++; };

            _binder.Bind(CommandTestEvent.Event02)
                   .To<Command02Retain>()
                   .To<Command02>()
                   .OnBreak(CommandTestEvent.Event02Complete)
                   .InSequence();

            _binder.Bind(CommandTestEvent.Event02Complete)
                   .To<Command02Copy>();
            
            Assert.AreEqual(0, countRetain);
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, countBreak);
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, string.Empty);

            Assert.AreEqual(1, countRetain);
            Assert.AreEqual(0, count);
            Assert.AreEqual(0, countBreak);

            Command02Retain.Instance.BreakImpl();
            
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
            var values01 = new List<int>();
            var values02 = new List<string>();
            
            Command02.OnExecute += (param01, param02) =>
            {
                values01.Add(param01);
                values02.Add(param02);
            };
            
            Command02Copy.OnExecute += (param01, param02) =>
            {
                values01.Add(param01);
                values02.Add(param02);
            };

            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02Copy>().InSequence();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 10, "1");

            Assert.AreEqual(10, values01[0]);
            Assert.AreEqual(10, values01[1]);
            Assert.AreEqual("1", values02[0]);
            Assert.AreEqual("1", values02[1]);
        }
    }
}