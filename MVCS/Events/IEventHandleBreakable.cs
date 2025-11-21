namespace Build1.PostMVC.Core.MVCS.Events
{
    public interface IEventHandleBreakable : IEventHandle
    {
        void Break();
    }
}