using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using UnityEngine;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup.Impl
{
    public sealed class PopupController : IPopupController
    {
        [Logger(LogLevel.Verbose)] public ILogger            Logger            { get; set; }
        [Inject]                   public IEventDispatcher   Dispatcher        { get; set; }
        [Inject]                   public IAssetsController  AssetsController  { get; set; }
        [Inject]                   public IDeviceController  DeviceController  { get; set; }
        [Inject]                   public IInjectionBinder   InjectionBinder   { get; set; }
        [Inject]                   public IMediationBinder   MediationBinder   { get; set; }
        [Inject]                   public IUILayerController UILayerController { get; set; }

        private readonly Queue<PopupBase>     _queue;
        private readonly Queue<object>        _queueData;
        private readonly HashSet<PopupConfig> _installedConfigs;

        private PopupBase _currentPopupInfo;

        public PopupController()
        {
            _queue = new Queue<PopupBase>();
            _queueData = new Queue<object>();
            _installedConfigs = new HashSet<PopupConfig>();
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
         * Initialization.
         */

        public void Initialize(IEnumerable<PopupBase> popups)
        {
            foreach (var popup in popups)
            {
                var configuration = DeviceController.GetConfiguration(popup);
                if (CheckConfigInstalled(configuration))
                    continue;

                InstallConfiguration(configuration);

                if (!popup.ToPreInstantiate)
                    continue;

                var layerView = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
                Instantiate(popup, configuration, layerView, false);
            }
        }

        /*
         * Opening.
         */

        public void Open(Popup popup)
        {
            _queue.Enqueue(popup);
            _queueData.Enqueue(null);
            if (_currentPopupInfo == null)
                ProcessQueue();
        }

        public void Open<T>(Popup<T> popup, T data)
        {
            _queue.Enqueue(popup);
            _queueData.Enqueue(data);
            if (_currentPopupInfo == null)
                ProcessQueue();
        }

        /*
         * Closing.
         */

        public void Close(PopupBase popup)
        {
            if (_currentPopupInfo != popup)
            {
                Logger.Error(() => $"Current popup doesn't match to closing popup: {_currentPopupInfo} != {popup}");
                return;
            }

            var configuration = DeviceController.GetConfiguration(popup);
            var layer = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
            var currentView = layer.Find(popup.name);
            if (currentView == null)
            {
                Logger.Error(() => $"Closing dialog not found: {popup}");
                return;
            }

            if (popup.ToDestroyOnClose)
                Object.Destroy(currentView.gameObject);
            else
                currentView.gameObject.SetActive(false);

            var closedPopup = _currentPopupInfo;
            _currentPopupInfo = null;

            Dispatcher.Dispatch(PopupEvent.Closed, closedPopup);
        }

        public void CloseCurrent()
        {
            Close(_currentPopupInfo);
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

            var configuration = DeviceController.GetConfiguration(popup);
            if (!CheckConfigInstalled(configuration))
                InstallConfiguration(configuration);

            var layer = UILayerController.GetLayerView(configuration.appLayerId);
            var transform = layer.transform.Find(popup.name);
            var instance = transform != null ? transform.gameObject : null;

            var isNewInstance = false;
            if (instance == null)
            {
                instance = Instantiate(popup, configuration, layer.transform, true);
                isNewInstance = true;
            }

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

        private GameObject Instantiate(PopupBase popup, PopupConfig configuration, Component parent, bool active)
        {
            var prefab = AssetsController.GetAsset<GameObject>(configuration.assetBundleId, configuration.prefabName);
            prefab.SetActive(active);

            var instance = Object.Instantiate(prefab, parent.transform);
            instance.name = popup.name;

            return instance;
        }
        
        private bool CheckConfigInstalled(PopupConfig configuration)
        {
            return _installedConfigs.Contains(configuration);
        }

        private void InstallConfiguration(PopupConfig configuration)
        {
            foreach (var binding in configuration)
            {
                var bindingTo = binding.viewInterfaceType != null
                                    ? MediationBinder.Bind(binding.viewType, binding.viewInterfaceType)
                                    : MediationBinder.Bind(binding.viewType);
                if (binding.mediatorType != null)
                    bindingTo.To(binding.mediatorType);
            }

            _installedConfigs.Add(configuration);
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