using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests
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
        
        [Test]
        public void DoubleDeinitTest()
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
    }
}