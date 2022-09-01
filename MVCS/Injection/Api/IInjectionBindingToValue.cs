namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    public interface IInjectionBindingToValue
    {
        IInjectionBindingToValueConstruct ConstructValue();
        IInjectionBindingToBinding        ConstructOnStart();
        IInjectionBinding                 ToBinding();
    }
}