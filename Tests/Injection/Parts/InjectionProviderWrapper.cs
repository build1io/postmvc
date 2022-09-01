using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Injection.Parts
{
    public sealed class InjectionProviderWrapper 
    {
        [Inject] public IInjectionProviderItem Item { get; set; }
    }
}