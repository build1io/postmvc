using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Popup.Animation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public abstract class PopupView : UnityView, IPopupView
    {
        [Header("Parts Base"), SerializeField] private RectTransform  content;
        [SerializeField]                       private GameObject     raycastBlocker;
        [SerializeField]                       private PopupAnimation animationObject;

        [Inject] public IPopupController PopupController { get; set; }

        public PopupBase     Popup      { get; private set; }
        public GameObject    GameObject => gameObject;
        public RectTransform Content    => content;

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

        protected virtual void OnShown()  { }
        protected virtual void OnHidden() { }

        /*
         * Private.
         */

        private void OnShownImpl()
        {
            if (raycastBlocker)
                raycastBlocker.SetActive(false);

            OnShown();
        }

        private void OnHiddenImpl()
        {
            OnHidden();
            PopupController.Close(Popup, true);
        }
    }
}