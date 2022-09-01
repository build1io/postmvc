namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToValue
    {
        IInjectionBindingToValueConstruct ConstructValue();
        IInjectionBindingToBinding        ConstructOnStart();
        IInjectionBinding                 ToBinding();
    }
}