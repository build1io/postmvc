namespace Build1.PostMVC.Core.Tests.Injection.Parts
{
    public sealed class InjectionProviderWrapperWithAttribute
    {
        [ItemInjection(10)] public IInjectionProviderItem Item { get; set; }
    }
}