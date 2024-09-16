using System;
using UnityEngine;

public abstract class MapTile : MonoBehaviour
{
    public bool hideVisualsOnStart;
    public Map map;
    public int ID;
    public Vector2 MatrixPosition { get; set; }
    public Vector2 MatrixPositionBase { get; set; }
    public abstract bool IsWalkable { get; }
    public abstract bool IsOccupied { get; }

    public virtual void Start()
    {
        SetMap(GetComponentInParent<Map>());
        if(hideVisualsOnStart) HideVisuals();
    }

    private void HideVisuals()
    {
        if(transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetMap(Map _map)
    {
        map = _map;
    }
}
