namespace Build1.PostMVC.Core.MVCS.Events
{
    public interface IEventHandle
    {
        bool IsRetained { get; }

        void Retain();
        void Release();
    }
}