#if ENABLE_INPUT_SYSTEM

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Build1.PostMVC.Extensions.Unity.Modules.Mobile.Impl
{
    public sealed class InputActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""AndroidBackButton"",
            ""id"": ""1d4213ae-38a6-444a-a28d-e9b99fc92d5e"",
            ""actions"": [
                {
                    ""name"": ""Esc"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6f200176-2d25-494c-8fbc-7f8c61b1ab02"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""241b313f-c30d-4516-a73d-041f5fc1cbbc"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Esc"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // AndroidBackButton
            m_AndroidBackButton = asset.FindActionMap("AndroidBackButton", throwIfNotFound: true);
            m_AndroidBackButton_Esc = m_AndroidBackButton.FindAction("Esc", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // AndroidBackButton
        private readonly InputActionMap            m_AndroidBackButton;
        private          IAndroidBackButtonActions m_AndroidBackButtonActionsCallbackInterface;
        private readonly InputAction               m_AndroidBackButton_Esc;
        public struct AndroidBackButtonActions
        {
            private @InputActions m_Wrapper;
            public AndroidBackButtonActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public                          InputAction    @Esc                          => m_Wrapper.m_AndroidBackButton_Esc;
            public                          InputActionMap Get()                         { return m_Wrapper.m_AndroidBackButton; }
            public                          void           Enable()                      { Get().Enable(); }
            public                          void           Disable()                     { Get().Disable(); }
            public                          bool           enabled                       => Get().enabled;
            public static implicit operator InputActionMap(AndroidBackButtonActions set) { return set.Get(); }
            public void SetCallbacks(IAndroidBackButtonActions instance)
            {
                if (m_Wrapper.m_AndroidBackButtonActionsCallbackInterface != null)
                {
                    @Esc.started -= m_Wrapper.m_AndroidBackButtonActionsCallbackInterface.OnEsc;
                    @Esc.performed -= m_Wrapper.m_AndroidBackButtonActionsCallbackInterface.OnEsc;
                    @Esc.canceled -= m_Wrapper.m_AndroidBackButtonActionsCallbackInterface.OnEsc;
                }
                m_Wrapper.m_AndroidBackButtonActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Esc.started += instance.OnEsc;
                    @Esc.performed += instance.OnEsc;
                    @Esc.canceled += instance.OnEsc;
                }
            }
        }
        public AndroidBackButtonActions @AndroidBackButton => new AndroidBackButtonActions(this);
        public interface IAndroidBackButtonActions
        {
            void OnEsc(InputAction.CallbackContext context);
        }
    }
}

#endif