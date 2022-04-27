using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public sealed class AssetsException : Exception
    {
        public readonly AssetsExceptionType type;
        
        public AssetsException(AssetsExceptionType type) : base(type.ToString())
        {
            this.type = type;
        }
        
        public AssetsException(AssetsExceptionType type, string message) : base($"{type} [{message}]")
        {
            this.type = type;
        }
    }
}