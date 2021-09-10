using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests
{
    public sealed class Command03OtherTests
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
        
        [Test]
        public void DoubleDeinitTest()
        {
            var count = 0;
            var postConstructs = 0;
            var preDestroys = 0;
            var executes = 0;
            var @catch = 0;

            Command03.OnExecute += (param01, param02, param03) => { count++; };
            Command03DoubleDeinit.OnPostConstruct += () => { postConstructs++; };
            Command03DoubleDeinit.OnExecute += (param01, param02, param03) => { executes++; };
            Command03DoubleDeinit.OnPreDestroy += () => { preDestroys++; };
            
            _binder.Bind(CommandTestEvent.Event03).To<Command03>().To<Command03DoubleDeinit>().To<Command03>().To<Command03>().InSequence();
            
            try
            {
                _dispatcher.Dispatch(CommandTestEvent.Event03, 0, null, null);
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