using System;

namespace Build1.PostMVC.Extensions
{
    public sealed class ExtensionException : Exception
    {
        public ExtensionException(ExtensionExceptionType type) : base(type.ToString())
        {
        }
        
        public ExtensionException(ExtensionExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}