using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapManager : Singleton<WorldMapManager>
{
    [SerializeField] private List<Map> createdMaps = new();
    [SerializeField] private List<Map> currentMaps = new();
    public List<Map> CurrentMaps => currentMaps;
    
    public void LoadMapAndAroundByMatrixPosition(Vector2Int _matrixPosition)
    {
        LoadMapAssetByMatrixPosition(_matrixPosition);
        LoadMapsAroundByMatrixPosition(_matrixPosition);
        DesactiveMapsNotAroundByMatrixPosition(_matrixPosition);
    }

    public void LoadMapsAroundByMatrixPosition(Vector2Int _matrixPosition)
    {
        // Orthogonal maps
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(0, 1));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(1, 0));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(0, -1));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(-1, 0));
        // Diagonal maps
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(1, 1));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(1, -1));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(-1, -1));
        LoadMapAssetByMatrixPosition(_matrixPosition + new Vector2Int(-1, 1));
    }

    private void LoadMapAssetByMatrixPosition(Vector2Int _matrixPosition)
    {
        Map _map = IsMapAlreadyExist(_matrixPosition);
        if (_map != null)
        {
            _map.gameObject.SetActive(true);
            if (!currentMaps.Contains(_map)) currentMaps.Add(_map);
        }
        else
        {
            _map = GetMapAssetByMatrixPosition(_matrixPosition);
            if (_map != null)
            {
                Map _mapGameObject = Instantiate(_map.gameObject, new Vector3(_matrixPosition.x, 0, -_matrixPosition.y) * MapSizeData.SIZE, Quaternion.identity).GetComponent<Map>();
                _mapGameObject.transform.SetParent(transform);
                createdMaps.Add(_mapGameObject);
                currentMaps.Add(_mapGameObject);
            }
        }
    }

    private void DesactiveMapsNotAroundByMatrixPosition(Vector2Int matrixPosition)
    {
        for (int _i = 0; _i < currentMaps.Count; _i++)
        {
            if (currentMaps[_i].matrixPosition.x < matrixPosition.x - 1 || currentMaps[_i].matrixPosition.x > matrixPosition.x + 1 ||
                currentMaps[_i].matrixPosition.y < matrixPosition.y - 1 || currentMaps[_i].matrixPosition.y > matrixPosition.y + 1)
            {
                currentMaps[_i].gameObject.SetActive(false);
                currentMaps.RemoveAt(_i);
                _i--;
            }
        }
    }

    public Map GetMapAssetByMatrixPosition(Vector2Int _matrixPosition)
    {
        Map _map = Resources.Load<Map>("MapPrefabs/ExploMap_" + _matrixPosition.x + "_" + _matrixPosition.y);
        if(_map == null) _map = Resources.Load<Map>("MapPrefabs/ExploMap_OnlyHole");
        _map.matrixPosition = _matrixPosition;
        _map.InitAll();
        return _map;
    }

    private Map IsMapAlreadyExist(Vector2Int _matrixPosition)
    {
        return createdMaps.Find(_m => _m.matrixPosition == _matrixPosition);
    }

    internal Map GetMapByMatrixPosition(Vector2Int _matrixPosition)
    {
        return currentMaps.Find(_m => _m.matrixPosition == _matrixPosition);
    }
}