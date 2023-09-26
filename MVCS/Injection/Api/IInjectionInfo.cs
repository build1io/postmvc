using System.Reflection;

namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    public interface IInjectionInfo
    {
        PropertyInfo PropertyInfo { get; }
        Inject       Attribute    { get; }
    }
}