using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup.Impl
{
    public sealed class PopupController : UIControlsController<PopupBase, PopupConfig>, IPopupController
    {
        [Logger(LogLevel.Verbose)] public ILogger          Logger          { get; set; }
        [Inject]                   public IEventDispatcher Dispatcher      { get; set; }
        [Inject]                   public IInjectionBinder InjectionBinder { get; set; }

        private readonly Queue<PopupBase> _queue;
        private readonly Queue<object>    _queueData;

        private PopupBase _currentPopupInfo;

        public PopupController()
        {
            _queue = new Queue<PopupBase>();
            _queueData = new Queue<object>();
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

        public void Open(Popup popup)
        {
            Open(popup, PopupBehavior.Default);
        }

        public void Open(Popup popup, PopupBehavior behavior)
        {
            if (behavior != PopupBehavior.Default)
                throw new Exception($"Unknown behavior: {behavior}");
            
            _queue.Enqueue(popup);
            _queueData.Enqueue(null);
            
            if (_currentPopupInfo == null)
                ProcessQueue();
        }

        public void Open<T>(Popup<T> popup, T data)
        {
            Open(popup, data, PopupBehavior.Default);
        }
        
        public void Open<T>(Popup<T> popup, T data, PopupBehavior behavior)
        {
            if (behavior != PopupBehavior.Default)
                throw new Exception($"Unknown behavior: {behavior}");
            
            _queue.Enqueue(popup);
            _queueData.Enqueue(data);
            
            if (_currentPopupInfo == null)
                ProcessQueue();
        }

        /*
         * Closing.
         */

        public void Close()
        {
            Close(_currentPopupInfo);
        }
        
        public void Close(PopupBase popup)
        {
            if (_currentPopupInfo != popup)
            {
                Logger.Error(p => $"Current popup doesn't match to closing popup: {_currentPopupInfo} != {p}", popup);
                return;
            }
            
            if (!Deactivate(popup))
            {
                Logger.Error(p => $"Closing dialog not found: {p}", popup);
                return;
            }

            var closedPopup = _currentPopupInfo;
            _currentPopupInfo = null;

            Dispatcher.Dispatch(PopupEvent.Closed, closedPopup);
        }

        /*
         * Private.
         */

        private void ProcessQueue()
        {
            if (_currentPopupInfo != null)
            {
                Logger.Error(() => $"Can't process queue. Current dialog isn't closed: {_currentPopupInfo}");
                return;
            }

            // Queue is empty, no dialogs to show.
            if (_queue.Count == 0)
                return;

            var popup = _queue.Dequeue();
            var data = _queueData.Dequeue();

            _currentPopupInfo = popup;

            IInjectionBinding dataBinding = null;

            if (popup.dataType != null)
                dataBinding = InjectionBinder.Bind(popup.dataType).ToValue(data).ToBinding();
            
            var instance = GetInstance(popup, UIControlOptions.Instantiate, out var isNewInstance);
            var view = instance.GetComponent<PopupView>();
            if (view == null)
                throw new Exception("Popup view doesn't inherit from PopupView.");

            view.SetUp(popup);

            if (!isNewInstance)
            {
                if (popup.dataType != null && view.Initialized)
                    MediationBinder.UpdateViewInjections(view);

                view.gameObject.SetActive(true);
            }

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