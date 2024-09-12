using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FightMap : MonoBehaviour
{
    [SerializeField] internal int areaId;
    [SerializeField] private List<FightMapTile> tiles;
    private int teamCount = 2;

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
    internal List<FightMapTile>  GetWalkableTiles()
    {
        return tiles.FindAll(tile => tile.IsWalkable);
    }
    internal List<FightMapTile> GetStartTiles()
    {
        return tiles.FindAll(tile => tile.IsStartTile);
    }
}
