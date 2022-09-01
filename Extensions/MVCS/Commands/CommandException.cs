using System;

namespace Build1.PostMVC.Core.Extensions.MVCS.Commands
{
    public sealed class CommandException : Exception
    {
        public CommandException(CommandExceptionType type) : base(type.ToString())
        {
        }
        
        public CommandException(CommandExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}