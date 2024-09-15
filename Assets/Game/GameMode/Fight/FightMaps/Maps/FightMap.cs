using System;
using System.Collections.Generic;
using UnityEngine;

public class FightMap : Map
{
    [SerializeField] internal int areaId;
    [SerializeField] internal Vector2 size;
    public Vector2 Size => size;

    private void Awake()
    {
        GetAllChildrenTiles();
    }
    private void Start()
    {
        FightManager.I.currentMaps.Add(this);
    }

    private void GetAllChildrenTiles()
    {
        mapTiles = new List<MapTile>(GetComponentsInChildren<MapTile>());
    }

    private void OnDestroy()
    {
        FightManager.I.currentMaps.Remove(this);
    }
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
    internal List<FightMapTile> GetWalkableTiles()
    {
        return mapTiles.FindAll(tile => tile.IsWalkable).ConvertAll(_tile => (FightMapTile)_tile);
    }
    internal List<FightMapTile> GetStartTiles()
    {
        return mapTiles.ConvertAll(_tile => (FightMapTile)_tile).FindAll(tile => tile.IsStartTile);
    }
}
