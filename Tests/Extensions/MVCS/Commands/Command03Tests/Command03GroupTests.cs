using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests
{
    public sealed class Command03GroupTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command03.OnExecute = null;
            Command03Copy.OnExecute = null;
            Command03DoubleDeinit.OnExecute = null;
            Command03DoubleDeinit.OnPostConstruct = null;
            Command03DoubleDeinit.OnPreDestroy = null;
            Command03Exception.OnExecute = null;
            Command03Fail.OnExecute = null;
            Command03Retain.OnExecute = null;
            Command03Retain.OnFail = null;
            Command03RetainExceptionInstant.OnExecute = null;
            Command03RetainFailInstant.OnExecute = null;
            Command03RetainReleaseInstant.OnExecute = null;
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
            var param01List = new List<int>();
            var param02List = new List<string>();
            var param03List = new List<CommandData>();
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                count++;
                param01List.Add(param01);
                param02List.Add(param02);
                param03List.Add(param03);
            };

            var data = new CommandData(1);
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", data);

            Assert.AreEqual(2, count);
            Assert.AreEqual(10, param01List[0]);
            Assert.AreEqual(10, param01List[1]);
            Assert.AreEqual("1", param02List[0]);
            Assert.AreEqual("1", param02List[1]);
            Assert.AreEqual(data, param03List[0]);
            Assert.AreEqual(data, param03List[1]);
        }

        [Test]
        public void OrderTest()
        {
            var indexes = new List<int>();
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
        }
        
        [Test]
        public void MultipleDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(4, count);
        }
        
        [Test]
        public void ParallelDispatchesTest()
        {
            var count = 0;
            Command03.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>();
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, "1", null);

            Assert.AreEqual(4, count);
        }

        [Test]
        public void OnCompleteTest()
        {
            var count = 0;
            var countCopy = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnFailTest()
        {
            var countFail = 0;
            var countException = 0;

            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(1, countFail);
            Assert.AreEqual(1, countException);
        }

        [Test]
        public void FailFailTest()
        {
            var countFail = 0;
            var countException = 0;

            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            CommandFailHandler.OnExecute += e => { countException++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
        }
        
        [Test]
        public void ExceptionTest()
        {
            var count = 0;
            var countException = 0;
            var countCatch = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Exception>().To<Command03>();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event03, 0, "1", null);
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
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countFail++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Exception>().To<Command03>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, "1", null);

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

            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Exception>().To<Command03Fail>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

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

            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().To<Command03Exception>().To<Command03Fail>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

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

            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandles++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Fail>().To<Command03Fail>().To<Command03Exception>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, countFail);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countExceptionsHandles);
        }

        [Test]
        public void SuccessFailSuccessTest()
        {
            var count = 0;
            var countFails = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { countFails++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().To<Command03>().OnFail(CommandTestEvent.EventFail);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, count);
            Assert.AreEqual(1, countFails);
        }

        [Test]
        public void SuccessExceptionSuccessTest()
        {
            var count = 0;
            var countException = 0;
            var countExceptionsHandled = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            CommandFailHandler.OnExecute += e => { countExceptionsHandled++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Exception>().To<Command03>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

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
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(2, count);
        }

        [Test]
        public void OnceFailTest()
        {
            var count = 0;
            var countFail = 0;
            var countCopy = 0;
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Fail.OnExecute += (param01, param02, param03) => { countFail++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Fail>().To<Command03Copy>().OnFail(CommandTestEvent.EventFail).Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

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
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Exception>().To<Command03Copy>().Once();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            }
            catch
            {
                countCatch++;
            }
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);
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
            
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Exception.OnExecute += (param01, param02, param03) => { countException++; };
            Command03Copy.OnExecute += (param01, param02, param03) => { countCopy++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Exception>().To<Command03Copy>().OnFail(CommandTestEvent.EventFail).Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);
            _dispatcher.Dispatch(CommandTestEvent.Event03, 5, "1", null);

            Assert.AreEqual(1, count);
            Assert.AreEqual(1, countException);
            Assert.AreEqual(1, countCopy);
        }
        
        [Test]
        public void OnceRetainReleaseTest()
        {
            var indexes = new List<int>();
            Command03Retain.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03>().Once();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);

            Command03Retain.Instance.ReleaseImpl();

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);
        }

        [Test]
        public void OnceRetainFailTest()
        {
            var indexes = new List<int>();
            Command03Retain.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            Command03Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03>().OnFail(CommandTestEvent.EventFail).Once();
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);

            Command03Retain.Instance.FailImpl();

            Assert.AreEqual(4, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
            Assert.AreEqual(3, indexes[3]);

            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

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
            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03Retain.OnExecute += (param01, param02, param03) => { count++; };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(2, count);
        }
        
        [Test]
        public void RetainReleaseTest()
        {
            var indexes = new List<int>();
            
            Command03Retain.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            Command03Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailTest()
        {
            var indexes = new List<int>();
            
            Command03Retain.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            Command03Retain.OnFail += () => { indexes.Add(2); };
            CommandFailHandler.OnExecute += e => { indexes.Add(3); };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes.Count);

            Command03Retain.Instance.FailImpl();
            
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
            
            Command03RetainReleaseInstant.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03RetainReleaseInstant>().To<Command03>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainReleaseInstantInTheEndTest()
        {
            var indexes = new List<int>();
            
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03RetainReleaseInstant.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            Command03Copy.OnExecute += (param01, param02, param03) => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03RetainReleaseInstant>().OnComplete(CommandTestEvent.Event03Complete);
            _binder.Bind(CommandTestEvent.Event03Complete).To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainFailInstantTest()
        {
            var indexes = new List<int>();
            
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03RetainFailInstant.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03RetainFailInstant>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainExceptionInstantTest()
        {
            var indexes = new List<int>();
            
            Command03.OnExecute += (param01, param02, param03) => { indexes.Add(0); };
            Command03RetainExceptionInstant.OnExecute += (param01, param02, param03) => { indexes.Add(1); };
            CommandFailHandler.OnExecute += e => { indexes.Add(2); };

            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03RetainExceptionInstant>().OnFail(CommandTestEvent.EventFail);
            _binder.Bind(CommandTestEvent.EventFail).To<CommandFailHandler>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", null);

            Assert.AreEqual(3, indexes.Count);
            Assert.AreEqual(0, indexes[0]);
            Assert.AreEqual(1, indexes[1]);
            Assert.AreEqual(2, indexes[2]);
        }
        
        [Test]
        public void RetainRetainReleaseReleaseTest()
        {
            var count = 0;

            Command03Retain.OnExecute += (param01, param02, param03) => { count++; };
            Command03RetainCopy.OnExecute += (param01, param02, param03) => { count++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03Retain>().To<Command03RetainCopy>();
            
            Assert.AreEqual(0, count);
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, string.Empty, null);
            
            Assert.AreEqual(2, count);
            
            Command03Retain.Instance.ReleaseImpl();
            
            Assert.AreEqual(2, count);
            
            Command03RetainCopy.Instance.ReleaseImpl();
            
            Assert.AreEqual(2, count);
        }
        
        /*
         * Parameters.
         */

        [Test]
        public void ParametersTest()
        {
            var values01 = new List<int>();
            var values02 = new List<string>();
            var values03 = new List<CommandData>();
            
            Command03.OnExecute += (param01, param02, param03) =>
            {
                values01.Add(param01);
                values02.Add(param02);
                values03.Add(param03);
            };
            
            Command03Copy.OnExecute += (param01, param02, param03) =>
            {
                values01.Add(param01);
                values02.Add(param02);
                values03.Add(param03);
            };

            var data = new CommandData(1);
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03Copy>();
            _dispatcher.Dispatch(CommandTestEvent.Event03, 10, "1", data);

            Assert.AreEqual(10, values01[0]);
            Assert.AreEqual(10, values01[1]);
            Assert.AreEqual("1", values02[0]);
            Assert.AreEqual("1", values02[1]);
            Assert.AreEqual(data, values03[0]);
            Assert.AreEqual(data, values03[1]);
        }
    }
}