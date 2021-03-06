namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public interface IPopupController
    {
        bool HasOpenPopups { get; }

        void Open(Popup popup);
        void Open(Popup popup, PopupBehavior behavior);
        
        void Open<T>(Popup<T> popup, T data);
        void Open<T>(Popup<T> popup, T data, PopupBehavior behavior);
        
        void Close(PopupBase popup);
        void CloseAll();
    }
}