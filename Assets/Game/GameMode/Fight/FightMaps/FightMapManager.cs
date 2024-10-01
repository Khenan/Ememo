using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using Random = UnityEngine.Random;

public class FightMapManager : Singleton<FightMapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<FightMap> maps = new();
    [SerializeField] private FightMapTile floor, wall, hole;
    public FightMapTile lastTileHovered;
    private List<FightMapTile> lastTilesHighlighted = new();

    private List<FightMap> currentMaps = new();

    private class OldHighlight
    {
        public List<FightMapTile> tiles = new();
        public List<Color> colors = new();
    }

    private OldHighlight oldHardHighlights = new();

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

    internal FightMap GetMapPrefab(int _areaId)
    {
        List<FightMap> possibleMaps = maps.FindAll(map => map.areaId == _areaId);
        return possibleMaps[Random.Range(0, possibleMaps.Count)];
    }

    public FightMap InitMap(FightRoom _fightRoom, int _areaId)
    {
        Debug.Log("_areaId: " + _areaId);
        Debug.Log("maps.Count: " + maps.Count);
        List<FightMap> possibleMaps = maps.FindAll(_map => _map.areaId == _areaId);
        Debug.Log("possibleMaps.Count: " + possibleMaps.Count);
        FightMap _map = Instantiate(possibleMaps[Random.Range(0, possibleMaps.Count)]);
        _map.fightRoom = _fightRoom;
        currentMaps.Add(_map);
        _fightRoom.garbage.Add(_map.gameObject);
        SetMapColor(_map);
        ShowStartTiles(_map);
        return _map;
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
        foreach (FightMap _map in currentMaps)
        {
            _map.GetStartTiles().ForEach(tile => tile.HideStartTile());
        }
    }

    internal void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map, bool _comeToExplo = false)
    {
        FightMapTile _tile = null;
        foreach (FightMapTile _currentTile in _map.GetTiles())
        {
            if (_currentTile.MatrixPositionWorld == _fightMapTile.MatrixPositionWorld)
            {
                _tile = _currentTile;
                break;
            }
        }
        if (_tile != null)
        {
            if (_comeToExplo)
            {
                ExplorationMapTile _exploTile = (ExplorationMapTile)_character.CurrentTile;
                _exploTile.characters.Remove(_character);
            }
            _tile.character = _character;
            _character.CurrentTile = _tile;
            _character.transform.position = _tile.transform.position;
        }
    }

    internal void SwitchTileCharacter(Character _character, FightMapTile _tile, bool _canSwitch = false)
    {
        FightMapTile _oldTile = (FightMapTile)_character.CurrentTile;
        Character _oldCharacter = _tile.character;

        if (!_canSwitch && _oldCharacter != null)
        {
            return;
        }

        _character.CurrentTile = _tile;
        _tile.character = _character;

        if (_canSwitch)
        {
            if (_oldCharacter != null)
            {
                _oldCharacter.CurrentTile = _oldTile;
            }
            _oldTile.character = _oldCharacter;
        }

        _character.transform.position = _tile.transform.position;

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

    internal List<FightMap> GetAllCurrentFightMaps()
    {
        return currentMaps;
    }
}