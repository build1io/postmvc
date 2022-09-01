using System;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PoolableAttribute : Attribute
    {
    }
}