using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public MapTile lastTileHovered;
    internal List<Vector2Int> GetMatrixPositionsByRange(Vector2Int _centerTile, int _rangeMin, int _rangeMax)
    {
        List<Vector2Int> _rangeTiles = new();
        int _startX = _centerTile.x;
        int _startY = _centerTile.y;
        for (int _x = _startX - _rangeMax; _x <= _startX + _rangeMax; _x++)
        {
            for (int _y = _startY - _rangeMax; _y <= _startY + _rangeMax; _y++)
            {
                int _sum = Mathf.Abs(_x - _startX) + Mathf.Abs(_y - _startY);

                if (_sum >= _rangeMin && _sum <= _rangeMax)
                {
                    _rangeTiles.Add(new Vector2Int(_x, _y));
                }
            }
        }
        return _rangeTiles;
    }


    internal int DistanceBetweenTiles(MapTile currentTile, MapTile tile)
    {
        return Mathf.Abs(currentTile.MatrixPositionWorld.x - tile.MatrixPositionWorld.x) + Mathf.Abs(currentTile.MatrixPositionWorld.y - tile.MatrixPositionWorld.y);
    }
    internal bool IsTileInRange(Map _map, MapTile _centerTile, MapTile _targetTile, int _rangeMin, int _rangeMax, bool _sight = false, bool _inFight = false)
    {
        bool _valueToReturn = false;
        bool _isInRangeMin = DistanceBetweenTiles(_centerTile, _targetTile) >= _rangeMin;
        bool _isInRangeMax = DistanceBetweenTiles(_centerTile, _targetTile) <= _rangeMax;
        bool _isInRange = _isInRangeMin && _isInRangeMax;
        if (_isInRange && (_sight ? LineOfSight(_map, _centerTile, _targetTile, _inFight) : true))
        {
            _valueToReturn = true;
        }
        return _valueToReturn;
    }
    public bool LineOfSight(Map _map, MapTile _centerTile, MapTile _targetTile, bool _inFight = false)
    {
        if (_centerTile == null || _targetTile == null)
        {
            throw new ArgumentNullException("Les tuiles ne doivent pas être nulles è_é.");
        }

        Vector2 centerPos = _centerTile.MatrixPositionWorld;
        Vector2 targetPos = _targetTile.MatrixPositionWorld;

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
                MapTile currentTile = GetFightTileByMatrixPosition(_map, new Vector2(currentX, currentY));
                if (currentTile != null && currentTile.BlockLineOfSight)
                {
                    FightMapTile _characterTile = (FightMapTile)currentTile;
                    if (_inFight && currentTile.MatrixPositionWorld == _characterTile.MatrixPositionWorld && _characterTile.character != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

    internal FightMapTile GetFightTileByMatrixPosition(Map _map, Vector2 _matrixPosition)
    {
        FightMapTile _fightTile = null;
        foreach (MapTile _tile in _map.mapTiles)
        {
            if (_tile.MatrixPositionWorld == _matrixPosition)
            {
                _fightTile = (FightMapTile) _tile;
            }
        }
        return _fightTile;
    }

    internal List<MapTile> GetTilesByRange(Map _map, MapTile _centerTile, int _rangeMin, int _rangeMax, bool _canWalk = false)
    {
        List<MapTile> _rangeTiles = new();
        int _startX = _centerTile.MatrixPositionWorld.x;
        int _startY = _centerTile.MatrixPositionWorld.y;
        for (int _x = _startX - _rangeMax; _x <= _startX + _rangeMax; _x++)
        {
            for (int _y = _startY - _rangeMax; _y <= _startY + _rangeMax; _y++)
            {
                int _sum = Mathf.Abs(_x - _startX) + Mathf.Abs(_y - _startY);

                if (_sum >= _rangeMin && _sum <= _rangeMax)
                {
                    MapTile _tile = GetFightTileByMatrixPosition(_map, new Vector2Int(_x, _y));
                    if (_tile != null && _canWalk ? !_tile.IsOccupied : true)
                    {
                        _rangeTiles.Add(_tile);
                    }
                }
            }
        }
        return _rangeTiles;
    }

    internal bool IsDiagonal(MapTile _startTile, MapTile _endTile)
    {
        return Mathf.Abs(_startTile.MatrixPositionWorld.x - _endTile.MatrixPositionWorld.x) == 1 && Mathf.Abs(_startTile.MatrixPositionWorld.y - _endTile.MatrixPositionWorld.y) == 1;
    }
}
