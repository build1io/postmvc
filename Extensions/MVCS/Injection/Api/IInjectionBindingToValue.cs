namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToValue
    {
        IInjectionBindingToValueConstruct ConstructValue();
        IInjectionBinding                 ToBinding();
    }
}