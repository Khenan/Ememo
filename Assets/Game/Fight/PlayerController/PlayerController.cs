using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerActionController actionAsset;

    [SerializeField] private InputAction leftClick;
    [SerializeField] private InputAction shortCut_1;
    [SerializeField] private InputAction shortCut_2;
    [SerializeField] private InputAction shortCut_3;
    [SerializeField] private InputAction shortCut_4;
    [SerializeField] private InputAction shortCut_5;
    [SerializeField] private InputAction ctrlBar;
    [SerializeField] private InputAction shiftBar;

    public Action<InputAction.CallbackContext> OnLeftClick;
    public Action<InputAction.CallbackContext> OnShortcut_01;
    public Action<InputAction.CallbackContext> OnShortcut_02;
    public Action<InputAction.CallbackContext> OnShortcut_03;
    public Action<InputAction.CallbackContext> OnShortcut_04;
    public Action<InputAction.CallbackContext> OnShortcut_05;
    public Action<InputAction.CallbackContext> OnCtrlBar;
    public Action<InputAction.CallbackContext> OnShiftBar;

    private void Awake()
    {
        actionAsset = new();
        leftClick = actionAsset.asset.FindAction("LeftClick");
        shortCut_1 = actionAsset.asset.FindAction("ShortCut_1");
        shortCut_2 = actionAsset.asset.FindAction("ShortCut_2");
        shortCut_3 = actionAsset.asset.FindAction("ShortCut_3");
        shortCut_4 = actionAsset.asset.FindAction("ShortCut_4");
        shortCut_5 = actionAsset.asset.FindAction("ShortCut_5");
        ctrlBar = actionAsset.asset.FindAction("CtrlBar");
        shiftBar = actionAsset.asset.FindAction("ShiftBar");
    }

    private void Start()
    {
        leftClick.performed += _context => InputActivation(OnLeftClick, _context);
        shortCut_1.performed += _context => InputActivation(OnShortcut_01, _context);
        shortCut_2.performed += _context => InputActivation(OnShortcut_02, _context);
        shortCut_3.performed += _context => InputActivation(OnShortcut_03, _context);
        shortCut_4.performed += _context => InputActivation(OnShortcut_04, _context);
        shortCut_5.performed += _context => InputActivation(OnShortcut_05, _context);
        ctrlBar.performed += _context => InputActivation(OnCtrlBar, _context);
        shiftBar.performed += _context => InputActivation(OnShiftBar, _context);
    }

    private void InputActivation(Action<InputAction.CallbackContext> _action, InputAction.CallbackContext _context)
    {
        _action?.Invoke(_context);
    }


}