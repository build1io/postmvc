using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection
{
    public sealed class InjectionProviderTests
    {
        private IInjectionBinder InjectionBinder;
        
        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();
        }

        [Test]
        public void Test()
        {
        }

        // Provider construction
        // Provider inner functions
        // Attributes testing
        // Getting instance without attrubute
        
        
    }
}