using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Map : MonoBehaviour
{
    public Vector2Int matrixPosition = Vector2Int.zero;
    public List<MapTile> mapTiles = new();
    public List<MapTile> GetTileOfYPosition(int _y)
    {
        return mapTiles.FindAll(_t => _t.MatrixPositionWorld.y == _y).OrderBy(_t => _t.MatrixPositionWorld.x).ToList();
    }

    [ContextMenu("Init Tiles")]
    public void InitAll()
    {
        GetAllChildrenTiles();
        InitOrderParentHierarchyByTransformPosition();
        InitTiles();
    }

    private void GetAllChildrenTiles()
    {
        mapTiles = new List<MapTile>(GetComponentsInChildren<MapTile>());
    }

    private void InitOrderParentHierarchyByTransformPosition()
    {
        Debug.Log("Init map " + name + " with " + mapTiles.Count + " tiles.");
        mapTiles = mapTiles.OrderByDescending(_t => _t.transform.position.z).ThenBy(_t => _t.transform.position.x).ToList();
        for (int _i = 0; _i < mapTiles.Count; _i++)
        {
            mapTiles[_i].transform.SetSiblingIndex(_i);
        }
    }

    internal void InitTiles()
    {
        for (int _i = 0; _i < mapTiles.Count; _i++)
        {
            mapTiles[_i].MatrixPositionLocalTemporary = Vector2Int.zero;
            mapTiles[_i].MatrixPositionLocal = new Vector2Int(_i % MapSizeData.SIZE, _i / MapSizeData.SIZE);
            int _worldX = mapTiles[_i].MatrixPositionLocal.x + (MapSizeData.SIZE * matrixPosition.x);
            int _worldY = mapTiles[_i].MatrixPositionLocal.y + (MapSizeData.SIZE * matrixPosition.y);
            mapTiles[_i].MatrixPositionWorld = new Vector2Int(_worldX, _worldY);
            mapTiles[_i].ID = _i;
            mapTiles[_i].name = $"Tile_{(mapTiles[_i].IsWalkable ? "Walkable" : "Hole")}_{mapTiles[_i].MatrixPositionWorld.x}_{mapTiles[_i].MatrixPositionWorld.y}";
            mapTiles[_i].SetMap(this);
            Debug.Log("Tile Init");  
        }
    }
    public void ResetTileMatrixPositionsLocalTemporary()
    {
        for (int _i = 0; _i < mapTiles.Count; _i++)
        {
            mapTiles[_i].MatrixPositionLocalTemporary = new Vector2Int(_i % MapSizeData.SIZE, _i / MapSizeData.SIZE);
        }
    }

    internal MapTile GetTileByMatrixPosition(Vector2Int _matrixPosition)
    {
        return mapTiles[_matrixPosition.x + _matrixPosition.y * MapSizeData.SIZE];
    }
}
