using System;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    internal class CommandBinderException : Exception
    {
        public CommandBinderException(CommandBinderExceptionType type) : base(type.ToString())
        {
        }
        
        public CommandBinderException(CommandBinderExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}