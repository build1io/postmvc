using Build1.PostMVC.Core.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Commands
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