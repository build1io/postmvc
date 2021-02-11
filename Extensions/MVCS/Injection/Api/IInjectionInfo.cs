using System.Reflection;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    internal interface IInjectionInfo
    {
        PropertyInfo PropertyInfo { get; }
        Inject       Attribute    { get; }
    }
}