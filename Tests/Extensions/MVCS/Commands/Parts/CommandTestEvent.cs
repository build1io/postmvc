using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public abstract class CommandTestEvent
    {
        public static readonly Event                           Event00 = new Event();
        public static readonly Event<int>                      Event01 = new Event<int>();
        public static readonly Event<int, string>              Event02 = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03 = new Event<int, string, CommandData>();
    }
}