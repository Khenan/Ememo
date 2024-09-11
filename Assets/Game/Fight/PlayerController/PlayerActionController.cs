//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Game/Fight/PlayerController/PlayerActionController.inputactions
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

public partial class @PlayerActionController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActionController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerActionController"",
    ""maps"": [
        {
            ""name"": ""InputActionPlayer"",
            ""id"": ""28511f42-69c0-49c8-9bf3-1898bfa14734"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""cc01ca8b-697e-4edc-bbd6-3c317d9b297b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shortcut_1"",
                    ""type"": ""Button"",
                    ""id"": ""ceb49d25-a0ed-4fa9-b3da-6a4af449e705"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shortcut_2"",
                    ""type"": ""Button"",
                    ""id"": ""cc02332f-c886-413a-ac65-6551f65d8a74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shortcut_3"",
                    ""type"": ""Button"",
                    ""id"": ""307a440a-12ee-4201-8cd3-adda7e24da6f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shortcut_4"",
                    ""type"": ""Button"",
                    ""id"": ""0e984f10-e88c-4433-a520-b77b71fd15cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shortcut_5"",
                    ""type"": ""Button"",
                    ""id"": ""1b74333a-ca8c-4827-bea9-e7b36e7095ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CtrlBar"",
                    ""type"": ""Button"",
                    ""id"": ""e10a552b-84a5-42ec-b7ed-4736ff241f3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ShiftBar"",
                    ""type"": ""Button"",
                    ""id"": ""e1dd14e6-ba60-45bd-8716-7e43ebddb1d3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""70f71db8-d372-4f3b-b706-8aaf8817f32d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d58827a1-43b4-4f7a-ac05-99088033ad63"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e934b84-4d68-41c0-a82c-1475c56b4c26"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Shortcut_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fee81eb9-d064-4afa-81c7-c2d3875054d4"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Shortcut_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a86d0923-77bd-4ca9-b967-4e272a759d6e"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Shortcut_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""052ce6d8-fce7-40f7-851e-1e5256360a9a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Shortcut_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""454b562c-8b5c-4d7f-af0a-914804913dda"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""Shortcut_5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1d8fbb2-ceb7-4d4a-9c0a-83bd156bab40"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""CtrlBar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16e835d5-31b5-4731-a578-636d5a9860dd"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""ShiftBar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b207a353-a4b0-4204-b2ba-062ad75c39f5"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard/Mouse"",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard/Mouse"",
            ""bindingGroup"": ""Keyboard/Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // InputActionPlayer
        m_InputActionPlayer = asset.FindActionMap("InputActionPlayer", throwIfNotFound: true);
        m_InputActionPlayer_LeftClick = m_InputActionPlayer.FindAction("LeftClick", throwIfNotFound: true);
        m_InputActionPlayer_Shortcut_1 = m_InputActionPlayer.FindAction("Shortcut_1", throwIfNotFound: true);
        m_InputActionPlayer_Shortcut_2 = m_InputActionPlayer.FindAction("Shortcut_2", throwIfNotFound: true);
        m_InputActionPlayer_Shortcut_3 = m_InputActionPlayer.FindAction("Shortcut_3", throwIfNotFound: true);
        m_InputActionPlayer_Shortcut_4 = m_InputActionPlayer.FindAction("Shortcut_4", throwIfNotFound: true);
        m_InputActionPlayer_Shortcut_5 = m_InputActionPlayer.FindAction("Shortcut_5", throwIfNotFound: true);
        m_InputActionPlayer_CtrlBar = m_InputActionPlayer.FindAction("CtrlBar", throwIfNotFound: true);
        m_InputActionPlayer_ShiftBar = m_InputActionPlayer.FindAction("ShiftBar", throwIfNotFound: true);
        m_InputActionPlayer_MousePosition = m_InputActionPlayer.FindAction("MousePosition", throwIfNotFound: true);
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

    // InputActionPlayer
    private readonly InputActionMap m_InputActionPlayer;
    private List<IInputActionPlayerActions> m_InputActionPlayerActionsCallbackInterfaces = new List<IInputActionPlayerActions>();
    private readonly InputAction m_InputActionPlayer_LeftClick;
    private readonly InputAction m_InputActionPlayer_Shortcut_1;
    private readonly InputAction m_InputActionPlayer_Shortcut_2;
    private readonly InputAction m_InputActionPlayer_Shortcut_3;
    private readonly InputAction m_InputActionPlayer_Shortcut_4;
    private readonly InputAction m_InputActionPlayer_Shortcut_5;
    private readonly InputAction m_InputActionPlayer_CtrlBar;
    private readonly InputAction m_InputActionPlayer_ShiftBar;
    private readonly InputAction m_InputActionPlayer_MousePosition;
    public struct InputActionPlayerActions
    {
        private @PlayerActionController m_Wrapper;
        public InputActionPlayerActions(@PlayerActionController wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_InputActionPlayer_LeftClick;
        public InputAction @Shortcut_1 => m_Wrapper.m_InputActionPlayer_Shortcut_1;
        public InputAction @Shortcut_2 => m_Wrapper.m_InputActionPlayer_Shortcut_2;
        public InputAction @Shortcut_3 => m_Wrapper.m_InputActionPlayer_Shortcut_3;
        public InputAction @Shortcut_4 => m_Wrapper.m_InputActionPlayer_Shortcut_4;
        public InputAction @Shortcut_5 => m_Wrapper.m_InputActionPlayer_Shortcut_5;
        public InputAction @CtrlBar => m_Wrapper.m_InputActionPlayer_CtrlBar;
        public InputAction @ShiftBar => m_Wrapper.m_InputActionPlayer_ShiftBar;
        public InputAction @MousePosition => m_Wrapper.m_InputActionPlayer_MousePosition;
        public InputActionMap Get() { return m_Wrapper.m_InputActionPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InputActionPlayerActions set) { return set.Get(); }
        public void AddCallbacks(IInputActionPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_InputActionPlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_InputActionPlayerActionsCallbackInterfaces.Add(instance);
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @Shortcut_1.started += instance.OnShortcut_1;
            @Shortcut_1.performed += instance.OnShortcut_1;
            @Shortcut_1.canceled += instance.OnShortcut_1;
            @Shortcut_2.started += instance.OnShortcut_2;
            @Shortcut_2.performed += instance.OnShortcut_2;
            @Shortcut_2.canceled += instance.OnShortcut_2;
            @Shortcut_3.started += instance.OnShortcut_3;
            @Shortcut_3.performed += instance.OnShortcut_3;
            @Shortcut_3.canceled += instance.OnShortcut_3;
            @Shortcut_4.started += instance.OnShortcut_4;
            @Shortcut_4.performed += instance.OnShortcut_4;
            @Shortcut_4.canceled += instance.OnShortcut_4;
            @Shortcut_5.started += instance.OnShortcut_5;
            @Shortcut_5.performed += instance.OnShortcut_5;
            @Shortcut_5.canceled += instance.OnShortcut_5;
            @CtrlBar.started += instance.OnCtrlBar;
            @CtrlBar.performed += instance.OnCtrlBar;
            @CtrlBar.canceled += instance.OnCtrlBar;
            @ShiftBar.started += instance.OnShiftBar;
            @ShiftBar.performed += instance.OnShiftBar;
            @ShiftBar.canceled += instance.OnShiftBar;
            @MousePosition.started += instance.OnMousePosition;
            @MousePosition.performed += instance.OnMousePosition;
            @MousePosition.canceled += instance.OnMousePosition;
        }

        private void UnregisterCallbacks(IInputActionPlayerActions instance)
        {
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @Shortcut_1.started -= instance.OnShortcut_1;
            @Shortcut_1.performed -= instance.OnShortcut_1;
            @Shortcut_1.canceled -= instance.OnShortcut_1;
            @Shortcut_2.started -= instance.OnShortcut_2;
            @Shortcut_2.performed -= instance.OnShortcut_2;
            @Shortcut_2.canceled -= instance.OnShortcut_2;
            @Shortcut_3.started -= instance.OnShortcut_3;
            @Shortcut_3.performed -= instance.OnShortcut_3;
            @Shortcut_3.canceled -= instance.OnShortcut_3;
            @Shortcut_4.started -= instance.OnShortcut_4;
            @Shortcut_4.performed -= instance.OnShortcut_4;
            @Shortcut_4.canceled -= instance.OnShortcut_4;
            @Shortcut_5.started -= instance.OnShortcut_5;
            @Shortcut_5.performed -= instance.OnShortcut_5;
            @Shortcut_5.canceled -= instance.OnShortcut_5;
            @CtrlBar.started -= instance.OnCtrlBar;
            @CtrlBar.performed -= instance.OnCtrlBar;
            @CtrlBar.canceled -= instance.OnCtrlBar;
            @ShiftBar.started -= instance.OnShiftBar;
            @ShiftBar.performed -= instance.OnShiftBar;
            @ShiftBar.canceled -= instance.OnShiftBar;
            @MousePosition.started -= instance.OnMousePosition;
            @MousePosition.performed -= instance.OnMousePosition;
            @MousePosition.canceled -= instance.OnMousePosition;
        }

        public void RemoveCallbacks(IInputActionPlayerActions instance)
        {
            if (m_Wrapper.m_InputActionPlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IInputActionPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_InputActionPlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_InputActionPlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public InputActionPlayerActions @InputActionPlayer => new InputActionPlayerActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard/Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IInputActionPlayerActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnShortcut_1(InputAction.CallbackContext context);
        void OnShortcut_2(InputAction.CallbackContext context);
        void OnShortcut_3(InputAction.CallbackContext context);
        void OnShortcut_4(InputAction.CallbackContext context);
        void OnShortcut_5(InputAction.CallbackContext context);
        void OnCtrlBar(InputAction.CallbackContext context);
        void OnShiftBar(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
    }
}
