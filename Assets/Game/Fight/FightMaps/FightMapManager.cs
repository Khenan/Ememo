using System.Collections.Generic;
using UnityEngine;

public class FightMapManager : Singleton<FightMapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<FightMap> maps;
    [SerializeField] private Color floorColor1, floorColor2, teamColor0, teamColor1;
    [SerializeField] private Camera cam;
    [SerializeField] private FightMapTile floor, wall, hole;

    public FightMapTile lastTileSelected;
    [SerializeField] private Transform tileSelectorVisual;
    private List<FightMapTile> lastTilesHighlighted = new();

    private FightMap currentMap;
    public FightMap CurrentMap => currentMap;

    private void Update()
    {
        tileSelectorVisual.gameObject.SetActive(lastTileSelected != null);
        if (lastTileSelected != null && tileSelectorVisual != null)
        {
            tileSelectorVisual.position = lastTileSelected.transform.position;
        }
    }

    private void SetCameraPosition()
    {
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
            bool _odd = (_floorTile.MatrixPosition.x + _floorTile.MatrixPosition.y) % 2 == 0;
            _floorTile.VisualTop.color = _odd ? floorColor1 : floorColor2;
        }
    }


    private void ShowHighlight(FightMapTile _tile, bool _show = true)
    {
        if (_tile != null)
            _tile.DisplayHighlight(_show);
    }

    private void ShowStartTiles(FightMap _map)
    {
        List<FightMapTile> _startTiles = _map.GetStartTiles();
        foreach (FightMapTile _startTile in _startTiles)
        {
            ShowHighlight(_startTile);
            switch (_startTile.TeamId)
            {
                case 0:
                    _startTile.ChangeColorHighlight(teamColor0);
                    break;
                case 1:
                    _startTile.ChangeColorHighlight(teamColor1);
                    break;
                default:
                    Debug.LogError("Undefined team");
                    break;
            };
        }
    }

    public void ShowTileList(List<FightMapTile> _tiles = null, bool _show = true)
    {
        if (lastTilesHighlighted != null)
        {
            foreach (FightMapTile _tile in lastTilesHighlighted)
            {
                ShowHighlight(_tile, false);
            }
        }
        if (_tiles != null)
        {
            foreach (FightMapTile _tile in _tiles)
            {
                ShowHighlight(_tile, _show);
            }
        }
        lastTilesHighlighted = _tiles;
    }

    public void StartFight()
    {
        ShowTileList(currentMap.GetStartTiles());
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
        int indexPos = (int)_matrixPosition.x + (int)_matrixPosition.y * (int)currentMap.Size.x;
        if (indexPos >= 0 && indexPos < currentMap.GetMapTileCount())
            return currentMap.GetTiles()[indexPos];
        else
            Debug.Log("This tile don't exist. Out of bounds of currentMap.GetTiles()");
        return null;
    }

    internal int DistanceBetweenTiles(FightMapTile currentTile, FightMapTile tile)
    {
        return (int)(Mathf.Abs(currentTile.MatrixPosition.x - tile.MatrixPosition.x) + Mathf.Abs(currentTile.MatrixPosition.y - tile.MatrixPosition.y));
    }
}