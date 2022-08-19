namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public sealed class InjectionProviderItem01 : IInjectionProviderItem
    {
        public int Param { get; }

        public InjectionProviderItem01()
        {
        }
        
        public InjectionProviderItem01(int param)
        {
            Param = param;
        }
    }
}