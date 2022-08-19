namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public sealed class InjectionProviderWrapperWithAttribute
    {
        [ItemInjection(10)] public IInjectionProviderItem Item { get; set; }
    }
}