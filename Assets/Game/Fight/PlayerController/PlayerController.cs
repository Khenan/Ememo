using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isLocalPlayer;

    [SerializeField] private Character characterToInstantiate;
    public Character CharacterToInstantiate => characterToInstantiate;
    private Character character;
    public Character Character => character;

    // Fight
    public bool onFight = false;
    internal bool lockOnFight = false;
    private bool isReadyToFight = false;
    public bool IsReadyToFight => isReadyToFight;
    public Action OnPlayerReady;

    #region Inputs
    private PlayerActionController actionAsset;

    private InputAction leftClick;
    private InputAction shortCut_1;
    private InputAction shortCut_2;
    private InputAction shortCut_3;
    private InputAction shortCut_4;
    private InputAction shortCut_5;
    private InputAction ctrlBar;
    private InputAction shiftBar;

    public Action<InputAction.CallbackContext> OnLeftClick;
    public Action<InputAction.CallbackContext> OnShortcut_01;
    public Action<InputAction.CallbackContext> OnShortcut_02;
    public Action<InputAction.CallbackContext> OnShortcut_03;
    public Action<InputAction.CallbackContext> OnShortcut_04;
    public Action<InputAction.CallbackContext> OnShortcut_05;
    public Action<InputAction.CallbackContext> OnCtrlBar;
    public Action<InputAction.CallbackContext> OnShiftBar;
    #endregion

    private void Awake()
    {
        InitActionAssets();
    }
    private void Start()
    {
        if (!isLocalPlayer) return;
        AssignInputActions();
        AssignInputActivations();
    }

    private void InitActionAssets()
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
    private void AssignInputActions()
    {
        actionAsset.Enable();

        leftClick.performed += _context => InputActivation(OnLeftClick, _context);
        shortCut_1.performed += _context => InputActivation(OnShortcut_01, _context);
        shortCut_2.performed += _context => InputActivation(OnShortcut_02, _context);
        shortCut_3.performed += _context => InputActivation(OnShortcut_03, _context);
        shortCut_4.performed += _context => InputActivation(OnShortcut_04, _context);
        shortCut_5.performed += _context => InputActivation(OnShortcut_05, _context);
        ctrlBar.performed += _context => InputActivation(OnCtrlBar, _context);
        shiftBar.performed += _context => InputActivation(OnShiftBar, _context);
    }
    private void AssignInputActivations()
    {
        OnLeftClick += context => GetTileUnderMouseWithRaycast(context);
    }

    public void SetCharacter(Character _character)
    {
        character = _character;
        character.isHumanController = true;
    }

    private void InputActivation(Action<InputAction.CallbackContext> _action, InputAction.CallbackContext _context)
    {
        _action?.Invoke(_context);
    }

    private FightMapTile GetTileUnderMouseWithRaycast(InputAction.CallbackContext _context)
    {
        FightMapManager.Instance.lastTileSelected = null;

        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.TryGetComponent(out FightMapTile _tile))
            {
                // Debug.Log(_tile.character ? _tile.character.CharacterName : "No character");
                // Debug.Log("ID: " + _tile.tileID + " | matrixPosition: " + _tile.MatrixPosition);

                if (!lockOnFight) SwitchCharacterPositionOnTile(_tile);

                FightMapManager.Instance.lastTileSelected = _tile;
                return _tile;
            }
        }
        return null;
    }

    private void SwitchCharacterPositionOnTile(FightMapTile _tile)
    {
        if (_tile.IsStartTile && _tile.TeamId == Character.CurrentTile.TeamId)
        {
            FightMapManager.Instance.SwitchTileCharacter(Character, _tile);
        }
    }

    internal void ReadyToFight()
    {
        isReadyToFight = true;
        OnPlayerReady?.Invoke();
    }

    internal void EndFight()
    {
        isReadyToFight = false;
    }
}