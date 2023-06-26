//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Batu/Scripts/RobotControl/inputActions/RobotInputManager.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @RobotInputManager: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @RobotInputManager()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""RobotInputManager"",
    ""maps"": [
        {
            ""name"": ""RobotControl"",
            ""id"": ""f5cea6b4-d3d7-4f5e-a981-61c18dddcb1a"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""ecc08609-674c-40f9-9e57-b75d3b8717a1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Descend"",
                    ""type"": ""Value"",
                    ""id"": ""c893d300-e171-4f43-b0a9-7041ea16d2fc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Ascend"",
                    ""type"": ""Value"",
                    ""id"": ""dc7938f4-b9be-4e3a-a40e-c977a629fb32"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ThirdPersonLook"",
                    ""type"": ""Value"",
                    ""id"": ""c1f7d7f8-ac7d-4202-90d4-f9a5ea65b569"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""a1272361-2995-4a28-a6f5-586e99de4f30"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""722d4179-69ae-4e15-83ed-abd378867b71"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d14d92ed-1d11-4dff-99a3-929b0c07cd3f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b78e8f4f-5284-4c1b-986d-28502e7f6260"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5453af4a-3f23-4314-96cf-0cf5e40edb54"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GamepadLeftStick"",
                    ""id"": ""07af3580-14a1-4aff-9e69-5120be9832d6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ea311bd4-c2e8-456f-b37b-1250eff8e9bf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4b38ee36-39f8-4072-a21f-1e39d3b3c1f6"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d02d8973-5cb7-4066-bb3a-664ee1340197"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2b42c6cc-c3eb-4c9a-a06b-56448c915f1d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""747aeb0c-cb71-4162-a59b-db82669aa323"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Descend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b761e78-88ed-4e06-917c-061e6a817cb3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Descend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ba481cc-bfe8-4a16-81d2-947eadd06165"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ascend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f53b0f4d-7bec-4b13-b6a0-d67845bd3f3b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ascend"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37909551-492d-4c76-87f8-98259fcae662"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThirdPersonLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa52411-5090-4032-b7f3-95c50a32eeee"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThirdPersonLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // RobotControl
        m_RobotControl = asset.FindActionMap("RobotControl", throwIfNotFound: true);
        m_RobotControl_Movement = m_RobotControl.FindAction("Movement", throwIfNotFound: true);
        m_RobotControl_Descend = m_RobotControl.FindAction("Descend", throwIfNotFound: true);
        m_RobotControl_Ascend = m_RobotControl.FindAction("Ascend", throwIfNotFound: true);
        m_RobotControl_ThirdPersonLook = m_RobotControl.FindAction("ThirdPersonLook", throwIfNotFound: true);
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

    // RobotControl
    private readonly InputActionMap m_RobotControl;
    private List<IRobotControlActions> m_RobotControlActionsCallbackInterfaces = new List<IRobotControlActions>();
    private readonly InputAction m_RobotControl_Movement;
    private readonly InputAction m_RobotControl_Descend;
    private readonly InputAction m_RobotControl_Ascend;
    private readonly InputAction m_RobotControl_ThirdPersonLook;
    public struct RobotControlActions
    {
        private @RobotInputManager m_Wrapper;
        public RobotControlActions(@RobotInputManager wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_RobotControl_Movement;
        public InputAction @Descend => m_Wrapper.m_RobotControl_Descend;
        public InputAction @Ascend => m_Wrapper.m_RobotControl_Ascend;
        public InputAction @ThirdPersonLook => m_Wrapper.m_RobotControl_ThirdPersonLook;
        public InputActionMap Get() { return m_Wrapper.m_RobotControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RobotControlActions set) { return set.Get(); }
        public void AddCallbacks(IRobotControlActions instance)
        {
            if (instance == null || m_Wrapper.m_RobotControlActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RobotControlActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Descend.started += instance.OnDescend;
            @Descend.performed += instance.OnDescend;
            @Descend.canceled += instance.OnDescend;
            @Ascend.started += instance.OnAscend;
            @Ascend.performed += instance.OnAscend;
            @Ascend.canceled += instance.OnAscend;
            @ThirdPersonLook.started += instance.OnThirdPersonLook;
            @ThirdPersonLook.performed += instance.OnThirdPersonLook;
            @ThirdPersonLook.canceled += instance.OnThirdPersonLook;
        }

        private void UnregisterCallbacks(IRobotControlActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Descend.started -= instance.OnDescend;
            @Descend.performed -= instance.OnDescend;
            @Descend.canceled -= instance.OnDescend;
            @Ascend.started -= instance.OnAscend;
            @Ascend.performed -= instance.OnAscend;
            @Ascend.canceled -= instance.OnAscend;
            @ThirdPersonLook.started -= instance.OnThirdPersonLook;
            @ThirdPersonLook.performed -= instance.OnThirdPersonLook;
            @ThirdPersonLook.canceled -= instance.OnThirdPersonLook;
        }

        public void RemoveCallbacks(IRobotControlActions instance)
        {
            if (m_Wrapper.m_RobotControlActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRobotControlActions instance)
        {
            foreach (var item in m_Wrapper.m_RobotControlActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RobotControlActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RobotControlActions @RobotControl => new RobotControlActions(this);
    public interface IRobotControlActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnDescend(InputAction.CallbackContext context);
        void OnAscend(InputAction.CallbackContext context);
        void OnThirdPersonLook(InputAction.CallbackContext context);
    }
}
