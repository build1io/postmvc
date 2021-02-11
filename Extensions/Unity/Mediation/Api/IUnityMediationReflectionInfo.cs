using System;
using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Api
{
    internal interface IUnityMediationReflectionInfo : IReflectionInfo
    {
        IList<MethodInfo> GetMethodsInfos<T>() where T : Attribute;
    }
}