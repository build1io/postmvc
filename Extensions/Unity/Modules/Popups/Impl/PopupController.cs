using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popups.Impl
{
    public sealed class PopupController : UIControlsController<PopupBase, PopupConfigBase>, IPopupController
    {
        [Log(LogLevel.Warning)] public ILog             Log             { get; set; }
        [Inject]                public IEventDispatcher Dispatcher      { get; set; }
        [Inject]                public IInjectionBinder InjectionBinder { get; set; }

        public bool HasOpenPopups => _openPopups.Count > 0;

        private readonly List<PopupBase>  _openPopups;
        private readonly Queue<PopupBase> _queue;
        private readonly Queue<object>    _queueData;

        public PopupController()
        {
            _openPopups = new List<PopupBase>(4);
            _queue = new Queue<PopupBase>(4);
            _queueData = new Queue<object>(4);
        }

        [PostConstruct]
        public void PostConstruct()
        {
            Dispatcher.AddListener(PopupEvent.Closed, OnPopupClosed);
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Dispatcher.RemoveListener(PopupEvent.Closed, OnPopupClosed);
        }

        /*
         * Opening.
         */

        public void Open(Popup popup)                         { OpenImpl(popup, null, PopupBehavior.Default); }
        public void Open(Popup popup, PopupBehavior behavior) { OpenImpl(popup, null, behavior); }

        public void Open<T>(Popup<T> popup, T data)                         { OpenImpl(popup, data, PopupBehavior.Default); }
        public void Open<T>(Popup<T> popup, T data, PopupBehavior behavior) { OpenImpl(popup, data, behavior); }

        private void OpenImpl(PopupBase popup, object data, PopupBehavior behavior)
        {
            switch (behavior)
            {
                case PopupBehavior.Default:
                {
                    _queue.Enqueue(popup);
                    _queueData.Enqueue(data);

                    if (_openPopups.Count == 0)
                        ProcessQueue();

                    return;
                }

                case PopupBehavior.OpenOnTop:
                    OpenPopup(popup, data);
                    return;

                default:
                    throw new Exception($"Unable to process behavior: {behavior}");
            }
        }

        /*
         * Closing.
         */

        public void Close(PopupBase popup, bool immediate = false)
        {
            if (!_openPopups.Contains(popup))
            {
                Log.Error(p => $"Specified popup is not open: {p}", popup);
                return;
            }

            if (immediate)
            {
                if (Deactivate(popup) && _openPopups.Remove(popup))
                {
                    Dispatcher.Dispatch(PopupEvent.Closed, popup);
                    return;
                }

                Log.Error(p => $"Failed to deactivate popup: {p}", popup);
                return;
            }
            
            var instance = GetInstance(popup, UIControlOptions.None);
            if (!instance)
                throw new Exception("Popup instance not found.");
            
            var view = instance.GetComponent<PopupView>() ?? (IPopupView)instance.GetComponent<PopupViewDispatcher>();
            if (view == null)
                throw new Exception("Popup view doesn't inherit from PopupView or PopupViewDispatcher.");

            view.Close();
        }

        public void CloseAll(bool immediate = false)
        {
            for (var i = _openPopups.Count - 1; i >= 0; i--)
                Close(_openPopups[i], immediate);
        }
        
        /*
         * Check.
         */

        public bool CheckPopupOpen(PopupBase popup)
        {
            return _openPopups.Any(p => p == popup);
        }

        public bool CheckPopupOnTop(IPopupView popupView)
        {
            if (_openPopups.Count == 0)
                return false;

            var lastOpenedPopup = _openPopups[_openPopups.Count - 1];
            
            return lastOpenedPopup == popupView.Popup;
        }

        /*
         * Private.
         */

        private void ProcessQueue()
        {
            if (_openPopups.Count > 0)
            {
                Log.Debug(op => $"Queue processing cancelled. There are open popups: {string.Join(",", op.Select(p => p.ToString()))}", _openPopups);
                return;
            }

            // Checking if there are popups in the queue. 
            if (_queue.Count > 0)
                OpenPopup(_queue.Dequeue(), _queueData.Dequeue());
        }

        private void OpenPopup(PopupBase popup, object data)
        {
            _openPopups.Add(popup);

            IInjectionBinding dataBinding = null;

            if (popup.dataType != null)
                dataBinding = InjectionBinder.Bind(popup.dataType).ToValue(data).ToBinding();

            var instance = GetInstance(popup, UIControlOptions.Instantiate);

            var view = instance.GetComponent<PopupView>() ?? (IPopupView)instance.GetComponent<PopupViewDispatcher>();
            if (view == null)
                throw new Exception("Popup view doesn't inherit from PopupView or PopupViewDispatcher.");

            view.SetUp(popup);

            if (view.Initialized)
                MediationBinder.UpdateViewInjections(view);

            view.GameObject.SetActive(true);

            if (dataBinding != null)
                InjectionBinder.Unbind(dataBinding);

            Dispatcher.Dispatch(PopupEvent.Open, popup);
        }

        /*
         * Event Handlers.
         */

        private void OnPopupClosed(PopupBase popup)
        {
            ProcessQueue();
        }
    }
}