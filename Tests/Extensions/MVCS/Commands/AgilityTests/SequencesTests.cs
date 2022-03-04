using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.AgilityTests
{
    public sealed class SequencesTests
    {
        private ICommandBinder   _binder;
        private IEventDispatcher _dispatcher;
        
        [SetUp]
        public void SetUp()
        {
            var binder = new CommandBinder();

            _binder = binder;
            _dispatcher = new EventDispatcherWithCommandProcessing(binder);

            binder.InjectionBinder = new InjectionBinder();
            binder.Dispatcher = _dispatcher;
        }
        
        [Test]
        public void Execution01Test()
        {
            var count01 = 0;
            var count00 = 0;
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };
            
            Command00.OnExecute = () =>
            {
                count00++;
            };

            _binder.Bind(CommandTestEvent.Event01)
                   .To1<Command01>()
                   .To0<Command00>()
                   .To1<Command01>()
                   .To0<Command00>()
                   .InSequence();

            _dispatcher.Dispatch(CommandTestEvent.Event01, 0);

            Assert.AreEqual(2, count01);
            Assert.AreEqual(2, count00);
        }
        
        [Test]
        public void Execution02Test()
        {
            var count02 = 0;
            var count01 = 0;
            var count00 = 0;
            
            Command02.OnExecute = (param01, param02) =>
            {
                count02++;
            };
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };

            Command00.OnExecute = () => 
            {
                count00++;
            };
            
            _binder.Bind(CommandTestEvent.Event02)
                   .To2<Command02>()
                   .To1<Command01>()
                   .To0<Command00>()
                   .To2<Command02>()
                   .To0<Command00>()
                   .To1<Command01>()
                   .InSequence();
            
            _dispatcher.Dispatch(CommandTestEvent.Event02, 0, string.Empty);

            Assert.AreEqual(2, count02);
            Assert.AreEqual(2, count01);
            Assert.AreEqual(2, count00);
        }
        
        [Test]
        public void Execution03Test()
        {
            var count03 = 0;
            var count02 = 0;
            var count01 = 0;
            var count00 = 0;
            
            Command03.OnExecute = (param01, param02, param03) =>
            {
                count03++;
            };
            
            Command02.OnExecute = (param01, param02) =>
            {
                count02++;
            };
            
            Command01.OnExecute = (param01) =>
            {
                count01++;
            };

            Command00.OnExecute = () => 
            {
                count00++;
            };
            
            _binder.Bind(CommandTestEvent.Event03)
                   .To3<Command03>()
                   .To2<Command02>()
                   .To1<Command01>()
                   .To0<Command00>()
                   .To3<Command03>()
                   .To1<Command01>()
                   .To2<Command02>()
                   .To0<Command00>()
                   .InSequence();
            
            _dispatcher.Dispatch(CommandTestEvent.Event03, 0, string.Empty, null);

            Assert.AreEqual(2, count03);
            Assert.AreEqual(2, count02);
            Assert.AreEqual(2, count01);
            Assert.AreEqual(2, count00);
        }
    }
}