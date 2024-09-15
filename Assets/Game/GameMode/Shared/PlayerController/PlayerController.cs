using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isLocalPlayer;
    public bool IsLocalPlayer => isLocalPlayer;

    // Character
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
    private bool canMoveOnThisTile = false;

    // Spell 
    private SpellData currentSpellSelected;

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
    private InputAction spaceBar;

    public Action<InputAction.CallbackContext> OnLeftClick;
    public Action<InputAction.CallbackContext> OnRightClick;
    public Action<InputAction.CallbackContext> OnShortcut_01;
    public Action<InputAction.CallbackContext> OnShortcut_02;
    public Action<InputAction.CallbackContext> OnShortcut_03;
    public Action<InputAction.CallbackContext> OnShortcut_04;
    public Action<InputAction.CallbackContext> OnShortcut_05;
    public Action<InputAction.CallbackContext> OnCtrlBar;
    public Action<InputAction.CallbackContext> OnShiftBar;
    public Action<InputAction.CallbackContext> OnSpaceBar;
    #endregion

    private void Awake()
    {
        if (!isLocalPlayer) return;
        DontDestroyOnLoad(gameObject);
        InitActionAssets();
    }
    private void Start()
    {
        if (!isLocalPlayer) return;
        AssignInputActions();
        AssignInputActivations();
        UpdateHUDUI();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (onFight)
        {
            UpdateFight();
        }
        else
        {
            UpdateHoverworld();
        }
    }

    private void UpdateHoverworld()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;
        if(Physics.Raycast(_ray, out _hit))
        {
            if(_hit.collider.TryGetComponent(out FightData _fightData))
            {
                GameManager.I.LaunchFightGameMode(_fightData);
            }
        }
    }

    private void UpdateFight()
    {
        FightMapTile _tile = HoverTileUnderMouse();
        if (FightMapManager.I.lastTileHovered != _tile)
        {
            FightMapManager.I.lastTileHovered = _tile;

            if (_tile != null)
            {
                bool _highlight = false;
                if (character.isMyTurn)
                {
                    // Show PM Path
                    if (currentSpellSelected == null && _tile.IsWalkable && !_tile.IsOccupied)
                    {
                        if (FightMapManager.I.DistanceBetweenTiles(character.CurrentTile, _tile) <= character.CurrentData.currentMovementPoints)
                        {
                            ShowPMPath(_tile);
                            _highlight = true;
                        }
                    }
                    // Show Hover Highlight Tile
                    else if (currentSpellSelected == null && _tile.IsWalkable)
                    {
                        HoverHighlightTile(_tile);
                        _highlight = true;
                    }
                    else if (currentSpellSelected != null)
                    {
                        if (FightMapManager.I.IsTileInRange(character.CurrentTile, _tile, currentSpellSelected.rangeMin, currentSpellSelected.rangeMax, currentSpellSelected.isLignOfSight))
                            FightMapManager.I.ColorHighlightTiles(new List<FightMapTile> { _tile }, Colors.I.SpellHighlightHover);
                        else
                            FightMapManager.I.ColorHighlightTiles(new List<FightMapTile> { }, Colors.I.SpellHighlightSightless);
                    }
                }
                // Show PM Path of Hover Character
                if (currentSpellSelected == null && _tile.character != null)
                {
                    HoverHighlightPMOfCharacter(_tile);
                    _highlight = true;
                }
                if (!_highlight && currentSpellSelected == null)
                {
                    FightMapManager.I.HideHighlightTiles();
                }
            }
        }
    }

    private void HoverHighlightPMOfCharacter(FightMapTile _tile)
    {
        FightMapManager.I.HideHighlightTiles();
        int _pmMax = _tile.character.CurrentData.currentMovementPoints;
        List<FightMapTile> _rangeTiles = FightMapManager.I.GetTilesByRange(_tile, 1, _pmMax, true);
        FightMapManager.I.ShowHighlightTiles(_rangeTiles, Colors.I.PMPathHoverCharacter);
    }

    private void HoverHighlightTile(FightMapTile _tile)
    {
        FightMapManager.I.ShowHighlightTiles(new List<FightMapTile> { _tile }, Colors.I.HoverHighlight);
    }

    private void ShowPMPath(FightMapTile _tile)
    {
        List<FightMapTile> _tiles = AStar.FindPath(character.CurrentTile, _tile);
        if (_tiles != null && _tiles.Count > 0 && _tiles.Count <= character.CurrentData.currentMovementPoints)
        {
            canMoveOnThisTile = true;
            FightMapManager.I.ShowHighlightTiles(_tiles, Colors.I.PMPath);
        }
        else
        {
            canMoveOnThisTile = false;
            FightMapManager.I.HideHighlightTiles();
        }
    }

    private FightMapTile HoverTileUnderMouse()
    {
        return GetTileUnderMouseWithRaycast();
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
        spaceBar = actionAsset.asset.FindAction("SpaceBar");
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
        spaceBar.performed += _context => InputActivation(OnSpaceBar, _context);
    }

    private void AssignInputActivations()
    {
        OnLeftClick += context => LeftClickAction(context);
        OnRightClick += context => RightClickAction(context);
        OnShortcut_01 += context => ActionSelectionSpell(context, 0);
        OnShortcut_02 += context => ActionSelectionSpell(context, 1);
        OnShortcut_03 += context => ActionSelectionSpell(context, 2);
        OnSpaceBar += context => SpaceBarAction();
    }

    private void SpaceBarAction()
    {
        if (onFight)
        {
            if (!isReadyToFight)
            {
                ReadyToFight();
            }
            else if (character != null && character.isMyTurn)
            {
                FightManager.I.EndTurn(character);
            }
        }
    }

    private void LeftClickAction(InputAction.CallbackContext _context)
    {
        if (onFight)
        {
            FightMapTile _tile = GetTileUnderMouseWithRaycast();
            if (_tile == null) return;
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
        }
    }

    private void RightClickAction(InputAction.CallbackContext _context)
    {
        if (lockOnFight && currentSpellSelected != null) ActionSelectionSpell(_context, -1);
    }

    public void SetCharacter(Character _character)
    {
        character = _character;
        character.isHumanController = true;
    }

    private void InputActivation(Action<InputAction.CallbackContext> _action, InputAction.CallbackContext _context)
    {
        _action?.Invoke(_context);
        UpdateHUDUI();
    }

    private void ActionSelectionSpell(InputAction.CallbackContext _context = default, int _spellIndex = -1)
    {
        if (lockOnFight) SelectionSpell(_spellIndex);
    }

    public void SelectionSpell(int _spellIndex = -1)
    {
        if (character == null) return;
        if (_spellIndex != -1 && character.Spells[_spellIndex].apCost <= character.CurrentData.currentActionPoints)
        {
            currentSpellSelected = currentSpellSelected == character.Spells[_spellIndex] ? null : character.Spells[_spellIndex];
            List<FightMapTile> _rangeTiles = GetTilesFromSpellSelectedRange();
            FightMapManager.I.ShowHighlightTiles(_rangeTiles, Colors.I.SpellHighlight);
            if (currentSpellSelected.isLignOfSight)
            {
                foreach (var _tile in _rangeTiles)
                {
                    if (_tile != null && !FightMapManager.I.LineOfSight(character.CurrentTile, _tile))
                        _tile.DisplayHighlight(true, Colors.I.SpellHighlightSightless);
                }
            }
        }
        else
        {
            currentSpellSelected = null;
            FightMapManager.I?.HideColorHighlightTiles();
            FightMapManager.I?.HideHighlightTiles();
        }
    }

    private FightMapTile GetTileUnderMouseWithRaycast()
    {
        FightMapTile _tileToReturn = null;
        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.TryGetComponent(out FightMapTile _tile))
            {
                // Debug.Log(_tile.character ? _tile.character.CharacterName : "No character");
                // Debug.Log("ID: " + _tile.tileID + " | matrixPosition: " + _tile.MatrixPosition);
                _tileToReturn = _tile;
            }
        }
        return _tileToReturn;
    }

    private void MoveOnTile(FightMapTile _tile)
    {
        if (character.isMyTurn && canMoveOnThisTile && _tile.IsWalkable && !_tile.IsOccupied)
        {
            if (character.CurrentData.currentMovementPoints > 0)
            {
                int _tileDistance = FightMapManager.I != null ? FightMapManager.I.DistanceBetweenTiles(character.CurrentTile, _tile) : -1;
                if (_tileDistance != -1 && _tileDistance <= character.CurrentData.currentMovementPoints)
                {
                    character.CurrentData.currentMovementPoints -= _tileDistance;
                    FightMapManager.I?.SwitchTileCharacter(Character, _tile);
                    character.UpdateAllUI();
                    FightMapManager.I?.HideHighlightTiles();
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
                if (character.CurrentData.currentActionPoints >= currentSpellSelected.apCost)
                {
                    if (FightMapManager.I.IsTileInRange(character.CurrentTile, _tile, currentSpellSelected.rangeMin, currentSpellSelected.rangeMax, currentSpellSelected.isLignOfSight))
                    {
                        character.CurrentData.currentActionPoints -= currentSpellSelected.apCost;
                        FightManager.I?.CastSpell(currentSpellSelected, _tile);
                        character.UpdateAllUI();
                    }
                }
                currentSpellSelected = null;
                FightMapManager.I?.HideHighlightTiles();
                FightMapManager.I?.HideColorHighlightTiles();
            }
        }
    }

    private List<FightMapTile> GetTilesFromSpellSelectedRange()
    {
        List<FightMapTile> _rangeTiles = new();
        if (currentSpellSelected != null)
        {
            FightMapTile _centerTile = character.CurrentTile;
            int rangeMin = currentSpellSelected.rangeMin;
            int rangeMax = currentSpellSelected.rangeMax;
            _rangeTiles = FightMapManager.I?.GetTilesByRange(_centerTile, rangeMin, rangeMax);
            return _rangeTiles;
        }
        return _rangeTiles;
    }
    private void SwitchCharacterPositionOnTile(FightMapTile _tile)
    {
        if (_tile.IsStartTile && _tile.TeamId == Character.CurrentTile.TeamId)
        {
            FightMapManager.I?.SwitchTileCharacter(Character, _tile);
        }
    }

    internal void ReadyToFight()
    {
        isReadyToFight = true;
        OnPlayerReady?.Invoke();
    }

    internal void StartFight()
    {
        lockOnFight = true;
        character.InitSpellBar(this);
        InitCharacterActions();
    }
    internal void EndFight()
    {
        isReadyToFight = false;
        lockOnFight = false;
        onFight = false;
    }
    internal void StartTurn()
    {

    }
    internal void EndTurn()
    {
        UpdateHUDUI();
    }

    private void InitCharacterActions()
    {
        character.OnTakeDamage += UpdateHUDUI;
        character.OnStartTurn += StartTurn;
        character.OnEndTurn += EndTurn;
    }

    internal void UpdateHUDUI()
    {
        if (character != null)
            CharacterDataUIManager.I?.SetHudValues(onFight, character.CurrentData.currentHealth, character.CurrentData.currentActionPoints, character.CurrentData.currentMovementPoints);
    }
}