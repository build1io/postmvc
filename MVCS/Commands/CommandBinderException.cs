using System;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class CommandBinderException : Exception
    {
        public CommandBinderException(CommandBinderExceptionType type) : base(type.ToString())
        {
        }
        
        public CommandBinderException(CommandBinderExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}