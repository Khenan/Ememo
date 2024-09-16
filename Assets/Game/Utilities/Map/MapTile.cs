using UnityEngine;

public abstract class MapTile : MonoBehaviour
{
    public Map map;
    public int ID;
    public Vector2 MatrixPosition { get; set; }
    public Vector2 MatrixPositionBase { get; set; }
    public abstract bool IsWalkable { get; }
    public abstract bool IsOccupied { get; }

    public virtual void Start()
    {
        SetMap(GetComponentInParent<Map>());
    }
    public void SetMap(Map _map)
    {
        map = _map;
    }
}
