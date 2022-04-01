using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common
{
    public abstract class CommandTestEvent
    {
        public static readonly Event                           Event00 = new Event();
        public static readonly Event<int>                      Event01 = new Event<int>();
        public static readonly Event<int, string>              Event02 = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03 = new Event<int, string, CommandData>();
        
        public static readonly Event                           Event00Copy01 = new Event();
        public static readonly Event<int>                      Event01Copy01 = new Event<int>();
        public static readonly Event<int, string>              Event02Copy01 = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03Copy01 = new Event<int, string, CommandData>();

        public static readonly Event                           Event00Copy02 = new Event();
        public static readonly Event<int>                      Event01Copy02 = new Event<int>();
        public static readonly Event<int, string>              Event02Copy02 = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03Copy02 = new Event<int, string, CommandData>();

        public static readonly Event                           Event00Complete = new Event();
        public static readonly Event<int>                      Event01Complete = new Event<int>();
        public static readonly Event<int, string>              Event02Complete = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03Complete = new Event<int, string, CommandData>();
        
        public static readonly Event                           Event00Break = new Event();
        public static readonly Event<int>                      Event01Break = new Event<int>();
        public static readonly Event<int, string>              Event02Break = new Event<int, string>();
        public static readonly Event<int, string, CommandData> Event03Break = new Event<int, string, CommandData>();

        public static readonly Event<Exception> EventFail = new Event<Exception>();
    }
}