using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

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
    internal bool IsTileInRange(List<MapTile> _tiles, MapTile _centerTile, MapTile _targetTile, int _rangeMin, int _rangeMax, bool _sight = false, bool _inFight = false)
    {
        bool _valueToReturn = false;
        bool _isInRangeMin = DistanceBetweenTiles(_centerTile, _targetTile) >= _rangeMin;
        bool _isInRangeMax = DistanceBetweenTiles(_centerTile, _targetTile) <= _rangeMax;
        bool _isInRange = _isInRangeMin && _isInRangeMax;
        if (_isInRange && (_sight ? LineOfSight(_tiles, _centerTile, _targetTile, _inFight) : true))
        {
            _valueToReturn = true;
        }
        return _valueToReturn;
    }
    public bool LineOfSight(List<MapTile> _tiles, MapTile _centerTile, MapTile _targetTile, bool _inFight = false)
    {
        if (_centerTile == null || _targetTile == null)
        {
            throw new ArgumentNullException("Les tuiles ne doivent pas être nulles è_é.");
        }

        Vector2 centerPos = _centerTile.MatrixPositionLocalTemporary;
        Vector2 targetPos = _targetTile.MatrixPositionLocalTemporary;

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
                MapTile currentTile = GetFightTileByMatrixPositionTemporaryInList(_tiles, new Vector2Int(currentX, currentY));
                if (currentTile != null && currentTile.BlockLineOfSight)
                {
                    if (_inFight)
                    {
                        FightMapTile _characterTargetTile = (FightMapTile)_targetTile;
                        if (_inFight && _characterTargetTile.character != null && currentTile.MatrixPositionLocalTemporary == _characterTargetTile.MatrixPositionLocalTemporary)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
    internal List<MapTile> GetTilesByRangeInTemporaryList(List<MapTile> _tiles, MapTile _centerTile, int _rangeMin, int _rangeMax, bool _canWalk = false)
    {
        List<MapTile> _rangeTiles = new();
        int _startX = _centerTile.MatrixPositionLocalTemporary.x;
        int _startY = _centerTile.MatrixPositionLocalTemporary.y;
        // Get max Y or X in tiles
        int _rangeTilesMaxX = _tiles.Max(_t => _t.MatrixPositionLocalTemporary.x);
        int _rangeTilesMaxY = _tiles.Max(_t => _t.MatrixPositionLocalTemporary.y);
        int _rangeMaxTiles = Mathf.Max(_rangeTilesMaxX, _rangeTilesMaxY);

        for (int _x = 0; _x <= _rangeTilesMaxX; _x++)
        {
            for (int _y = 0; _y <= _rangeTilesMaxY; _y++)
            {
                int _sum = Mathf.Abs(_x - _startX) + Mathf.Abs(_y - _startY);
                if (_sum >= _rangeMin && _sum <= _rangeMax)
                {
                    MapTile _tile = GetFightTileByMatrixPositionTemporaryInList(_tiles, new Vector2Int(_x, _y));
                    if (_tile != null)
                    {
                        if (_canWalk && (!_tile.IsWalkable || _tile.IsOccupied))
                        {
                            continue;
                        }
                        _rangeTiles.Add(_tile);
                    }
                }
            }
        }
        return _rangeTiles;
    }
    private MapTile GetFightTileByMatrixPositionTemporaryInList(List<MapTile> _tiles, Vector2Int _matrixPositionTemporary)
    {
        MapTile _tile = null;
        foreach (MapTile _mapTile in _tiles)
        {
            if (_mapTile.MatrixPositionLocalTemporary == _matrixPositionTemporary)
            {
                _tile = _mapTile;
                break;
            }
        }
        return _tile;
    }
    internal bool IsDiagonal(MapTile _startTile, MapTile _endTile)
    {
        return Mathf.Abs(_startTile.MatrixPositionWorld.x - _endTile.MatrixPositionWorld.x) == 1 && Mathf.Abs(_startTile.MatrixPositionWorld.y - _endTile.MatrixPositionWorld.y) == 1;
    }
}
