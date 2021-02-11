using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    internal interface IMVCSItemReflectionInfo : IReflectionInfo
    {
        IList<IInjectionInfo> Injections       { get; }
        IList<MethodInfo>     PostConstructors { get; }
        IList<MethodInfo>     PreDestroys      { get; }
    }
}