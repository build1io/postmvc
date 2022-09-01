using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Core.Utils.Reflection;

namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    internal interface IMVCSItemReflectionInfo : IReflectionInfo
    {
        IList<IInjectionInfo> Injections       { get; }
        IList<MethodInfo>     PostConstructors { get; }
        IList<MethodInfo>     PreDestroys      { get; }
    }
}