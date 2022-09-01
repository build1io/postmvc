using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands
{
    public class CommandBindingTestsCommon
    {
        protected CommandBinder _binder;
        
        [SetUp]
        public void SetUp()
        {
            _binder = new CommandBinder
            {
                InjectionBinder = new InjectionBinder()
            };
        }
        
        [Test]
        public void BindingNullTest()
        {
            Assert.That(() => _binder.Bind(null), Throws.Exception);
        }

        [Test]
        public void UnbindingNullTest()
        {
            Assert.That(() => _binder.Unbind(null), Throws.Exception);
        }

        [Test]
        public void UnbindAllNullTest()
        {
            Assert.That(() => _binder.UnbindAll(null), Throws.Exception);
        }

        [Test]
        public void GetBindingsNullTest()
        {
            Assert.That(() => _binder.GetBindings(null), Throws.Exception);
        }

        [Test]
        public void StopNullTest()
        {
            Assert.That(() => _binder.OnCommandFail(null, null), Throws.Exception);
        }
    }
}