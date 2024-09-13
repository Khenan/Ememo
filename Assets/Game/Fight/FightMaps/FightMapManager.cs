using System.Collections.Generic;
using UnityEditor.PackageManager;
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

    private void ShowStartTiles(FightMap _map)
    {
        SpriteRenderer _highlightSprite;

        List<FightMapTile> _startTiles = _map.GetStartTiles();
        foreach (FightMapTile _startTile in _startTiles)
        {
            _startTile.Highlight.gameObject.SetActive(true);
            _highlightSprite = _startTile.Highlight;
            if (_startTile.TeamId == 0)
            {
                _highlightSprite.color = teamColor0;
            }
            else if (_startTile.TeamId == 1)
            {
                _highlightSprite.color = teamColor1;
            }
            else
            {
                Debug.LogError("Undefined team");
            }
        }
    }

    internal void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        _map.GetTiles().Find(tile => tile.Position == _fightMapTile.Position).character = _character;
    }

    internal void SwitchTileCharacter(Character character, FightMapTile tile, bool _canSwitch = false)
    {
        FightMapTile _oldTile = character.CurrentTile;
        Character _oldCharacter = tile.character;

        if(!_canSwitch && _oldCharacter != null)
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
        else {
            _oldTile.character = null;
        }
    }

    internal FightMapTile GetTileByMatrixPosition(Vector2 _matrixPosition)
    {
        return currentMap.GetTiles()[(int)_matrixPosition.x + (int)_matrixPosition.y * (int)currentMap.Size.x];
    }

    internal int DistanceBetweenTiles(FightMapTile currentTile, FightMapTile tile)
    {
        return (int)(Mathf.Abs(currentTile.MatrixPosition.x - tile.MatrixPosition.x) + Mathf.Abs(currentTile.MatrixPosition.y - tile.MatrixPosition.y));
    }
}