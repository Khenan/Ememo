using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Vector2 matrixPosition = Vector2.zero;
    public List<MapTile> mapTiles = new();
    public List<MapTile> GetTileOfYPosition(int _y)
    {
        return mapTiles.FindAll(_t => _t.MatrixPosition.y == _y).OrderBy(_t => _t.MatrixPosition.x).ToList();
    }

    [ContextMenu("Init Tiles")]
    private void InitAll()
    {
        GetAllChildrenTiles();
        InitOrderParentHierarchyByTransformPosition();
        InitTiles();
    }

    private void GetAllChildrenTiles()
    {
        mapTiles = new List<MapTile>(GetComponentsInChildren<MapTile>());
    }
    
    private void InitOrderParentHierarchyByTransformPosition() {
        Debug.Log("InitOrderParentHierarchyByTransformPosition: " + mapTiles.Count);
        mapTiles = mapTiles.OrderByDescending(_t => _t.transform.position.z).ThenBy(_t => _t.transform.position.x).ToList();
        for(int _i = 0; _i < mapTiles.Count; _i++) {
            mapTiles[_i].transform.SetSiblingIndex(_i);
        }
    }
    
    internal void InitTiles()
    {
        for(int _i = 0; _i < mapTiles.Count; _i++)
        {
            mapTiles[_i].MatrixPosition = new Vector2(_i % MapSizeData.SIZE, _i / MapSizeData.SIZE);
            mapTiles[_i].MatrixPositionBase = mapTiles[_i].MatrixPosition;
            mapTiles[_i].ID = _i;
            mapTiles[_i].name = $"Tile_{(mapTiles[_i].IsWalkable ? "Walkable" : "Hole")}_{mapTiles[_i].MatrixPosition.x}_{mapTiles[_i].MatrixPosition.y}";
            mapTiles[_i].SetMap(this);
        }
    }
    public void SetTileMatrixPositions()
    {
        for(int _i = 0; _i < mapTiles.Count; _i++)
        {
            mapTiles[_i].MatrixPosition = new Vector2(_i % MapSizeData.SIZE, _i / MapSizeData.SIZE);
        }
    }
}
