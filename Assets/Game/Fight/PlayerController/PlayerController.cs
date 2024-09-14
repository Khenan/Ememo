using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    // Spell 
    private SpellData currentSpellSelected;

    private Dictionary<Direction, Vector2> lineDirection = new()
    {
        {Direction.Up, new(0, -1)},
        {Direction.Down, new(0, 1)},
        {Direction.Left, new(-1, 0)},
        {Direction.Right, new(1, 0)}
    };
    private Dictionary<Direction, Vector2> diagonaleDirection = new()
    {
        {Direction.Up, new(-1, -1)},
        {Direction.Down, new(1, 1)},
        {Direction.Left, new(-1, 1)},
        {Direction.Right, new(1, -1)}
    };

    #region Inputs
    private PlayerActionController actionAsset;

    private InputAction leftClick;
    private InputAction rightClick;
    private InputAction shortCut_1;
    private InputAction shortCut_2;
    private InputAction shortCut_3;
    private InputAction shortCut_4;
    private InputAction shortCut_5;
    private InputAction ctrlBar;
    private InputAction shiftBar;

    public Action<InputAction.CallbackContext> OnLeftClick;
    public Action<InputAction.CallbackContext> OnRightClick;
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
        rightClick = actionAsset.asset.FindAction("RightClick");
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
        rightClick.performed += _context => InputActivation(OnRightClick, _context);
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
        OnRightClick += context => RightClickAction(context);
        OnShortcut_01 += context => SelectionSpell(context, 0);
        OnShortcut_02 += context => SelectionSpell(context, 1);
    }



    private void RightClickAction(InputAction.CallbackContext _context)
    {
        SelectionSpell(_context, -1);
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

    private void SelectionSpell(InputAction.CallbackContext _context = default, int _spellIndex = -1)
    {
        if (_spellIndex != -1)
        {
            currentSpellSelected = currentSpellSelected == character.Spells[_spellIndex] ? null : character.Spells[_spellIndex];
        }
        else
        {
            currentSpellSelected = null;
            Debug.Log("Spell is NULL");
        }
        FightMapManager.I.ShowHighlightTiles(GetTilesFromSpellSelectedRange(), Colors.I.SpellHighlight);
    }

    private FightMapTile GetTileUnderMouseWithRaycast(InputAction.CallbackContext _context)
    {
        FightMapManager.I.lastTileSelected = null;

        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.TryGetComponent(out FightMapTile _tile))
            {
                // Debug.Log(_tile.character ? _tile.character.CharacterName : "No character");
                // Debug.Log("ID: " + _tile.tileID + " | matrixPosition: " + _tile.MatrixPosition);

                if (lockOnFight)
                {
                    if (currentSpellSelected != null)
                    {
                        CastSpellOnTile(_tile);
                    }
                    else
                    {
                        MoveOnTile(_tile);
                    }
                }
                else
                {
                    SwitchCharacterPositionOnTile(_tile);
                }

                FightMapManager.I.lastTileSelected = _tile;
                return _tile;
            }
        }
        return null;
    }

    private void MoveOnTile(FightMapTile _tile)
    {
        if (character.isMyTurn && _tile.IsWalkable && !_tile.IsOccupied)
        {
            if (character.CurrentData.currentMovementPoints > 0)
            {
                int _tileDistance = FightMapManager.I.DistanceBetweenTiles(character.CurrentTile, _tile);
                if (_tileDistance <= character.CurrentData.currentMovementPoints)
                {
                    character.CurrentData.currentMovementPoints -= _tileDistance;
                    FightMapManager.I.SwitchTileCharacter(Character, _tile);
                    character.UpdateAllUI();
                }
            }
        }
    }

    private void CastSpellOnTile(FightMapTile _tile)
    {
        if (character.isMyTurn && _tile.IsWalkable)
        {
            if (currentSpellSelected != null)
            {
                if (character.CurrentData.currentActionPoints >= currentSpellSelected.cost)
                {
                    character.CurrentData.currentActionPoints -= currentSpellSelected.cost;
                    FightManager.I.CastSpell(currentSpellSelected, _tile);
                    character.UpdateAllUI();
                }
                currentSpellSelected = null;
            }
        }
    }

    private List<FightMapTile> GetTilesFromSpellSelectedRange()
    {
        if (currentSpellSelected != null)
        {
            List<FightMapTile> _rangeTiles = new();
            FightMapTile _centerTile = character.CurrentTile;
            int rangeMin = currentSpellSelected.rangeMin;
            int rangeMax = currentSpellSelected.rangeMax;
            if (rangeMax <= 0)
            {
                _rangeTiles.Add(_centerTile);
                Debug.Log("Center Tile is Added");
                return _rangeTiles;
            }
            for (int rangeCurrent = rangeMin; rangeCurrent <= rangeMax; rangeCurrent++)
            {
                foreach (KeyValuePair<Direction, Vector2> dir in lineDirection)
                {
                    _rangeTiles.Add(FightMapManager.I.GetTileByMatrixPosition(_centerTile.MatrixPosition + (dir.Value * rangeCurrent)));
                }
                foreach (KeyValuePair<Direction, Vector2> dir in diagonaleDirection)
                {
                    _rangeTiles.Add(FightMapManager.I.GetTileByMatrixPosition(_centerTile.MatrixPosition + (dir.Value * (rangeCurrent - 1))));
                }
            }
            return _rangeTiles;
        }
        return null;
    }
    private void SwitchCharacterPositionOnTile(FightMapTile _tile)
    {
        if (_tile.IsStartTile && _tile.TeamId == Character.CurrentTile.TeamId)
        {
            FightMapManager.I.SwitchTileCharacter(Character, _tile);
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

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}