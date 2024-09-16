using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConcatenatorMapList
{
    public static List<MapTile> ConcatenateMaps(List<Map> _mapList, MapTile _currentTile, MapTile _targetTile)
    {
        List<MapTile> _concatenatedMap = new();
        List<Map> _orderedMapList = OrderByMatrixPosition(_mapList);
        ResetMatrixPositionOfAllTiles(_orderedMapList);
        int _maxMapY = GetMaxMapY(_orderedMapList);
        int _minMapY = GetMinMapY(_orderedMapList);
        int _diffY = (int)GetMapDiff(_currentTile, _targetTile).y;
        int _diffX = (int)GetMapDiff(_currentTile, _targetTile).x;

        // We take the range of tile lists from each map and add them to the concatenated list
        for (int _mapY = _minMapY; _mapY <= _maxMapY; _mapY++)
        {
            for (int _y = 0; _y < MapSizeData.SIZE * (_diffY + 1); _y++)
            {
                for (int _index = 0; _index < _orderedMapList.Count; _index++)
                {
                    if ((int)_orderedMapList[_index].matrixPosition.y == _mapY)
                    {
                        List<MapTile> _tiles = _orderedMapList[_index].GetTileOfYPosition(_y);
                        _concatenatedMap.AddRange(_tiles);
                    }
                }
            }
        }

        // Set the matrix position of each tile in the concatenated map
        for (int _i = 0; _i < _concatenatedMap.Count; _i++)
        {
            _concatenatedMap[_i].MatrixPositionLocalTemporary = new Vector2Int(_i % (MapSizeData.SIZE * (_diffX + 1)), _i / (MapSizeData.SIZE * (_diffX + 1)));
        }

        return _concatenatedMap;
    }

    private static void ResetMatrixPositionOfAllTiles(List<Map> orderedMapList)
    {
        foreach (Map _map in orderedMapList)
        {
            _map.ResetTileMatrixPositionsLocalTemporary();
        }
    }

    private static int GetMinMapY(List<Map> orderedMapList)
    {
        return (int)orderedMapList.Min(_m => _m.matrixPosition.y);
    }

    private static int GetMaxMapY(List<Map> orderedMapList)
    {
        return (int)orderedMapList.Max(_m => _m.matrixPosition.y);
    }

    private static List<Map> OrderByMatrixPosition(List<Map> _mapList)
    {
        return _mapList.OrderBy(_m => _m.matrixPosition.y).ThenBy(_m => _m.matrixPosition.x).ToList();
    }

    private static Vector2 GetMapDiff(MapTile _currentTile, MapTile _targetTile)
    {
        Vector2 _mapDiff = new Vector2();
        _mapDiff.x = Mathf.Abs((int)_targetTile.map.matrixPosition.x - (int)_currentTile.map.matrixPosition.x);
        _mapDiff.y = Mathf.Abs((int)_targetTile.map.matrixPosition.y - (int)_currentTile.map.matrixPosition.y);
        return _mapDiff;
    }
}
