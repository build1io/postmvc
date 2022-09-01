using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests
{
    public sealed class Command01OtherTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;

        [SetUp]
        public void SetUp()
        {
            Command01.OnExecute = null;
            Command01Copy.OnExecute = null;
            Command01Fail.OnExecute = null;
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

            Command01.OnExecute += param01 => { count++; };
            Command01DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command01DoubleDeinit.OnExecute += param01 => { executes++; };
            Command01DoubleDeinit.OnPreDestroy += () => { preDestroys++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01DoubleDeinit>().To<Command01>().To<Command01>().InSequence();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
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

            Command01.OnExecute += param01 => { count++; };
            Command01DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command01DoubleDeinit.OnExecute += param01 => { executes++; };
            Command01DoubleDeinit.OnPreDestroy += () => { preDestroys++; };

            _binder.Bind(CommandTestEvent.Event01).To<Command01>().To<Command01DoubleDeinit>().To<Command01>().To<Command01>();

            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
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
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01DoubleResolve>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
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
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
            
            try
            {
                Command01Retain.Instance.ReleaseImpl();
                Command01Retain.Instance.ReleaseImpl();
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
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01DoubleFail>();
         
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
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
            
            _binder.Bind(CommandTestEvent.Event01).To<Command01Retain>();
            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);
            
            try
            {
                Command01Retain.Instance.FailImpl();
                
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOf<Exception>(exception);
                Assert.IsNotInstanceOf<CommandException>(exception);
                @catch++;
            }

            try
            {
                Command01Retain.Instance.FailImpl();
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