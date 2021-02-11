namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionInstanceProvider
    {
        object GetInstance(object parent, Inject attribute);
        void   ReturnInstance(object instance);
    }
}