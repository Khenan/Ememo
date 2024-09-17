using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isLocalPlayer;
    public bool IsLocalPlayer => isLocalPlayer;

    public Vector2Int WorlMapMatrixPosition;
    public Vector2Int WorldTileMatrixPositionBase;

    // Character
    [SerializeField] private Character characterToInstantiate;
    public Character CharacterToInstantiate => characterToInstantiate;
    private Character character;
    public Character Character => character;

    // Path
    private List<MapTile> nextPath;
    private bool newPathOnCurrentPath = false;
    private List<MapTile> currentPath;
    private int currentPathIndex = 0;
    private bool onMove = false;
    private bool directionCharacterVisualInit = false;
    private bool temporaryPositionEnable = false;
    private Vector3 temporaryPosition;
    private float currentMoveTimer = 0f;
    private float maxMoveTimer = 0.3f;

    // Fight
    public bool onFight = false;
    internal bool lockOnFight = false;
    private bool isReadyToFight = false;
    public bool IsReadyToFight => isReadyToFight;
    public Action OnPlayerReady;
    private bool canMoveOnThisTile = false;
    private int pmCountToMove = 0;

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
        GameManager.I.SetPlayerController(this);
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

        if (onFight) UpdateFight();
        else UpdateExploration();
    }

    private void UpdateFight()
    {
        FightMapTile _tile = HoverFightTileUnderMouse();
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
                        if (MapManager.I.DistanceBetweenTiles((FightMapTile)character.CurrentTile, _tile) <= character.CurrentData.currentMovementPoints)
                        {
                            ShowPMPathInFight(_tile);
                            _highlight = true;
                        }
                    }
                    else if (currentSpellSelected != null)
                    {
                        List<MapTile> _allTiles = GetAllTilesBetweenTwoTiles(character.CurrentTile, _tile);
                        if (MapManager.I.IsTileInRange(_allTiles, (FightMapTile)character.CurrentTile, _tile, currentSpellSelected.rangeMin, currentSpellSelected.rangeMax, currentSpellSelected.isLignOfSight))
                            FightMapManager.I.ColorHighlightTiles(new List<FightMapTile> { _tile }, Colors.I.SpellHighlightHover);
                        else
                            FightMapManager.I.ColorHighlightTiles(new List<FightMapTile> { }, Colors.I.SpellHighlightSightless);
                    }
                }
                // Show PM remaining of Hover Character
                if (currentSpellSelected == null && _tile.character != null)
                {
                    Debug.Log("_tile.character: " + _tile.character);
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

    private void UpdateExploration()
    {
        if (onMove && currentPath != null)
        {
            if (!directionCharacterVisualInit)
            {
                character.ChangeVisualDirection(currentPath[currentPathIndex]);
                directionCharacterVisualInit = true;
            }
            if (currentMoveTimer < maxMoveTimer)
            {
                float _speedMultiplier = 1;
                if (currentPathIndex < currentPath.Count - 1 && MapManager.I.IsDiagonal(currentPath[currentPathIndex], currentPath[currentPathIndex + 1]))
                {
                    _speedMultiplier = 1.4f;
                }

                currentMoveTimer += Time.deltaTime / _speedMultiplier;

                Vector3 _startPosition = temporaryPositionEnable ? temporaryPosition : character.CurrentTile.transform.position;
                Vector3 _nextPosition = currentPath[currentPathIndex].transform.position;
                character.transform.position = Vector3.Lerp(_startPosition, _nextPosition, currentMoveTimer / maxMoveTimer);
            }
            else
            {
                temporaryPositionEnable = false;
                currentMoveTimer = 0;
                SwitchTileCharacterOnExploTile(currentPath[currentPathIndex] as ExplorationMapTile);
                currentPathIndex++;
                if (newPathOnCurrentPath)
                {
                    currentPath = nextPath;
                    newPathOnCurrentPath = false;
                    currentPathIndex = 0;
                }

                if (currentPathIndex >= currentPath.Count)
                {
                    directionCharacterVisualInit = false;
                    currentPathIndex = 0;
                    currentMoveTimer = 0;

                    currentPath = null;
                    nextPath = null;
                    newPathOnCurrentPath = false;
                    onMove = false;
                }
                else
                {
                    // direction of character
                    if (character.CurrentTile.MatrixPositionWorld != currentPath[currentPathIndex].MatrixPositionWorld)
                        character.ChangeVisualDirection(currentPath[currentPathIndex]);
                }
            }
        }
    }

    private void HoverHighlightPMOfCharacter(FightMapTile _tile)
    {
        FightMapManager.I.HideHighlightTiles();
        int _currentPM = _tile.character.CurrentData.currentMovementPoints;

        List<MapTile> _allTiles = ConcatenatorMapList.ConcatenateMaps(FightManager.I.currentMaps.ConvertAll(_m => (Map)_m));
        Debug.Log("Before _rangeTiles.Count");
        List<FightMapTile> _rangeTiles = MapManager.I.GetTilesByRangeInTemporaryList(_allTiles, _tile, 1, _currentPM, true).ConvertAll(_t => (FightMapTile)_t);
        Debug.Log("_rangeTiles.Count: " + _rangeTiles.Count);
        _rangeTiles.Add(_tile);
        FightMapManager.I.ShowHighlightTiles(_rangeTiles, Colors.I.PMPathHoverCharacter);
    }

    private void ShowPMPathInFight(FightMapTile _tile)
    {
        if (character.CurrentTile == null || _tile == null || character.CurrentTile.MatrixPositionWorld == _tile.MatrixPositionWorld || character.CurrentData.currentMovementPoints == 0)
        {
            FightMapManager.I.HideHighlightTiles();
            return;
        }

        List<MapTile> _allTiles = GetAllTilesBetweenTwoTiles(character.CurrentTile, _tile);
        List<MapTile> _path = AStar.FindPath(_allTiles, character.CurrentTile, _tile);
        List<FightMapTile> _pathTiles = _path.ConvertAll(_t => (FightMapTile)_t);

        if (_pathTiles != null && _pathTiles.Count > 0 && _pathTiles.Count <= character.CurrentData.currentMovementPoints)
        {
            canMoveOnThisTile = true;
            pmCountToMove = _pathTiles.Count;
            FightMapManager.I.ShowHighlightTiles(_pathTiles, Colors.I.PMPath);
        }
        else
        {
            canMoveOnThisTile = false;
            pmCountToMove = 0;
            FightMapManager.I.HideHighlightTiles();
        }
    }

    private List<Map> GetMapsBetweenTwoMap(Vector2Int _currentMapMatrix, Vector2Int _targetMapMatrix)
    {
        List<Map> _mapsBetween = new();
        // Get all maps between the current map and the target map
        int _minX = Mathf.Min(_currentMapMatrix.x, _targetMapMatrix.x);
        int _maxX = Mathf.Max(_currentMapMatrix.x, _targetMapMatrix.x);
        int _minY = Mathf.Min(_currentMapMatrix.y, _targetMapMatrix.y);
        int _maxY = Mathf.Max(_currentMapMatrix.y, _targetMapMatrix.y);
        List<Map> _maps = null;
        if (onFight) _maps = FightManager.I.currentMaps.ConvertAll(_m => (Map)_m);
        else _maps = WorldMapManager.I.CurrentMaps;
        if (_maps != null)
            foreach (Map _map in _maps)
                if (_map.matrixPosition.x >= _minX && _map.matrixPosition.x <= _maxX && _map.matrixPosition.y >= _minY && _map.matrixPosition.y <= _maxY)
                    _mapsBetween.Add(_map);
        return _mapsBetween;
    }
    private List<MapTile> GetAllTilesBetweenTwoTiles(MapTile _currentTile, MapTile _targetTile)
    {
        Debug.Log("_currentTile: " + _currentTile);
        Debug.Log("_targetTile: " + _targetTile);
        List<Map> _maps = new();
        if (_currentTile.map.matrixPosition == _targetTile.map.matrixPosition)
        {
            _maps.Add(_currentTile.map);
        }
        else
        {
            _maps = GetMapsBetweenTwoMap(_currentTile.map.matrixPosition, _targetTile.map.matrixPosition);
        }
        List<MapTile> _allTiles = ConcatenatorMapList.ConcatenateMaps(_maps);
        return _allTiles;
    }

    private List<MapTile> GetPathInExploration(MapTile _startTile, MapTile _targetTile)
    {
        List<MapTile> _mapTiles = null;
        if (_startTile != null && _targetTile != null)
        {
            if (_startTile != _targetTile)
            {
                List<MapTile> _allTiles = GetAllTilesBetweenTwoTiles(_startTile, _targetTile);
                _mapTiles = AStar.FindPath(_allTiles, _startTile, _targetTile, true);
                Debug.Log(_mapTiles[^1], _mapTiles[^1]);
                Debug.Log(_mapTiles.Count);
            }
        }
        return _mapTiles;
    }

    private FightMapTile HoverFightTileUnderMouse()
    {
        return GetFightTileUnderMouseWithRaycast();
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
        if (onFight) FightLeftClickAction(_context);
        else ExplorationLeftClickAction(_context);
    }
    private void FightLeftClickAction(InputAction.CallbackContext _context)
    {
        FightMapTile _tile = GetFightTileUnderMouseWithRaycast();
        if (_tile == null) return;
        if (lockOnFight)
        {
            if (currentSpellSelected != null)
            {
                CastSpellOnTile(_tile);
            }
            else
            {
                MoveOnFightTile(_tile);
            }
        }
        else
        {
            SwitchCharacterPositionOnFightTile(_tile);
        }
    }
    private void ExplorationLeftClickAction(InputAction.CallbackContext _context)
    {
        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.TryGetComponent(out FightData _fightData))
            {
                ExplorationManager.I.GoToFight(_fightData);
            }
            else if (_hit.collider.TryGetComponent(out ExplorationMapTile _tile))
            {
                if (MapManager.I.lastTileHovered != _tile)
                {
                    MapManager.I.lastTileHovered = _tile;

                    if (_tile != null && _tile.IsWalkable && !_tile.IsOccupied)
                    {
                        List<MapTile> _path;
                        if (onMove) _path = GetPathInExploration(currentPath[currentPathIndex], _tile);
                        else _path = GetPathInExploration(character.CurrentTile, _tile);

                        if (_path != null && _path.Count > 0)
                        {
                            if (onMove)
                            {
                                temporaryPosition = character.transform.position;
                                temporaryPositionEnable = true;
                                directionCharacterVisualInit = false;
                                newPathOnCurrentPath = true;
                                nextPath = _path;
                            }
                            else
                            {
                                currentPath = _path;
                                currentPathIndex = 0;
                            }
                            onMove = true;
                        }
                    }
                }
            }
        }
    }

    private void RightClickAction(InputAction.CallbackContext _context)
    {
        if (lockOnFight && currentSpellSelected != null) ActionSelectionSpell(_context, -1);
    }

    public void SetCharacter(Character _character)
    {
        if (_character == null) return;

        if (character != null)
        {
            character.OnTakeDamage -= UpdateHUDUI;
            if (character.mode == CharacterMode.Fight)
            {
                character.OnStartTurn -= StartTurn;
                character.OnEndTurn -= EndTurn;
            }
        }
        character = _character;
        character.isHumanController = true;
        InitCharacterActions();

        // Camera
        CameraManager.I.SetTarget(character.transform);
    }

    private void InputActivation(Action<InputAction.CallbackContext> _action, InputAction.CallbackContext _context)
    {
        _action?.Invoke(_context);
        UpdateHUDUI();
    }

    #region Exploration
    private void SwitchTileCharacterOnExploTile(ExplorationMapTile _tile)
    {
        if (_tile.IsWalkable)
        {
            WorldTileMatrixPositionBase = _tile.MatrixPositionWorld;
            WorlMapMatrixPosition = _tile.map.matrixPosition;
            ExplorationManager.I.SwitchTileCharacter(Character, _tile, false);
        }
    }
    #endregion

    #region Fight

    private void ActionSelectionSpell(InputAction.CallbackContext _context = default, int _spellIndex = -1)
    {
        if (lockOnFight) SelectionSpell(_spellIndex);
    }

    public void SelectionSpell(int _spellIndex = -1)
    {
        if (character == null) return;
        if (_spellIndex != -1 && character.Spells[_spellIndex].apCost <= character.CurrentData.currentActionPoints)
        {
            currentSpellSelected = currentSpellSelected == character.Spells[_spellIndex] ? currentSpellSelected : character.Spells[_spellIndex];
            if (currentSpellSelected == null)
            {
                FightMapManager.I?.HideColorHighlightTiles();
                FightMapManager.I?.HideHighlightTiles();
            }
            List<FightMapTile> _rangeTiles = GetTilesFromSpellSelectedRange();
            FightMapManager.I.ShowHighlightTiles(_rangeTiles, Colors.I.SpellHighlight);
            if (currentSpellSelected.isLignOfSight)
            {
                foreach (var _tile in _rangeTiles)
                {
                    List<MapTile> _allTiles = GetAllTilesBetweenTwoTiles(character.CurrentTile, _tile);
                    if (_tile != null && !MapManager.I.LineOfSight(_allTiles, (FightMapTile)character.CurrentTile, _tile))
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

    private FightMapTile GetFightTileUnderMouseWithRaycast()
    {
        FightMapTile _tileToReturn = null;
        Ray _ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.TryGetComponent(out FightMapTile _tile))
            {
                _tileToReturn = _tile;
            }
        }
        return _tileToReturn;
    }


    private void MoveOnFightTile(FightMapTile _tile)
    {
        if (character.isMyTurn && canMoveOnThisTile && _tile.IsWalkable && !_tile.IsOccupied)
        {
            if (character.CurrentData.currentMovementPoints >= pmCountToMove)
            {
                int _tileDistance = pmCountToMove;
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
                    List<MapTile> _allTiles = GetAllTilesBetweenTwoTiles(character.CurrentTile, _tile);
                    if (MapManager.I.IsTileInRange(_allTiles, (FightMapTile)character.CurrentTile, _tile, currentSpellSelected.rangeMin, currentSpellSelected.rangeMax, currentSpellSelected.isLignOfSight))
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
        FightMapManager.I.HideHighlightTiles();
        List<FightMapTile> _rangeTiles = new();
        if (currentSpellSelected != null)
        {
            List<MapTile> _allTiles = ConcatenatorMapList.ConcatenateMaps(FightManager.I.currentMaps.ConvertAll(_m => (Map)_m));
            _rangeTiles = MapManager.I.GetTilesByRangeInTemporaryList(_allTiles, character.CurrentTile, currentSpellSelected.rangeMin, currentSpellSelected.rangeMax).ConvertAll(_t => (FightMapTile)_t);
            return _rangeTiles;
        }
        return _rangeTiles;
    }
    private void SwitchCharacterPositionOnFightTile(FightMapTile _tile)
    {
        FightMapTile _currentTile = (FightMapTile)character.CurrentTile;
        if (_tile.IsStartTile && _tile.TeamId == _currentTile.TeamId)
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
    #endregion

    private void InitCharacterActions()
    {
        character.OnTakeDamage += UpdateHUDUI;
        if (character.mode == CharacterMode.Fight)
        {
            character.OnStartTurn += StartTurn;
            character.OnEndTurn += EndTurn;
        }
    }

    internal void UpdateHUDUI()
    {
        if (character != null && character.CurrentData != null)
            CharacterDataUIManager.I?.SetHudValues(onFight, character.CurrentData.currentHealth, character.CurrentData.currentActionPoints, character.CurrentData.currentMovementPoints);
    }
}