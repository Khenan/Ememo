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
    public bool IsVisible = true;
    private bool tipsActive = false;
    private float tipsAlpha = 0f;

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

    internal void DisplayTips(bool _active, Color? _color = null, TipsType _tipsType = TipsType.Default, bool _hardDisplay = false)
    {
        tipsActive = _active;
        if (!_active) return;
        else tips.gameObject.SetActive(true);
        if (_hardDisplay)
        {
            tipsAlpha = 1;
            tips.gameObject.SetActive(_active);
        }
        tips.color = _color.Value;
        tips.sprite = _tipsType switch
        {
            TipsType.Blind => SpriteManager.I.Default,
            TipsType.Default => SpriteManager.I.TipsSprite.Default,
            _ => throw new NotImplementedException()
        };
    }

    internal void UpdateTile()
    {
        DisplayFadeTips(tipsActive);
    }

    private void DisplayFadeTips(bool tipsActive)
    {
        if (tips.gameObject.activeSelf)
        {
            tipsAlpha += tipsActive ? Time.deltaTime : -Time.deltaTime;
            tipsAlpha = Mathf.Clamp(tipsAlpha, 0, 1);
            tips.color = new Color(tips.color.r, tips.color.g, tips.color.b, tipsAlpha);
            if (tipsAlpha == 0) tips.gameObject.SetActive(false);
        }
    }
}