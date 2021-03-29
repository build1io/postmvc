using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Utils.Extensions;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI.Impl
{
    public abstract class UIControlsController<T, C> where T : UIControl<C>
                                                     where C : UIControlConfiguration
    {
        [Inject] protected IUILayersController UILayerController { get; set; }
        [Inject] protected IAssetsController   AssetsController  { get; set; }
        [Inject] protected IMediationBinder    MediationBinder   { get; set; }
        [Inject] protected IDeviceController   DeviceController  { get; set; }

        private readonly HashSet<UIControlConfiguration> _installedConfigs;

        protected UIControlsController()
        {
            _installedConfigs = new HashSet<UIControlConfiguration>();
        }

        /*
         * Public.
         */

        public void Initialize(IEnumerable<T> controls)
        {
            foreach (var control in controls)
            {
                var configuration = DeviceController.GetConfiguration(control);
                if (CheckConfigInstalled(configuration))
                    continue;

                InstallConfiguration(configuration);

                if (!control.ToPreInstantiate)
                    continue;

                var layerView = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
                Instantiate(control, configuration, layerView, false);
            }
        }
        
        /*
         * Protected.
         */

        protected GameObject GetInstance(T control, UIControlOptions options)
        {
            return GetInstance(control, options, out var isNewInstance);
        }
        
        protected GameObject GetInstance(T control, UIControlOptions options, out bool isNewInstance)
        {
            isNewInstance = false;
            if (control == null)
                return null;
            
            var configuration = DeviceController.GetConfiguration(control);
            if (!CheckConfigInstalled(configuration))
                InstallConfiguration(configuration);

            var activate = (options & UIControlOptions.Activate) == UIControlOptions.Activate;
            var layer = UILayerController.GetLayerView<Transform>(configuration.appLayerId);
            var instanceTransform = layer.Find(control.name);
            if (instanceTransform != null)
            {
                if (activate)
                    instanceTransform.gameObject.SetActive(true);
                return instanceTransform.gameObject;
            }

            var instantiate = (options & UIControlOptions.Instantiate) == UIControlOptions.Instantiate;
            if (!instantiate)
                return null;
            
            isNewInstance = true;
            return Instantiate(control, configuration, layer, activate);
        }

        protected bool Deactivate(T control)
        {
            if (control == null)
                return false;
            
            var configuration = DeviceController.GetConfiguration(control);
            var layer = UILayerController.GetLayerView(configuration.appLayerId);
            var view = layer.transform.Find(control.name);
            if (view == null || !view.gameObject.activeSelf)
                return false;

            if (control.ToDestroyOnDeactivation)
                Object.Destroy(view.gameObject);
            else
                view.gameObject.SetActive(false);

            return true;
        }

        protected GameObject Instantiate(T control, C configuration, Component parent, bool active)
        {
            var prefab = AssetsController.GetAsset<GameObject>(configuration.assetBundle, configuration.prefabName);
            prefab.SetActive(active);

            var instance = Object.Instantiate(prefab, parent.transform);
            instance.name = control.name;

            return instance;
        }

        protected bool CheckControlIsActive(T control)
        {
            if (control == null)
                return false;
            
            var configuration = DeviceController.GetConfiguration(control);
            var layer = UILayerController.GetLayerView(configuration.appLayerId);
            var view = layer.transform.Find(control.name);
            return view != null && view.gameObject.activeSelf;
        }

        protected bool CheckConfigInstalled(C configuration)
        {
            return _installedConfigs.Contains(configuration);
        }

        protected void InstallConfiguration(C configuration)
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