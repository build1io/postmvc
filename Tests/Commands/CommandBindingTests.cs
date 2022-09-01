using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Commands.Common;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands
{
    public sealed class CommandBindingTests
    {
        private ICommandBinder _binder;
        
        [SetUp]
        public void SetUp()
        {
            _binder = new CommandBinder
            {
                InjectionBinder = new InjectionBinder()
            };
        }

        [Test]
        public void GetBindingsForUnregisteredTest()
        {
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
        }

        [Test]
        public void GetBindingsNotNullTest()
        {
            _binder.Bind(CommandTestEvent.Event00);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event01);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event02);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event03);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event03));
        }

        [Test]
        public void GetBindingsCountTest()
        {
            _binder.Bind(CommandTestEvent.Event00);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event00);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event01);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event01);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event02);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event02);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
            
            _binder.Bind(CommandTestEvent.Event03);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event03).Count);
            
            _binder.Bind(CommandTestEvent.Event03);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event03).Count);
        }

        [Test]
        public void GetBindingsCountDecrementTest()
        {
            var binding011 = _binder.Bind(CommandTestEvent.Event00);
            var binding012 = _binder.Bind(CommandTestEvent.Event00);
            
            var binding021 = _binder.Bind(CommandTestEvent.Event01);
            var binding022 = _binder.Bind(CommandTestEvent.Event01);
            
            var binding031 = _binder.Bind(CommandTestEvent.Event02);
            var binding032 = _binder.Bind(CommandTestEvent.Event02);
            
            var binding041 = _binder.Bind(CommandTestEvent.Event03);
            var binding042 = _binder.Bind(CommandTestEvent.Event03);
            
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event03).Count);
            
            _binder.Unbind(binding011);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event03).Count);
            
            _binder.Unbind(binding021);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event03).Count);
            
            _binder.Unbind(binding031);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(2, _binder.GetBindings(CommandTestEvent.Event03).Count);
            
            _binder.Unbind(binding041);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event01).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event02).Count);
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event03).Count);
        }

        [Test]
        public void BindingsRemovalTest()
        {
            var bindings01 = _binder.Bind(CommandTestEvent.Event00);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event00));
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event00).Count);
            _binder.Unbind(bindings01);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event00));
            
            var bindings02 = _binder.Bind(CommandTestEvent.Event01);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event01));
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event01).Count);
            _binder.Unbind(bindings02);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            
            var bindings03 = _binder.Bind(CommandTestEvent.Event02);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event02));
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event02).Count);
            _binder.Unbind(bindings03);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            
            var bindings04 = _binder.Bind(CommandTestEvent.Event03);
            Assert.IsNotNull(_binder.GetBindings(CommandTestEvent.Event03));
            Assert.AreEqual(1, _binder.GetBindings(CommandTestEvent.Event03).Count);
            _binder.Unbind(bindings04);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
        }
        
        [Test]
        public void UnbindingAllTest()
        {
            _binder.Bind(CommandTestEvent.Event00);
            _binder.Bind(CommandTestEvent.Event00);
            _binder.UnbindAll(CommandTestEvent.Event00);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event00));
            
            _binder.Bind(CommandTestEvent.Event01);
            _binder.Bind(CommandTestEvent.Event01);
            _binder.UnbindAll(CommandTestEvent.Event01);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event01));
            
            _binder.Bind(CommandTestEvent.Event02);
            _binder.Bind(CommandTestEvent.Event02);
            _binder.UnbindAll(CommandTestEvent.Event02);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event02));
            
            _binder.Bind(CommandTestEvent.Event03);
            _binder.Bind(CommandTestEvent.Event03);
            _binder.UnbindAll(CommandTestEvent.Event03);
            Assert.IsNull(_binder.GetBindings(CommandTestEvent.Event03));
        }
    }
}