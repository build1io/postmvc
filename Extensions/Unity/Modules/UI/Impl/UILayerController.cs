using System;
using System.Collections.Generic;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI.Impl
{
    public sealed class UILayerController : IUILayerController
    {
        private readonly Dictionary<int, GameObject> _layers;
        
        public UILayerController()
        {
            _layers = new Dictionary<int, GameObject>();
        }

        /*
         * Public.
         */

        public void RegisterLayer(int layerId, GameObject view)
        {
            if (_layers.ContainsKey(layerId))
                throw new Exception($"Layer view already registered. Id: {layerId}");
            _layers.Add(layerId, view);
        }

        public GameObject GetLayerView(int layerId)
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new Exception($"Layer not registered. Id: {layerId}");
            return view;
        }
        
        public T GetLayerView<T>(int layerId) where T : Component
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new Exception($"Layer not registered: Id: {layerId}");

            if (typeof(T) == typeof(Transform))
                return view.transform as T;
            
            if (typeof(T) == typeof(GameObject))
                return view as T;
            
            throw new Exception($"Incompatible layer view type: {typeof(T)}");
        }
    }
}