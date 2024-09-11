using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FightMapManager : Singleton<FightMapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<FightMap> maps;
    [SerializeField] private Color floorColor1, floorColor2;
    [SerializeField] private Camera cam;

    private void Start() {
        GenerateMap(GetMap(0));
        cam.transform.position = new Vector3(12,1,-6);
        cam.transform.rotation = Quaternion.Euler(-35,-45,60);
    }

    internal FightMap GetMap(int _areaId)
    {
        List<FightMap> possibleMaps = maps.FindAll(map => map.areaId == _areaId);
        return possibleMaps[Random.Range(0, possibleMaps.Count)];
    }
    // Génère une map

    private void GenerateMap(FightMap _map)
    {
        FightMap _currentMap = Instantiate(_map);
        SetMapColor(_map);
    }

    private void SetMapColor(FightMap _map)
    {
        List<FightMapTile> _floorTiles = _map.GetWalkableTiles();
        foreach (FightMapTile _tile in _floorTiles)
        {
            SpriteRenderer _sprite = _tile.GetComponent<SpriteRenderer>();
            if ((_tile.transform.position.x + _tile.transform.position.y) % 2 == 1)
            {
                _sprite.color = floorColor1;
            }
            else
            {
                _sprite.color = floorColor2;

            }
        }
    }

    // Retourne une map parmi la liste des maps possibles
}