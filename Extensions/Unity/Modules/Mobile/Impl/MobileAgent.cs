using System;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Mobile.Impl
{
    internal sealed class MobileAgent : MonoBehaviour
    {
        internal event Action OnEsc;

        #if ENABLE_INPUT_SYSTEM
        
        private InputActions _actions;
        
        private void Awake()
        {
            _actions = new InputActions();
            _actions.Enable();
        }

        private void OnDestroy()
        {
            _actions.Disable();
            _actions = null;
        }
        
        private void Update()
        {
            if (_actions.AndroidBackButton.Esc.WasReleasedThisFrame())
                OnEsc?.Invoke();    
        }
        
        #else
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                OnEsc?.Invoke();
        }
        
        #endif
    }
}