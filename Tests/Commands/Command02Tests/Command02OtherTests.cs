using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Events;
using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests
{
    public sealed class Command02OtherTests
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

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command02DoubleDeinit.OnExecute += (param01, param02) => { executes++; };
            Command02DoubleDeinit.OnPreDestroy += () => { preDestroys++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02DoubleDeinit>().To<Command02>().To<Command02>().InSequence();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 0, null);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
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

            Command02.OnExecute += (param01, param02) => { count++; };
            Command02DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command02DoubleDeinit.OnExecute += (param01, param02) => { executes++; };
            Command02DoubleDeinit.OnPreDestroy += () => { preDestroys++; };
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02>().To<Command02DoubleDeinit>().To<Command02>().To<Command02>();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 0, null);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
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
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02DoubleResolve>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);
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
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);
            
            try
            {
                Command02Retain.Instance.ReleaseImpl();
                Command02Retain.Instance.ReleaseImpl();
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
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02DoubleFail>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);
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
            
            _binder.Bind(CommandTestEvent.Event02).To<Command02Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);
            
            try
            {
                Command02Retain.Instance.FailImpl();
                
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<Exception>(exception);
                Assert.IsNotInstanceOf<CommandException>(exception);
                @catch++;
            }

            try
            {
                Command02Retain.Instance.FailImpl();
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