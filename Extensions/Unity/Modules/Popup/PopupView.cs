using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Popup.Animation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public abstract class PopupView : UnityView, IPopupView
    {
        [Header("Parts Base"), SerializeField] private GameObject     overlay;
        [SerializeField]                       private RectTransform  content;
        [SerializeField]                       private GameObject     raycastBlocker;
        [SerializeField]                       private PopupAnimation animationObject;

        [Inject] public IPopupController PopupController { get; set; }

        public PopupBase  Popup      { get; private set; }
        public GameObject GameObject => gameObject;

        public GameObject    Overlay => overlay;
        public RectTransform Content => content;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (raycastBlocker)
                raycastBlocker.SetActive(true);

            if (animationObject)
                animationObject.AnimateShow(this, OnShownImpl);
        }

        /*
         * Public.
         */

        public void SetUp(PopupBase popup)
        {
            Popup = popup;
        }

        public virtual void Close()
        {
            if (raycastBlocker)
                raycastBlocker.SetActive(true);

            if (animationObject)
                animationObject.AnimateHide(this, OnHiddenImpl);
            else
                OnHiddenImpl();
        }

        /*
         * Protected.
         */

        protected virtual void OnShownHandler()  { }
        protected virtual void OnHiddenHandler() { }

        /*
         * Private.
         */

        private void OnShownImpl()
        {
            if (raycastBlocker)
                raycastBlocker.SetActive(false);

            OnShownHandler();
        }

        private void OnHiddenImpl()
        {
            OnHiddenHandler();
            PopupController.Close(Popup, true);
        }
    }
}