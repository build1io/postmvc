namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToProviderInstanceBy
    {
        IInjectionBindingToProvider ConstructProvider();
        IInjectionBinding           ToBinding();
    }
}