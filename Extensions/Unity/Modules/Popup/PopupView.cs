using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Mediation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public abstract class PopupView : UnityView, IPopupView
    {
        [Inject] public IPopupController PopupController { get; set; }

        public GameObject GameObject => gameObject;
        public PopupBase  Popup      { get; private set; }

        public void SetUp(PopupBase popup)
        {
            Popup = popup;
        }

        public virtual void Close()
        {
            PopupController.Close(Popup, true);
        }
    }
}