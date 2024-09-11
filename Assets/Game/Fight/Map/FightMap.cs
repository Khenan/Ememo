using System.Collections.Generic;
using UnityEngine;

public class FightMap : MonoBehaviour
{
    internal int areaId;
    private List<FightMapTile> tiles;
    private int teamCount = 2;
    internal List<FightMapTile> GetStartTiles()
    {
        return tiles.FindAll(tile => tile.IsStartTile);
    }
}

public class FightMapTile : MonoBehaviour
{
    private Vector2 position;
    public Vector2 Position => position;
    [SerializeField] private bool isWalkable;
    public bool IsWalkable => isWalkable;
    [SerializeField] private bool isOccupied;
    public bool IsOccupied => isOccupied;
    [SerializeField] private bool isStartTile;
    public bool IsStartTile => isStartTile;
    [SerializeField] private int teamId;
    public int TeamId => teamId;
}