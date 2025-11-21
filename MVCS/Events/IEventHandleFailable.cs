using System;

namespace Build1.PostMVC.Core.MVCS.Events
{
    internal interface IEventHandleFailable : IEventHandle
    {
        void Fail(Exception exception);
    }
}