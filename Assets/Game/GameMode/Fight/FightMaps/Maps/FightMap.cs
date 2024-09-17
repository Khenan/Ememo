using System;
using System.Collections.Generic;
using UnityEngine;

public class FightMap : Map
{
    [SerializeField] internal int areaId;
    public void Start()
    {
        FightManager.I.currentMaps.Add(this);
    }

    private void OnDestroy()
    {
        FightManager.I.currentMaps.Remove(this);
    }
    internal int GetMapTileCount()
    {
        return mapTiles.Count;
    }
    internal List<FightMapTile> GetTiles()
    {
        return mapTiles.ConvertAll(_tile => (FightMapTile)_tile);
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
