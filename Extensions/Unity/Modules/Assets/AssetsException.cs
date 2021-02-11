using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public sealed class AssetsException : Exception
    {
        public AssetsException(AssetsExceptionType type) : base(type.ToString())
        {
        }
        
        public AssetsException(AssetsExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}