using System;

namespace Build1.PostMVC.Utils.Reflection
{
    internal interface IReflectionInfo
    {
        IReflectionInfo Build(Type type);
    }
}