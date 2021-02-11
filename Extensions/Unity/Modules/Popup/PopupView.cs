using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Mediation;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public abstract class PopupView : UnityView
    {
        [Inject] public IPopupController PopupController { get; set; }

        public PopupBase Popup { get; private set; }

        public void SetUp(PopupBase popup)
        {
            Popup = popup;
        }

        public void Close()
        {
            PopupController.Close(Popup);
        }
    }
}