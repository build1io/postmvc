using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands
{
    public class CommandBindingTestsCommon
    {
        protected ICommandBinder _binder;
        
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