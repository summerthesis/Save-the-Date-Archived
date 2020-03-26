// GENERATED AUTOMATICALLY FROM 'Assets/Hyukin_Kwon/InputAction/PlayerInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""Player Controls"",
            ""id"": ""47558235-f427-4844-94fd-6e669c4852a2"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""bdce686e-f2c5-4360-88b7-c19d533ebfa3"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""27ba75bd-e10e-4f74-881a-d5c14b053f4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ActivateHook"",
                    ""type"": ""Button"",
                    ""id"": ""6298dd73-24c4-47ca-89f2-34b42bf65d0b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ElecArm"",
                    ""type"": ""Button"",
                    ""id"": ""56a0e44a-df62-43f1-b37f-028d909a1dc5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ActivateMagArm"",
                    ""type"": ""Button"",
                    ""id"": ""27dca19b-aede-49a3-a087-caa37659cb1f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3f26004c-f9e5-4d87-a20a-823650b55901"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""MoveAxis"",
                    ""id"": ""2d998636-e2fc-4953-b329-7859f087a4f4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cd7dbdd5-e88b-4e55-9a48-a8c7c7c6551b"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""976edf49-59d5-409f-966b-12430845fba7"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42bece95-25ec-4320-b386-5f42b8ce4fde"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d6e16011-0c2d-4119-afae-4c9a62f1799e"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""85024393-279d-4d7d-89e5-125dadf0d644"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""be047dff-a4cf-4131-a12f-799361720583"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7140730f-8ac9-4610-a7f5-2320f64772cf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5167d804-1402-41d6-9c03-bbfc1b22e90a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""02942a27-5f9a-4237-8586-1784bec131ac"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a4b694ce-0d10-4c56-9a72-c038c0589e94"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2737dd3e-bc73-418b-a203-99080f6762bc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6cc62609-36fa-4ea7-9cb8-bf43da07e1bc"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ActivateHook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""495b632a-6f12-497c-800c-bb9774eef36e"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ElecArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01f14076-9c49-4df6-a4c5-42b24dd8b7b4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""ElecArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b00e7355-636d-488c-85a9-1da79951eaf5"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ActivateMagArm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""InventoryControls"",
            ""id"": ""b07e10e3-90e4-45e2-af4e-0ca004d01b92"",
            ""actions"": [
                {
                    ""name"": ""Left Bumper"",
                    ""type"": ""Button"",
                    ""id"": ""96fcca6c-8af8-41d3-9a29-550779a22693"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Bumper"",
                    ""type"": ""Button"",
                    ""id"": ""f59fdf44-6f47-49b8-a790-0357988b8d93"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftSelectCog"",
                    ""type"": ""Button"",
                    ""id"": ""16e84fa2-5b4f-4f6f-b652-04d56b19475e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightSelectCog"",
                    ""type"": ""Button"",
                    ""id"": ""3b2f8056-134a-4de9-a533-ccb3853960b0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""East Button"",
                    ""type"": ""Button"",
                    ""id"": ""04748b9e-1b5a-4987-9681-ecdad844cf1a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""South Button"",
                    ""type"": ""Button"",
                    ""id"": ""6861ee0b-ad32-47fb-94c7-7909cc523062"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectPushed"",
                    ""type"": ""Button"",
                    ""id"": ""524d8a71-3cab-43f4-b090-8bb9972209ad"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""897c15cc-dc3a-4189-a1b6-ddab6841ef7a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""ea9d91ea-40c6-4562-bcf9-e4c592fe0117"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left"",
                    ""type"": ""Button"",
                    ""id"": ""8582c7af-b2c5-4cc5-b4ef-baa79247ebba"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right"",
                    ""type"": ""Button"",
                    ""id"": ""647367cc-52b9-4697-bcba-a9b0fbd84242"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""df1970f9-7f88-49f2-9d60-fec4195a338d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Left Bumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9a3987f-9a4d-4818-a619-c60793f206cc"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Right Bumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13a11cdc-417e-4308-a848-7a7e5ec74a9c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LeftSelectCog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e69e55e-c8b2-4a73-ae78-bb54f91dc342"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""LeftSelectCog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""905876c8-133d-4ff2-b21a-30eaed5dde01"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RightSelectCog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81fd1348-ca24-4903-bc7b-f4116fb7a60b"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RightSelectCog"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42ff141b-b233-40ca-aa16-3ea30a0e9f1c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""East Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98fcad01-a135-400a-a5ae-78f73d0d61e4"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""South Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14a2bafd-1a5a-4ea8-83d2-d3a0cadfa04d"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectPushed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cb84442a-e9fd-4b4a-942d-7c12764f3ea1"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ce5a48d-53d9-4aee-b14f-d1ba3462e121"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de0389e8-cc18-468e-b08e-a330c0a8ec52"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abbcc1af-9dbe-48eb-a610-7ae4c80d90c5"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Right"",
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
        // Player Controls
        m_PlayerControls = asset.FindActionMap("Player Controls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_ActivateHook = m_PlayerControls.FindAction("ActivateHook", throwIfNotFound: true);
        m_PlayerControls_ElecArm = m_PlayerControls.FindAction("ElecArm", throwIfNotFound: true);
        m_PlayerControls_ActivateMagArm = m_PlayerControls.FindAction("ActivateMagArm", throwIfNotFound: true);
        // InventoryControls
        m_InventoryControls = asset.FindActionMap("InventoryControls", throwIfNotFound: true);
        m_InventoryControls_LeftBumper = m_InventoryControls.FindAction("Left Bumper", throwIfNotFound: true);
        m_InventoryControls_RightBumper = m_InventoryControls.FindAction("Right Bumper", throwIfNotFound: true);
        m_InventoryControls_LeftSelectCog = m_InventoryControls.FindAction("LeftSelectCog", throwIfNotFound: true);
        m_InventoryControls_RightSelectCog = m_InventoryControls.FindAction("RightSelectCog", throwIfNotFound: true);
        m_InventoryControls_EastButton = m_InventoryControls.FindAction("East Button", throwIfNotFound: true);
        m_InventoryControls_SouthButton = m_InventoryControls.FindAction("South Button", throwIfNotFound: true);
        m_InventoryControls_SelectPushed = m_InventoryControls.FindAction("SelectPushed", throwIfNotFound: true);
        m_InventoryControls_Up = m_InventoryControls.FindAction("Up", throwIfNotFound: true);
        m_InventoryControls_Down = m_InventoryControls.FindAction("Down", throwIfNotFound: true);
        m_InventoryControls_Left = m_InventoryControls.FindAction("Left", throwIfNotFound: true);
        m_InventoryControls_Right = m_InventoryControls.FindAction("Right", throwIfNotFound: true);
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

    // Player Controls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_ActivateHook;
    private readonly InputAction m_PlayerControls_ElecArm;
    private readonly InputAction m_PlayerControls_ActivateMagArm;
    public struct PlayerControlsActions
    {
        private @PlayerInputAction m_Wrapper;
        public PlayerControlsActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @ActivateHook => m_Wrapper.m_PlayerControls_ActivateHook;
        public InputAction @ElecArm => m_Wrapper.m_PlayerControls_ElecArm;
        public InputAction @ActivateMagArm => m_Wrapper.m_PlayerControls_ActivateMagArm;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @ActivateHook.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateHook;
                @ActivateHook.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateHook;
                @ActivateHook.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateHook;
                @ElecArm.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnElecArm;
                @ElecArm.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnElecArm;
                @ElecArm.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnElecArm;
                @ActivateMagArm.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateMagArm;
                @ActivateMagArm.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateMagArm;
                @ActivateMagArm.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnActivateMagArm;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @ActivateHook.started += instance.OnActivateHook;
                @ActivateHook.performed += instance.OnActivateHook;
                @ActivateHook.canceled += instance.OnActivateHook;
                @ElecArm.started += instance.OnElecArm;
                @ElecArm.performed += instance.OnElecArm;
                @ElecArm.canceled += instance.OnElecArm;
                @ActivateMagArm.started += instance.OnActivateMagArm;
                @ActivateMagArm.performed += instance.OnActivateMagArm;
                @ActivateMagArm.canceled += instance.OnActivateMagArm;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // InventoryControls
    private readonly InputActionMap m_InventoryControls;
    private IInventoryControlsActions m_InventoryControlsActionsCallbackInterface;
    private readonly InputAction m_InventoryControls_LeftBumper;
    private readonly InputAction m_InventoryControls_RightBumper;
    private readonly InputAction m_InventoryControls_LeftSelectCog;
    private readonly InputAction m_InventoryControls_RightSelectCog;
    private readonly InputAction m_InventoryControls_EastButton;
    private readonly InputAction m_InventoryControls_SouthButton;
    private readonly InputAction m_InventoryControls_SelectPushed;
    private readonly InputAction m_InventoryControls_Up;
    private readonly InputAction m_InventoryControls_Down;
    private readonly InputAction m_InventoryControls_Left;
    private readonly InputAction m_InventoryControls_Right;
    public struct InventoryControlsActions
    {
        private @PlayerInputAction m_Wrapper;
        public InventoryControlsActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftBumper => m_Wrapper.m_InventoryControls_LeftBumper;
        public InputAction @RightBumper => m_Wrapper.m_InventoryControls_RightBumper;
        public InputAction @LeftSelectCog => m_Wrapper.m_InventoryControls_LeftSelectCog;
        public InputAction @RightSelectCog => m_Wrapper.m_InventoryControls_RightSelectCog;
        public InputAction @EastButton => m_Wrapper.m_InventoryControls_EastButton;
        public InputAction @SouthButton => m_Wrapper.m_InventoryControls_SouthButton;
        public InputAction @SelectPushed => m_Wrapper.m_InventoryControls_SelectPushed;
        public InputAction @Up => m_Wrapper.m_InventoryControls_Up;
        public InputAction @Down => m_Wrapper.m_InventoryControls_Down;
        public InputAction @Left => m_Wrapper.m_InventoryControls_Left;
        public InputAction @Right => m_Wrapper.m_InventoryControls_Right;
        public InputActionMap Get() { return m_Wrapper.m_InventoryControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InventoryControlsActions set) { return set.Get(); }
        public void SetCallbacks(IInventoryControlsActions instance)
        {
            if (m_Wrapper.m_InventoryControlsActionsCallbackInterface != null)
            {
                @LeftBumper.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftBumper;
                @LeftBumper.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftBumper;
                @LeftBumper.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftBumper;
                @RightBumper.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightBumper;
                @RightBumper.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightBumper;
                @RightBumper.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightBumper;
                @LeftSelectCog.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftSelectCog;
                @LeftSelectCog.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftSelectCog;
                @LeftSelectCog.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeftSelectCog;
                @RightSelectCog.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightSelectCog;
                @RightSelectCog.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightSelectCog;
                @RightSelectCog.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRightSelectCog;
                @EastButton.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnEastButton;
                @EastButton.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnEastButton;
                @EastButton.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnEastButton;
                @SouthButton.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSouthButton;
                @SouthButton.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSouthButton;
                @SouthButton.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSouthButton;
                @SelectPushed.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSelectPushed;
                @SelectPushed.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSelectPushed;
                @SelectPushed.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnSelectPushed;
                @Up.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnUp;
                @Down.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnDown;
                @Left.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeft;
                @Left.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeft;
                @Left.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnLeft;
                @Right.started -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRight;
                @Right.performed -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRight;
                @Right.canceled -= m_Wrapper.m_InventoryControlsActionsCallbackInterface.OnRight;
            }
            m_Wrapper.m_InventoryControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftBumper.started += instance.OnLeftBumper;
                @LeftBumper.performed += instance.OnLeftBumper;
                @LeftBumper.canceled += instance.OnLeftBumper;
                @RightBumper.started += instance.OnRightBumper;
                @RightBumper.performed += instance.OnRightBumper;
                @RightBumper.canceled += instance.OnRightBumper;
                @LeftSelectCog.started += instance.OnLeftSelectCog;
                @LeftSelectCog.performed += instance.OnLeftSelectCog;
                @LeftSelectCog.canceled += instance.OnLeftSelectCog;
                @RightSelectCog.started += instance.OnRightSelectCog;
                @RightSelectCog.performed += instance.OnRightSelectCog;
                @RightSelectCog.canceled += instance.OnRightSelectCog;
                @EastButton.started += instance.OnEastButton;
                @EastButton.performed += instance.OnEastButton;
                @EastButton.canceled += instance.OnEastButton;
                @SouthButton.started += instance.OnSouthButton;
                @SouthButton.performed += instance.OnSouthButton;
                @SouthButton.canceled += instance.OnSouthButton;
                @SelectPushed.started += instance.OnSelectPushed;
                @SelectPushed.performed += instance.OnSelectPushed;
                @SelectPushed.canceled += instance.OnSelectPushed;
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
                @Left.started += instance.OnLeft;
                @Left.performed += instance.OnLeft;
                @Left.canceled += instance.OnLeft;
                @Right.started += instance.OnRight;
                @Right.performed += instance.OnRight;
                @Right.canceled += instance.OnRight;
            }
        }
    }
    public InventoryControlsActions @InventoryControls => new InventoryControlsActions(this);
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
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnActivateHook(InputAction.CallbackContext context);
        void OnElecArm(InputAction.CallbackContext context);
        void OnActivateMagArm(InputAction.CallbackContext context);
    }
    public interface IInventoryControlsActions
    {
        void OnLeftBumper(InputAction.CallbackContext context);
        void OnRightBumper(InputAction.CallbackContext context);
        void OnLeftSelectCog(InputAction.CallbackContext context);
        void OnRightSelectCog(InputAction.CallbackContext context);
        void OnEastButton(InputAction.CallbackContext context);
        void OnSouthButton(InputAction.CallbackContext context);
        void OnSelectPushed(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
        void OnLeft(InputAction.CallbackContext context);
        void OnRight(InputAction.CallbackContext context);
    }
}
