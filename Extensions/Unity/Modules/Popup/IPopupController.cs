namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public interface IPopupController
    {
        void Open(Popup popup);
        void Open(Popup popup, PopupBehavior behavior);
        
        void Open<T>(Popup<T> popup, T data);
        void Open<T>(Popup<T> popup, T data, PopupBehavior behavior);

        void Close();
        void Close(PopupBase popup);
    }
}