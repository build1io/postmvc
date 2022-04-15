using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands
{
    public sealed class CommandReleasingTests
    {
        private CommandBinder _binder;
        
        [SetUp]
        public void SetUp()
        {
            _binder = new CommandBinder
            {
                InjectionBinder = new InjectionBinder()
            };
        }

        [Test]
        public void ReleasingTest()
        {
            Assert.That(() => _binder.OnCommandFinish(null), Throws.Exception);
        }
    }
}