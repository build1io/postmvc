using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
{
    public sealed class InjectionProviderTests
    {
        private IInjectionBinder InjectionBinder;

        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();

            InjectionProvider01.Constructed = 0;
            InjectionProvider01.Destroyed = 0;
            InjectionProvider01.ItemsCreated = 0;
            InjectionProvider01.ItemsTaken = 0;
            InjectionProvider01.ItemsReturned = 0;
        }

        [Test]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();
            var instance = InjectionBinder.Get<IInjectionProviderItem>();

            Assert.NotNull(instance);
            Assert.AreEqual(InjectionProvider01.Constructed, 1);
        }

        [Test]
        public void PreDestroy()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();
            var instance = InjectionBinder.Get<IInjectionProviderItem>();

            InjectionBinder.Unbind<IInjectionProviderItem>();

            Assert.NotNull(instance);
            Assert.AreEqual(InjectionProvider01.Constructed, 1);
            Assert.AreEqual(InjectionProvider01.Destroyed, 1);
        }

        [Test]
        public void ItemInstanceInjection()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();
            var wrapper = InjectionBinder.Construct<InjectionProviderWrapper>(false);
            Assert.NotNull(wrapper.Item);
        }
        
        [Test]
        public void GetInstance()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();
            Assert.NotNull(InjectionBinder.Get<IInjectionProviderItem>());
        }

        [Test]
        public void ItemLifecycle()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();

            var wrapper = InjectionBinder.Construct<InjectionProviderWrapper>(false);
            var instance = wrapper.Item;
            
            Assert.NotNull(instance);
            Assert.AreEqual(InjectionProvider01.ItemsTaken, 1);
            Assert.AreEqual(InjectionProvider01.ItemsCreated, 1);
            Assert.AreEqual(InjectionProvider01.ItemsReturned, 0);

            InjectionBinder.Destroy(wrapper, false);
            
            Assert.AreEqual(InjectionProvider01.ItemsTaken, 1);
            Assert.AreEqual(InjectionProvider01.ItemsCreated, 1);
            Assert.AreEqual(InjectionProvider01.ItemsReturned, 1);
        }

        [Test]
        public void ItemInjectionAttribute()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProviderWithAttribute, ItemInjectionAttribute>();
            
            var wrapper = InjectionBinder.Construct<InjectionProviderWrapperWithAttribute>(false);
            var instance = wrapper.Item;
            
            Assert.NotNull(instance);
            Assert.AreEqual(instance.Param, 10);
        }
    }
}