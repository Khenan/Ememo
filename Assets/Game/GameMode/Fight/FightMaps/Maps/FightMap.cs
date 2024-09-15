using System.Collections.Generic;
using UnityEngine;

public class FightMap : Map
{
    [SerializeField] internal int areaId;
    [SerializeField] internal Vector2 size;
    public Vector2 Size => size;

    internal void Init()
    {
        InitTiles();
        SetTileMatrixPositions();
    }
    internal int GetMapTileCount()
    {
        return mapTiles.Count;
    }
    internal List<FightMapTile> GetTiles()
    {
        return mapTiles.ConvertAll(_tile => (FightMapTile)_tile);
    }
    internal void InitTiles()
    {
        int _id = 0;
        foreach (FightMapTile _tile in mapTiles)
        {
            _tile.tileID = _id;
            _tile.SetMap(this);
            _id++;
        }
    }
    internal void SetTileMatrixPositions()
    {
        int _tileCount = mapTiles.Count;
        while (_tileCount > 0)
        {
            foreach (FightMapTile _tile in mapTiles)
            {
                _tile.MatrixPosition = new Vector3(_tile.tileID % (int)size.x, _tile.tileID / (int)size.x, 0);
                _tileCount--;
            }
        }
    }
    internal List<FightMapTile> GetWalkableTiles()
    {
        return mapTiles.FindAll(tile => tile.IsWalkable).ConvertAll(_tile => (FightMapTile)_tile);
    }
    internal List<FightMapTile> GetStartTiles()
    {
        return mapTiles.ConvertAll(_tile => (FightMapTile)_tile).FindAll(tile => tile.IsStartTile);
    }
}
