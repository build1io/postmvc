namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    public interface IInjectionBindingToTypeConstructOnStart
    {
        IInjectionBindingToBinding ConstructOnStart();
        IInjectionBinding          ToBinding();
    }
}