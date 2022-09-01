using System.Reflection;

namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    internal interface IInjectionInfo
    {
        PropertyInfo PropertyInfo { get; }
        Inject       Attribute    { get; }
    }
}