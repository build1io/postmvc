using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;
using UnityEngine;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests
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
        
        [Test]
        public void DoubleDeinitTest()
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
    }
}