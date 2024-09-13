using System;
using System.Collections.Generic;
using UnityEngine;

public class FightMap : MonoBehaviour
{
    [SerializeField] internal int areaId;
    [SerializeField] internal Vector2 size;
    public Vector2 Size => size;
    [SerializeField] private List<FightMapTile> tiles;
    private int teamCount = 2;

    internal void Init()
    {
        SetTileIDs();
        SetTileMatrixPositions();
    }
    internal List<FightMapTile>  GetTiles()
    {
        return tiles;
    }
    internal void SetTileIDs()
    {
        int _id = 0;
        foreach (FightMapTile _tile in tiles)
        {
            _tile.tileID = _id;
            _id++;
        }
    }
    internal void SetTileMatrixPositions()
    {
        int _tileCount = tiles.Count;
        while (_tileCount > 0)
        {
            foreach (FightMapTile _tile in tiles)
            {
                _tile.MatrixPosition = new Vector3(_tile.tileID % (int)size.x, _tile.tileID / (int)size.x, 0);
                _tileCount--;
            }
        }
    }
    internal List<FightMapTile>  GetWalkableTiles()
    {
        return tiles.FindAll(tile => tile.IsWalkable);
    }
    internal List<FightMapTile> GetStartTiles()
    {
        return tiles.FindAll(tile => tile.IsStartTile);
    }
}
