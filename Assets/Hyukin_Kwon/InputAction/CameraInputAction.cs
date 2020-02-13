// GENERATED AUTOMATICALLY FROM 'Assets/Hyukin_Kwon/InputAction/CameraInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @CameraInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @CameraInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraInputAction"",
    ""maps"": [
        {
            ""name"": ""Camera Controls"",
            ""id"": ""a606d20a-b28a-469d-a304-2094f4f56b57"",
            ""actions"": [
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Button"",
                    ""id"": ""898c2227-4ba5-426e-86a4-f30c189e04a0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""1ef58676-f09e-41e1-a3af-0b3f371cc745"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bc2ad3fc-2d85-4de5-bcc5-a9386191117c"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df534737-4f20-407f-a73d-94b944cfffdc"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78bddebf-4508-4ba1-af16-f3a0fe959404"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cc449a4-b54c-4410-9179-395e40cac70d"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Camera Controls
        m_CameraControls = asset.FindActionMap("Camera Controls", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
        m_CameraControls_Reset = m_CameraControls.FindAction("Reset", throwIfNotFound: true);
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

    // Camera Controls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_Zoom;
    private readonly InputAction m_CameraControls_Reset;
    public struct CameraControlsActions
    {
        private @CameraInputAction m_Wrapper;
        public CameraControlsActions(@CameraInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputAction @Reset => m_Wrapper.m_CameraControls_Reset;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @Zoom.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Reset.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnReset;
                @Reset.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnReset;
                @Reset.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnReset;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Reset.started += instance.OnReset;
                @Reset.performed += instance.OnReset;
                @Reset.canceled += instance.OnReset;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICameraControlsActions
    {
        void OnZoom(InputAction.CallbackContext context);
        void OnReset(InputAction.CallbackContext context);
    }
}
