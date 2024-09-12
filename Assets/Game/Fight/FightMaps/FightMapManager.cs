using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FightMapManager : Singleton<FightMapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<FightMap> maps;
    [SerializeField] private Color floorColor1, floorColor2, teamColor0, teamColor1;
    [SerializeField] private Camera cam;

    private void Start()
    {
        InitMap(GetMap(0));
        cam.transform.position = new Vector3(24, 21, -11);
        cam.transform.rotation = Quaternion.Euler(30, -45, 0);
    }

    internal FightMap GetMap(int _areaId)
    {
        List<FightMap> possibleMaps = maps.FindAll(map => map.areaId == _areaId);
        return possibleMaps[Random.Range(0, possibleMaps.Count)];
    }
    // Génère une map

    private void InitMap(FightMap _map)
    {
        FightMap _currentMap = Instantiate(_map);
        _map.SetTileIDs();
        SetMapColor(_map);
        ShowStartTiles(_map);
    }

    private void SetMapColor(FightMap _map)
    {
        List<FightMapTile> _floorTiles = _map.GetWalkableTiles();
        foreach (FightMapTile _floorTile in _floorTiles)
        {
            if ((_floorTile.transform.position.x + _floorTile.transform.position.z) % 2 == 1)
            {
                _floorTile.VisualTop.color = floorColor1;
            }
            else
            {
                _floorTile.VisualTop.color = floorColor2;
            }
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
}