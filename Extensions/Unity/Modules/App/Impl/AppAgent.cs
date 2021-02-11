using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Impl
{
    public sealed class AppAgent : MonoBehaviour
    {
        public event Action<bool> Pause;
        public event Action<bool> Focus;
        public event Action       Quitting;
        
        [PostConstruct]
        public void PostConstruct()
        {
            Application.focusChanged += OnFocusChanged;
            Application.quitting += OnQuitting;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Application.focusChanged -= OnFocusChanged;
            Application.quitting -= OnQuitting;
        }
        
        /*
         * Mono Behavior.
         */

        private void OnApplicationPause(bool pauseStatus)
        {
            Pause?.Invoke(pauseStatus);
        }
        
        private void OnFocusChanged(bool focused)
        {
            Focus?.Invoke(focused);
        }
        
        private void OnQuitting()
        {
            Quitting?.Invoke();
        }
    }
}