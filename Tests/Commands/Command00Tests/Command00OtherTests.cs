using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests
{
    public sealed class Command00OtherTests
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
         * Double Deinit.
         */
        
        [Test]
        public void DoubleDeinitSequenceTest()
        {
            var count = 0;
            var postConstructs = 0;
            var preDestroys = 0;
            var executes = 0;
            var @catch = 0;

            Command00.OnExecute += () => { count++; };
            Command00DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command00DoubleDeinit.OnExecute += () => { executes++; };
            Command00DoubleDeinit.OnPreDestroy += () => { preDestroys++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00DoubleDeinit>().To<Command00>().To<Command00>().InSequence();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                @catch++;
            }
            
            Assert.AreEqual(0, @catch);
            Assert.AreEqual(3, count);
            Assert.AreEqual(1, postConstructs);
            Assert.AreEqual(1, executes);
            Assert.AreEqual(1, preDestroys);
        }
        
        [Test]
        public void DoubleDeinitGroupTest()
        {
            var count = 0;
            var postConstructs = 0;
            var preDestroys = 0;
            var executes = 0;
            var @catch = 0;

            Command00.OnExecute += () => { count++; };
            Command00DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command00DoubleDeinit.OnExecute += () => { executes++; };
            Command00DoubleDeinit.OnPreDestroy += () => { preDestroys++; };
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00>().To<Command00DoubleDeinit>().To<Command00>().To<Command00>();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch
            {
                @catch++;
            }
            
            Assert.AreEqual(0, @catch);
            Assert.AreEqual(3, count);
            Assert.AreEqual(1, postConstructs);
            Assert.AreEqual(1, executes);
            Assert.AreEqual(1, preDestroys);
        }
        
        /*
         * Double Resolve.
         */

        [Test]
        public void DoubleResolveTest()
        {
            var @catch = 0;
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00DoubleResolve>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<CommandException>(exception);
                @catch++;
            }
            
            Assert.AreEqual(1, @catch);
        }
        
        [Test]
        public void DoubleResolveAfterRetainTest()
        {
            var @catch = 0;
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            try
            {
                Command00Retain.Instance.ReleaseImpl();
                Command00Retain.Instance.ReleaseImpl();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<CommandException>(exception);
                @catch++;
            }
            
            Assert.AreEqual(1, @catch);
        }
        
        [Test]
        public void DoubleFailTest()
        {
            var @catch = 0;
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00DoubleFail>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event00);
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<CommandException>(exception);
                @catch++;
            }
            
            Assert.AreEqual(1, @catch);
        }

        [Test]
        public void DoubleFailAfterRetainTest()
        {
            var @catch = 0;
            
            _binder.Bind(CommandTestEvent.Event00).To<Command00Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event00);
            
            try
            {
                Command00Retain.Instance.FailImpl();
                
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<Exception>(exception);
                Assert.IsNotInstanceOf<CommandException>(exception);
                @catch++;
            }

            try
            {
                Command00Retain.Instance.FailImpl();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<CommandException>(exception);
                @catch++;
            }
            
            Assert.AreEqual(2, @catch);
        }
    }
}