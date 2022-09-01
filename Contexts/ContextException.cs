using System;

namespace Build1.PostMVC.Core.Contexts
{
    public sealed class ContextException : Exception
    {
        public ContextException(ContextExceptionType type) : base(type.ToString())
        {
        }
        
        public ContextException(ContextExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}