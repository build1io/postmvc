using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public interface IPopupController
    {
        void Initialize(IEnumerable<PopupBase> popups);

        void Open(Popup popup);
        void Open<T>(Popup<T> popup, T data);

        void Close(PopupBase popup);
        void CloseCurrent();
    }
}