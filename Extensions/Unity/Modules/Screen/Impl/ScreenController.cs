using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Utils;
using UnityEngine;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen.Impl
{
    public sealed class ScreenController : IScreenController
    {
        [Logger(LogLevel.Verbose)] public ILogger            Logger            { get; set; }
        [Inject]                   public IEventDispatcher   Dispatcher        { get; set; }
        [Inject]                   public IUILayerController UILayerController { get; set; }
        [Inject]                   public IAssetsController  AssetsController  { get; set; }
        [Inject]                   public IMediationBinder   MediationBinder   { get; set; }
        [Inject]                   public IDeviceController  DeviceController  { get; set; }

        private readonly HashSet<ScreenConfig> _installedConfigs;

        private Screen _currentScreen;

        public ScreenController()
        {
            _installedConfigs = new HashSet<ScreenConfig>();
        }

        /*
         * Public.
         */

        public void Initialize(IEnumerable<Screen> screens)
        {
            foreach (var screen in screens)
            {
                var configuration = DeviceController.GetConfiguration(screen);
                if (CheckConfigInstalled(configuration))
                    continue;

                InstallConfiguration(configuration);

                if (!screen.ToPreInstantiate)
                    continue;

                var layerView = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
                Instantiate(screen, configuration, layerView, false);
            }
        }

        public void Show(Screen screen)
        {
            if (_currentScreen != null)
                Close(_currentScreen);

            _currentScreen = screen;
            if (_currentScreen == null)
                return;

            var configuration = DeviceController.GetConfiguration(_currentScreen);
            if (!CheckConfigInstalled(configuration))
                InstallConfiguration(configuration);

            var layer = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
            var instanceTransform = layer.Find(configuration.prefabName);
            if (instanceTransform != null)
                instanceTransform.gameObject.SetActive(true);
            else
                Instantiate(screen, configuration, layer, true);

            Dispatcher.Dispatch(ScreenEvent.Open, screen);
        }

        /*
         * Private.
         */

        private void Close(Screen screen)
        {
            var configuration = DeviceController.GetConfiguration(screen);
            var layer = UILayerController.GetLayerView(configuration.appLayerId);
            var view = layer.GetFirstActiveChild();
            if (view == null || view.name != screen.name)
                return;

            if (screen.ToDestroyOnClose)
                Object.Destroy(view.gameObject);
            else
                view.gameObject.SetActive(false);

            Dispatcher.Dispatch(ScreenEvent.Closed, screen);
        }

        private void Instantiate(Screen screen, UIControlConfiguration configuration, Component parent, bool active)
        {
            var prefab = AssetsController.GetAsset<GameObject>(configuration.assetBundleId, configuration.prefabName);
            prefab.SetActive(active);

            var instance = Object.Instantiate(prefab, parent.transform);
            instance.name = screen.name;
        }

        private bool CheckConfigInstalled(ScreenConfig configuration)
        {
            return _installedConfigs.Contains(configuration);
        }

        private void InstallConfiguration(ScreenConfig configuration)
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
    }
}