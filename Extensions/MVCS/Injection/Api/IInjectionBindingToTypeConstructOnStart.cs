namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToTypeConstructOnStart
    {
        IInjectionBindingToBinding ConstructOnStart();
        IInjectionBinding          ToBinding();
    }
}