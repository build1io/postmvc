using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public sealed class InjectionProviderWithAttribute : InjectionProvider<IInjectionProviderItem, ItemInjectionAttribute>
    {
        public override IInjectionProviderItem TakeInstance(object parent, ItemInjectionAttribute attribute)
        {
            return new InjectionProviderItem01(attribute.param);
        }

        public override void ReturnInstance(IInjectionProviderItem instance)
        {
        }
    }
}