using System;
using Build1.PostMVC.Core.MVCS.Commands.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class CommandException : Exception
    {
        internal CommandException(CommandExceptionType type, CommandBase command) : base($"{type.ToString()} [{command.GetType().Name}]")
        {
        }
    }
}