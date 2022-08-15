using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public sealed class InjectionProviderWrapper 
    {
        [Inject] public IInjectionProviderItem Item { get; set; }
    }
}