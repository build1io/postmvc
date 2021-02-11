using System.Reflection;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal sealed class InjectionInfo : IInjectionInfo
    {
        public PropertyInfo PropertyInfo { get; }
        public Inject       Attribute    { get; }

        public InjectionInfo(PropertyInfo propertyInfo, Inject attribute)
        {
            PropertyInfo = propertyInfo;
            Attribute = attribute;
        }

        public override string ToString()
        {
            return PropertyInfo.PropertyType.FullName;
        }
    }
}