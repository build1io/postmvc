using System.Collections.Generic;
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
    public sealed class Command00GroupTests
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
         * Group.
         */

        [Test]
        public void ExecutionTest()
        {
            var count = 0;
            
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void OrderTest()
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
        public void MultipleDispatchesTest()
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
        public void ParallelDispatchesTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>();
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(4, count);
        }

        [Test]
        public void OnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00>().OnComplete(CommandTestEvent.Event00Complete);
            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnFailTest()
        {
            var countFail = 0;
            var countException = 0;

            Command00Fail.OnExecute += () => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void FailFailTest()
        {
            var countFail = 0;
            var countException = 0;

            Command00Fail.OnExecute += () => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Fail>().To<Command00Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
        }
        
        [Test]
        public void ExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCatch = 0;

            Command00.OnExecute += () => { count++; };
            Command00Exception.OnExecute += () => { countException++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Exception>().To<Command00>();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                countCatch++;
            }

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCatch);
        }
        
        [Test]
        public void ExceptionOnFailTest()
        {
            var count = 0;
            var countException = 0;
            var countFail = 0;
            
            Command00.OnExecute += () => { count++; };
            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countFail++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Exception>().To<Command00>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countFail);
        }
        
        [Test]
        public void ExceptionFailFailTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command00Fail.OnExecute += () => { countFail++; };
            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Exception>().To<Command00Fail>().To<Command00Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }

        [Test]
        public void FailExceptionFailTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command00Fail.OnExecute += () => { countFail++; };
            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Fail>().To<Command00Exception>().To<Command00Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }

        [Test]
        public void FailFailExceptionTest()
        {
            var countFail = 0;
            var countException = 0;
            var countExceptionsHandles = 0;

            Command00Fail.OnExecute += () => { countFail++; };
            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Fail>().To<Command00Fail>().To<Command00Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }

        [Test]
        public void SuccessFailSuccessTest()
        {
            var count = 0;
            var countFails = 0;

            Command00.OnExecute += () => { count++; };
            Command00Fail.OnExecute += () => { countFails++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Fail>().To<Command00>().OnFail(CommandTestEvent.EventFail);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countFails);
        }

        [Test]
        public void SuccessExceptionSuccessTest()
        {
            var count = 0;
            var countException = 0;
            var countExceptionsHandled = 0;

            Command00.OnExecute += () => { count++; };
            Command00Exception.OnExecute += () => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Exception>().To<Command00>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
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
            Command00.OnExecute += () => { count++; };
            Command00Copy.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void OnceFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countCopy = 0;
            
            Command00.OnExecute += () => { count++; };
            Command00Fail.OnExecute += () => { countFail++; };
            Command00Copy.OnExecute += () => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Fail>().To<Command00Copy>().OnFail(CommandTestEvent.EventFail).Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countCopy);
        }

        [Test]
        public void OnceExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCopy = 0;
            var countCatch = 0;
            
            Command00.OnExecute += () => { count++; };
            Command00Exception.OnExecute += () => { countException++; };
            Command00Copy.OnExecute += () => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Exception>().To<Command00Copy>().Once();

            Assert.AreEqual(0, count);
            Assert.AreEqual(0, countException);
            Assert.AreEqual(0, countCopy);
            Assert.AreEqual(0, countCatch);
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                countCatch++;
            }
            
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCopy);
            Assert.AreEqual(1, countCatch);
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                countCatch++;
            }
            
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, countException);
            Assert.AreEqual(2, countCopy);
            Assert.AreEqual(2, countCatch);
        }
        
        [Test]
        public void OnceExceptionOnFailTest()
        {
            var count = 0;
            var countException = 0;
            var countCopy = 0;
            
            Command00.OnExecute += () => { count++; };
            Command00Exception.OnExecute += () => { countException++; };
            Command00Copy.OnExecute += () => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Exception>().To<Command00Copy>().OnFail(CommandTestEvent.EventFail).Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnceRetainReleaseTest()
        {
            var indexes = new List<int>();
            Command00Retain.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);

            Command00Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }

        [Test]
        public void OnceRetainFailTest()
        {
            var indexes = new List<int>();
            Command00Retain.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            Command00Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00>().OnFail(CommandTestEvent.EventFail).Once();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);

            Command00Retain.Instance.FailImpl();

            Assert.AreEqual(4, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
            Assert.AreEqual(3, indexes[3]);

            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(4, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
            Assert.AreEqual(3, indexes[3]);
        }
        
        /*
         * Retain / Release.
         */

        [Test]
        public void RetainTest()
        {
            var count = 0;
            Command00.OnExecute += () => { count++; };
            Command00Retain.OnExecute += () => { count++; };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            
            Command00Retain.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            Command00Copy.OnExecute += () => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00>().OnComplete(CommandTestEvent.Event00Complete);
            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            Command00Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailTest()
        {
            var indexes = new List<int>();
            
            Command00Retain.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            Command00Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            Command00Retain.Instance.FailImpl();
            
            Assert.AreEqual(4, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
            Assert.AreEqual(3, indexes[3]);
        }

        [Test]
        public void RetainReleaseInstantTest()
        {
            var indexes = new List<int>();
            
            Command00RetainReleaseInstant.OnExecute += () => { indexes.Add(0); };
            Command00.OnExecute += () => { indexes.Add(1); };
            Command00Copy.OnExecute += () => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00RetainReleaseInstant>().To<Command00>().OnComplete(CommandTestEvent.Event00Complete);
            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantInTheEndTest()
        {
            var indexes = new List<int>();
            
            Command00.OnExecute += () => { indexes.Add(0); };
            Command00RetainReleaseInstant.OnExecute += () => { indexes.Add(1); };
            Command00Copy.OnExecute += () => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00RetainReleaseInstant>().OnComplete(CommandTestEvent.Event00Complete);
            _binder.Bind(CommandTestEvent.Event00Complete).To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailInstantTest()
        {
            var indexes = new List<int>();
            
            Command00.OnExecute += () => { indexes.Add(0); };
            Command00RetainFailInstant.OnExecute += () => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00RetainFailInstant>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainExceptionInstantTest()
        {
            var indexes = new List<int>();
            
            Command00.OnExecute += () => { indexes.Add(0); };
            Command00RetainExceptionInstant.OnExecute += () => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00RetainExceptionInstant>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }

        [Test]
        public void RetainRetainReleaseReleaseTest()
        {
            var count = 0;

            Command00Retain.OnExecute += () => { count++; };
            Command00RetainCopy.OnExecute += () => { count++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>().To<Command00RetainCopy>();
            
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            Assert.AreEqual(2, count);
            
            Command00Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(2, count);
            
            Command00RetainCopy.Instance.ReleaseImpl();
            
            Assert.AreEqual(2, count);
        }
        
        /*
         * Parameters.
         */

        [Test]
        public void ParametersTest()
        {
            var values = new List<int>();
            Command00.OnExecute += () => { values.Add(10); };
            Command00Copy.OnExecute += () => { values.Add(10); };

            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);

            Assert.AreEqual(10, values[0]);
            Assert.AreEqual(10, values[1]);
        }
    }
}