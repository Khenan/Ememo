using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConcatenatorMapList
{
    public static List<MapTile> ConcatenateMaps(List<Map> _mapList)
    {
        List<MapTile> _concatenatedMap = new();
        List<Map> _orderedMapList = OrderByMatrixPosition(_mapList);
        ResetMatrixPositionOfAllTiles(_orderedMapList);

        if (_mapList.Count == 1)
        {
            return _mapList[0].mapTiles;
        }

        int _maxMapY = GetMaxMapY(_orderedMapList);
        int _minMapY = GetMinMapY(_orderedMapList);
        int _diffY = GetMapDiff(_mapList).y;
        int _diffX = GetMapDiff(_mapList).x;

        // We take the range of tile lists from each map and add them to the concatenated list
        for (int _mapY = _minMapY; _mapY <= _maxMapY; _mapY++)
        {
            for (int _y = 0; _y < MapSizeData.SIZE * (_diffY + 1); _y++)
            {
                for (int _index = 0; _index < _orderedMapList.Count; _index++)
                {
                    if (_orderedMapList[_index].matrixPosition.y == _mapY)
                    {
                        List<MapTile> _tiles = _orderedMapList[_index].GetTileOfYLocalPosition(_y);
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

    private static int GetMaxMapY(List<Map> _orderedMapList)
    {
        return _orderedMapList.Max(_m => _m.matrixPosition.y);
    }

    private static List<Map> OrderByMatrixPosition(List<Map> _mapList)
    {
        return _mapList.OrderBy(_m => _m.matrixPosition.y).ThenBy(_m => _m.matrixPosition.x).ToList();
    }

    private static Vector2Int GetMapDiff(List<Map> _mapList)
    {
        Vector2Int _mapDiff = new Vector2Int();
        // Get min and max x of Matrix position in Maps
        int _minX = _mapList.Min(_m => _m.matrixPosition.x);
        int _maxX = _mapList.Max(_m => _m.matrixPosition.x);
        // Get min and max y of Matrix position in Maps
        int _minY = _mapList.Min(_m => _m.matrixPosition.y);
        int _maxY = _mapList.Max(_m => _m.matrixPosition.y);
        // Get the difference between the min and max x and y
        _mapDiff.x = Mathf.Abs(_minX - _maxX);
        _mapDiff.y = Mathf.Abs(_minY - _maxY);
        return _mapDiff;
    }
}
