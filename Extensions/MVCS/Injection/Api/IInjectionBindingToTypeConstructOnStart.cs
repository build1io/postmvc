namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToTypeConstructOnStart
    {
        IInjectionBindingToBinding ConstructOnStart();
        IInjectionBinding          ToBinding();
    }
}