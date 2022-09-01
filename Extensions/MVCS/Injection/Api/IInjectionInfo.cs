using System.Reflection;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Api
{
    internal interface IInjectionInfo
    {
        PropertyInfo PropertyInfo { get; }
        Inject       Attribute    { get; }
    }
}