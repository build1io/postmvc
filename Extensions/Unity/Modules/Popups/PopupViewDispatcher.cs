using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Popups.Animation;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popups
{
    public abstract class PopupViewDispatcher : UnityViewDispatcher, IPopupView
    {
        public static readonly MVCS.Events.Event OnShown  = new MVCS.Events.Event();
        public static readonly MVCS.Events.Event OnHidden = new MVCS.Events.Event();

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

            if (animationObject)
            {
                if (raycastBlocker)
                    raycastBlocker.SetActive(true);

                animationObject.AnimateShow(this, OnShownImpl);
                return;
            }

            OnShownImpl();
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
            if (animationObject)
            {
                if (raycastBlocker)
                    raycastBlocker.SetActive(true);

                animationObject.AnimateHide(this, OnHiddenImpl);
                return;
            }

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
            
            Dispatch(OnShown);
        }

        private void OnHiddenImpl()
        {
            OnHiddenHandler();

            Dispatch(OnHidden);

            PopupController.Close(Popup, true);
        }
    }
}