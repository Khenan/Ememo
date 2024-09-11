using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FightMap : MonoBehaviour
{
    internal int areaId;
    [SerializeField] private List<FightMapTile> tiles;
    private int teamCount = 2;

    internal List<FightMapTile>  GetTiles()
    {
        return tiles;
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