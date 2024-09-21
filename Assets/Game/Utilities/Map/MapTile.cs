using System;
using UnityEngine;

public abstract class MapTile : MonoBehaviour
{
    public bool hideVisualsOnStart;
    public Map map;
    public int ID;
    public Vector2Int MatrixPositionWorld = Vector2Int.zero;
    public Vector2Int MatrixPositionLocal = Vector2Int.zero;
    public Vector2Int MatrixPositionLocalTemporary = Vector2Int.zero;
    public abstract bool IsWalkable { get; }
    public abstract bool IsOccupied { get; }
    public abstract bool IsBlock { get; }
    public abstract bool BlockLineOfSight { get; }

    [SerializeField] private SpriteRenderer tips;

    public virtual void Start()
    {
        if (hideVisualsOnStart) HideVisuals();

        bool _odd = (MatrixPositionWorld.x + MatrixPositionWorld.y) % 2 == 0;
        SpriteRenderer _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer != null && IsWalkable) _spriteRenderer.color = _odd ? Colors.I.FloorOdd : Colors.I.FloorEven;
    }

    private void HideVisuals()
    {
        if (transform.childCount > 0) transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetMap(Map _map)
    {
        map = _map;
    }

    internal void DisplayTips(bool _active, Color? _color = null, TipsType _tipsType = TipsType.Default)
    {
        tips.gameObject.SetActive(_active);
        if(_active == false) return;
        tips.color = _color.Value;
        tips.sprite = _tipsType switch
        {
            TipsType.Blind => SpriteManager.I.Default,
            TipsType.Default => SpriteManager.I.TipsSprite.Default,
            _ => throw new NotImplementedException()
        };
    }
}