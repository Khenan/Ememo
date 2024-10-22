using System;
using System.Collections.Generic;
using UnityEngine;

public class FightMap : Map
{
    [SerializeField] internal int areaId;
    [SerializeField] internal FightRoom fightRoom;
    public void Start()
    {
        fightRoom.CurrentMaps.Add(this);
    }

    private void OnDestroy()
    {
        fightRoom.CurrentMaps.Remove(this);
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
