using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FightMapManager : Singleton<FightMapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<FightMap> maps;
    private Camera cam;
    [SerializeField] private FightMapTile floor, wall, hole;
    public FightMapTile lastTileHovered;
    private List<FightMapTile> lastTilesHighlighted = new();

    private FightMap currentMap;

    private class OldHighlight
    {
        public List<FightMapTile> tiles = new();
        public List<Color> colors = new();
    }

    private OldHighlight oldHardHighlights = new();

    private void SetCameraPosition()
    {
        if(cam == null)
        {
            cam = Camera.main;
        }
        cam.transform.position = new Vector3(23, 13, -10);
        cam.transform.rotation = Quaternion.Euler(30, -45, 0);
    }

    // private void GenerateMap(int _sizeX, int _sizeZ)
    // {
    //     for (int x = 0; x<_sizeX; x++)
    //     {
    //         for (int z = 0; z<_sizeZ; z++)
    //         {
    //             Instantiate(floor, new Vector3(x, 0, z), Quaternion.identity);
    //         }
    //     }
    // }

    internal FightMap GetMap(int _areaId)
    {
        List<FightMap> possibleMaps = maps.FindAll(map => map.areaId == _areaId);
        return possibleMaps[Random.Range(0, possibleMaps.Count)];
    }

    public FightMap InitMap(int _areaId)
    {
        List<FightMap> possibleMaps = maps.FindAll(map => map.areaId == _areaId);
        currentMap = Instantiate(possibleMaps[Random.Range(0, possibleMaps.Count)]);
        FightManager.I.AddGarbage(currentMap.gameObject);
        currentMap.Init();
        SetMapColor(currentMap);
        ShowStartTiles(currentMap);
        SetCameraPosition();
        return currentMap;
    }

    private void SetMapColor(FightMap _map)
    {
        List<FightMapTile> _floorTiles = _map.GetWalkableTiles();
        foreach (FightMapTile _floorTile in _floorTiles)
        {
            _floorTile.ColorBaseTopByIndex();
        }
    }

    private void ShowStartTiles(FightMap _map)
    {
        List<FightMapTile> _startTiles = _map.GetStartTiles();
        Color _color;
        foreach (FightMapTile _startTile in _startTiles)
        {
            switch (_startTile.TeamId)
            {
                case 0:
                    _color = Colors.I.TeamColors.Count > 0 ? Colors.I.TeamColors[0] : Colors.I.DefaultHightlight;
                    _startTile.ColorTop(_color);
                    break;
                case 1:
                    _color = Colors.I.TeamColors.Count > 1 ? Colors.I.TeamColors[1] : Colors.I.DefaultHightlight;
                    _startTile.ColorTop(_color);
                    break;
                default:
                    Debug.LogError("Undefined team");
                    break;
            };
        }
    }

    #region Highlight
    private void ToggleHighlightList(List<FightMapTile> _tiles = null, bool _show = true, Color _color = default)
    {
        if (lastTilesHighlighted != null)
        {
            foreach (FightMapTile _tile in lastTilesHighlighted)
            {
                DisplayHighlightTile(_tile, false);
            }
        }
        if (_tiles != null)
        {
            foreach (FightMapTile _tile in _tiles)
            {
                DisplayHighlightTile(_tile, _show, _color);
            }
        }
        lastTilesHighlighted = _tiles;
    }

    private void DisplayHighlightTile(FightMapTile _tile, bool _show = true, Color _color = default, bool _withTips = false, TipsType _tipsType = TipsType.Default)
    {
        if (_tile != null) _tile.DisplayHighlight(_show, _color, _withTips, _tipsType);
    }
    public void ShowHighlightTiles(List<FightMapTile> _tiles, Color _color = default) => ToggleHighlightList(_tiles, true, _color);
    public void HideHighlightTiles(List<FightMapTile> _tiles) => ToggleHighlightList(_tiles, false);
    public void HideHighlightTiles() => ToggleHighlightList(null, false);
    public void ColorHighlightTiles(List<FightMapTile> _tiles, Color _color, bool _withTips = false, TipsType _tipsType = TipsType.Default)
    {
        // On redonne l'ancienne couleur aux anciens highlights
        if (oldHardHighlights.tiles.Count > 0)
        {
            for (int _i = 0; _i < oldHardHighlights.tiles.Count; _i++)
            {
                DisplayHighlightTile(oldHardHighlights.tiles[_i], true, oldHardHighlights.colors[_i], _withTips, _tipsType);
            }
            oldHardHighlights.tiles.Clear();
            oldHardHighlights.colors.Clear();
        }
        foreach (FightMapTile _tile in _tiles)
        {
            if (_tile.Highlight != null)
            {
                oldHardHighlights.tiles.Add(_tile);
                oldHardHighlights.colors.Add(_tile.Highlight.color);
                DisplayHighlightTile(_tile, true, _color);
            }
        }
    }
    public void HideColorHighlightTiles()
    {
        for (int _i = 0; _i < oldHardHighlights.tiles.Count; _i++)
        {
            DisplayHighlightTile(oldHardHighlights.tiles[_i], false, oldHardHighlights.colors[_i]);
        }
        oldHardHighlights.tiles.Clear();
        oldHardHighlights.colors.Clear();
    }
    #endregion

    public void StartFight()
    {
        currentMap.GetStartTiles().ForEach(tile => tile.HideStartTile());
    }

    internal void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        _map.GetTiles().Find(tile => tile.Position == _fightMapTile.Position).character = _character;
    }

    internal void SwitchTileCharacter(Character character, FightMapTile tile, bool _canSwitch = false)
    {
        FightMapTile _oldTile = character.CurrentTile;
        Character _oldCharacter = tile.character;

        if (!_canSwitch && _oldCharacter != null)
        {
            return;
        }

        character.CurrentTile = tile;
        tile.character = character;

        if (_canSwitch)
        {
            if (_oldCharacter != null)
            {
                _oldCharacter.CurrentTile = _oldTile;
            }
            _oldTile.character = _oldCharacter;
        }

        character.transform.position = tile.transform.position;

        if (_canSwitch)
        {
            if (_oldCharacter != null)
            {
                _oldCharacter.transform.position = _oldTile.transform.position;
            }
        }
        else
        {
            _oldTile.character = null;
        }
    }

    internal FightMapTile GetTileByMatrixPosition(Vector2 _matrixPosition)
    {
        FightMapTile _tile = null;
        if (_matrixPosition.x >= 0 && _matrixPosition.x < currentMap.Size.x && _matrixPosition.y >= 0 && _matrixPosition.y < currentMap.Size.y)
        {
            int indexPos = (int)_matrixPosition.x + (int)_matrixPosition.y * (int)currentMap.Size.x;
            if (indexPos >= 0 && indexPos < currentMap.GetMapTileCount())
                _tile = currentMap.GetTiles()[indexPos];
        }
        return _tile;
    }

    internal List<FightMapTile> GetTilesByRange(FightMapTile _centerTile, int _rangeMin, int _rangeMax, bool _canWalk = false)
    {
        List<FightMapTile> _rangeTiles = new();
        int _startX = (int)_centerTile.MatrixPosition.x;
        int _startY = (int)_centerTile.MatrixPosition.y;
        for (int _x = _startX - _rangeMax; _x <= _startX + _rangeMax; _x++)
        {
            for (int _y = _startY - _rangeMax; _y <= _startY + _rangeMax; _y++)
            {
                int _sum = Mathf.Abs(_x - _startX) + Mathf.Abs(_y - _startY);

                if (_sum >= _rangeMin && _sum <= _rangeMax)
                {
                    FightMapTile _tile = GetTileByMatrixPosition(new Vector2(_x, _y));
                    if (_tile != null && _canWalk ? !_tile.IsOccupied : true)
                    {
                        _rangeTiles.Add(_tile);
                    }
                }
            }
        }
        return _rangeTiles;
    }

    internal int DistanceBetweenTiles(FightMapTile currentTile, FightMapTile tile)
    {
        return (int)(Mathf.Abs(currentTile.MatrixPosition.x - tile.MatrixPosition.x) + Mathf.Abs(currentTile.MatrixPosition.y - tile.MatrixPosition.y));
    }

    internal bool IsTileInRange(FightMapTile _centerTile, FightMapTile _targetTile, int _rangeMin, int _rangeMax, bool _sight = false)
    {
        bool _valueToReturn = false;
        bool _isInRangeMin = DistanceBetweenTiles(_centerTile, _targetTile) >= _rangeMin;
        bool _isInRangeMax = DistanceBetweenTiles(_centerTile, _targetTile) <= _rangeMax;
        bool _isInRange = _isInRangeMin && _isInRangeMax;
        if (_isInRange && (_sight ? LineOfSight(_centerTile, _targetTile) : true))
        {
            _valueToReturn = true;
        }
        return _valueToReturn;
    }
    public bool LineOfSight(FightMapTile _centerTile, FightMapTile _targetTile)
    {
        if (_centerTile == null || _targetTile == null)
        {
            throw new ArgumentNullException("Les tuiles ne doivent pas être nulles è_é.");
        }

        Vector2 centerPos = _centerTile.MatrixPosition;
        Vector2 targetPos = _targetTile.MatrixPosition;

        int dx = (int)Mathf.Abs(targetPos.x - centerPos.x);
        int dy = (int)Mathf.Abs(targetPos.y - centerPos.y);

        int sx = centerPos.x < targetPos.x ? 1 : -1;
        int sy = centerPos.y < targetPos.y ? 1 : -1;

        int currentX = (int)centerPos.x;
        int currentY = (int)centerPos.y;

        int n = 1 + dx + dy;
        int error = dx - dy;
        dx *= 2;
        dy *= 2;

        for (; n > 0; --n)
        {
            if (!(currentX == (int)centerPos.x && currentY == (int)centerPos.y))
            {
                FightMapTile currentTile = GetTileByMatrixPosition(new Vector2(currentX, currentY));
                if (currentTile != null && currentTile.BlockLineOfSight)
                {
                    if (currentTile == _targetTile && currentTile.character != null)
                    {
                        return true;
                    }
                    return false;
                }
            }

            if (Mathf.Abs(dx) == Mathf.Abs(dy))
            {
                currentX += sx;
                currentY += sy;
                --n;
            }
            else
            {
                if (error > 0)
                {
                    currentX += sx;
                    error -= dy;
                }
                else
                {
                    currentY += sy;
                    error += dx;
                }
            }
        }

        return true;
    }
}