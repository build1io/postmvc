using System;

namespace Build1.PostMVC.Core.Utils.Reflection
{
    public interface IReflectionInfo
    {
        IReflectionInfo Build(Type type);
    }
}