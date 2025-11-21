namespace Build1.PostMVC.Core.MVCS.Events
{
    internal interface IEventHandleBreakable : IEventHandle
    {
        void Break();
    }
}