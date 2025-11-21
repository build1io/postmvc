using System;

namespace Build1.PostMVC.Core.MVCS.Events
{
    public interface IEventHandleFailable : IEventHandle
    {
        void Fail(Exception exception);
    }
}