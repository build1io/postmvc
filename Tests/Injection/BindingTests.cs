using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
{
    public sealed class BindingTests
    {
        private IInjectionBinder InjectionBinder;

        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();
        }

        /*
         * Binding.
         */

        [Test]
        public void BindingGeneric()
        {
            var binding = InjectionBinder.Bind<Controller>().ToBinding();
            Assert.IsNotNull(binding);
            Assert.AreEqual(binding, InjectionBinder.GetBinding<Controller>());
        }

        [Test]
        public void BindingFinalType()
        {
            var binding = InjectionBinder.Bind(typeof(Controller)).ToBinding();
            Assert.IsNotNull(binding);
            Assert.AreEqual(binding, InjectionBinder.GetBinding(typeof(Controller)));
        }

        [Test]
        public void BindingByInterface()
        {
            var binding = InjectionBinder.Bind<IController, Controller>().ToBinding();
            Assert.IsNotNull(binding);
            Assert.IsNull(InjectionBinder.GetBinding<Controller>());
            Assert.AreEqual(binding, InjectionBinder.GetBinding<IController>());
        }

        [Test]
        public void BindingValueByInterface()
        {
            var controller = new Controller();
            var binding = InjectionBinder.Bind<IController>(controller).ToBinding();
            Assert.IsNotNull(binding);
            Assert.IsNull(InjectionBinder.GetBinding<Controller>());
            Assert.AreEqual(binding, InjectionBinder.GetBinding<IController>());
            Assert.AreEqual(controller, InjectionBinder.GetInstance<IController>());
        }
        
        [Test]
        public void BindingValueByTypeInstance()
        {
            var controller = new Controller();
            var binding = InjectionBinder.Bind(typeof(IController), controller).ToBinding();
            Assert.IsNotNull(binding);
            Assert.IsNull(InjectionBinder.GetBinding<Controller>());
            Assert.AreEqual(binding, InjectionBinder.GetBinding<IController>());
            Assert.AreEqual(controller, InjectionBinder.GetInstance<IController>());
        }

        [Test]
        public void BindingProvider()
        {
            var binding = InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>().ToBinding();
            Assert.IsNotNull(binding);
            Assert.AreEqual(binding, InjectionBinder.GetBinding<IInjectionProviderItem>());
        }

        [Test]
        public void BindingBinding()
        {
            var binding = InjectionBinder.Bind<Controller>().ToBinding();
            Assert.IsNotNull(binding);
            
            InjectionBinder.Unbind<Controller>();
            Assert.IsNull(InjectionBinder.GetBinding<Controller>());
            
            InjectionBinder.Bind(binding);
            Assert.AreEqual(binding, InjectionBinder.GetBinding<Controller>());
        }

        [Test]
        public void BindingException()
        {
            InjectionBinder.Bind<Controller>();
            Assert.That(() => InjectionBinder.Bind<Controller>(), Throws.TypeOf<BindingException>());
        }
        
        [Test]
        public void BindingExceptionMixed()
        {
            InjectionBinder.Bind<Controller>();
            Assert.That(() => InjectionBinder.Bind(typeof(Controller)), Throws.TypeOf<BindingException>());
        }

        /*
         * Rebinding.
         */

        [Test]
        public void RebindingType()
        {
            var binding01 = InjectionBinder.Bind<Controller>().ToBinding();
            var binding02 = InjectionBinder.Rebind<Controller>().ToBinding();
            Assert.AreNotEqual(binding01, binding02);
            Assert.AreEqual(binding02, InjectionBinder.GetBinding<Controller>());
        }
        
        [Test]
        public void RebindingGeneric()
        {
            var binding01 = InjectionBinder.Bind<IController, Controller>().ToBinding();
            var binding02 = InjectionBinder.Rebind<IController, Controller>().ToBinding();
            Assert.AreNotEqual(binding01, binding02);
            Assert.AreEqual(binding02, InjectionBinder.GetBinding<IController>());
        }

        [Test]
        public void RebindingTypeOf()
        {
            var binding01 = InjectionBinder.Bind(typeof(Controller)).ToBinding();
            var binding02 = InjectionBinder.Rebind(typeof(Controller)).ToBinding();
            Assert.AreNotEqual(binding01, binding02);
            Assert.AreEqual(binding02, InjectionBinder.GetBinding<Controller>());
        }

        [Test]
        public void RebindingMixed()
        {
            var binding01 = InjectionBinder.Bind<Controller>().ToBinding();
            var binding02 = InjectionBinder.Rebind(typeof(Controller)).ToBinding();
            Assert.AreNotEqual(binding01, binding02);
            Assert.AreEqual(binding02, InjectionBinder.GetBinding<Controller>());
        }

        [Test]
        public void RebindingValue01()
        {
            var controller01 = new Controller();
            var controller02 = new Controller();
            
            InjectionBinder.Bind<IController>(controller01);
            Assert.AreEqual(controller01, InjectionBinder.GetInstance<IController>());
            Assert.AreNotEqual(controller02, InjectionBinder.GetInstance<IController>());
            
            InjectionBinder.Rebind<IController>(controller02);
            Assert.AreEqual(controller02, InjectionBinder.GetInstance<IController>());
            Assert.AreNotEqual(controller01, InjectionBinder.GetInstance<IController>());
        }
        
        [Test]
        public void RebindingValue02()
        {
            var controller01 = new Controller();
            var controller02 = new Controller();
            
            InjectionBinder.Bind(typeof(IController), controller01);
            Assert.AreEqual(controller01, InjectionBinder.GetInstance<IController>());
            Assert.AreNotEqual(controller02, InjectionBinder.GetInstance<IController>());
            
            InjectionBinder.Rebind(typeof(IController), controller02);
            Assert.AreEqual(controller02, InjectionBinder.GetInstance<IController>());
            Assert.AreNotEqual(controller01, InjectionBinder.GetInstance<IController>());
        }

        [Test]
        public void RebindingProvider()
        {
            InjectionBinder.Bind<IInjectionProviderItem, InjectionProvider01, Inject>();
            InjectionBinder.Bind<InjectionProviderWrapper>().AsFactory();

            var wrapper01 = InjectionBinder.GetInstance<InjectionProviderWrapper>();

            Assert.IsTrue(wrapper01.Item is InjectionProviderItem01);
            Assert.IsFalse(wrapper01.Item is InjectionProviderItem02);
            
            InjectionBinder.Rebind<IInjectionProviderItem, InjectionProvider02, Inject>();
            var wrapper02 = InjectionBinder.GetInstance<InjectionProviderWrapper>();
            
            Assert.IsTrue(wrapper02.Item is InjectionProviderItem02);
            Assert.IsFalse(wrapper02.Item is InjectionProviderItem01);
        }

        /*
         * Unbinding.
         */

        [Test]
        public void UnbindingByType()
        {
            InjectionBinder.Bind<IController, Controller>();
            Assert.NotNull(InjectionBinder.GetBinding<IController>());

            InjectionBinder.Unbind<IController>();
            Assert.IsNull(InjectionBinder.GetBinding<IController>());
        }
        
        [Test]
        public void UnbindingByTypeValue()
        {
            InjectionBinder.Bind<IController, Controller>();
            Assert.NotNull(InjectionBinder.GetBinding<IController>());

            InjectionBinder.Unbind(typeof(IController));
            Assert.IsNull(InjectionBinder.GetBinding<IController>());
        }

        [Test]
        public void UnbindByBinding()
        {
            var binding = InjectionBinder.Bind<IController, Controller>().ToBinding();
            Assert.NotNull(InjectionBinder.GetBinding<IController>());            
            
            InjectionBinder.Unbind(binding);
            Assert.IsNull(InjectionBinder.GetBinding<IController>());
        }

        /*
         * Other.
         */

        [Test]
        public void Iteration()
        {
            var count = 0;
            var binding = InjectionBinder.Bind<IController, Controller>().ToBinding();
            InjectionBinder.ForEachBinding(b =>
            {
                count++;
                Assert.AreEqual(b, binding);
            });
            Assert.AreEqual(count, 1);
        }
    }
}