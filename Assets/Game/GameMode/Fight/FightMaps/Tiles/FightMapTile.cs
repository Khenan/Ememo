using UnityEngine;

public partial class FightMapTile : MapTile
{
    private Vector3 position;
    public Vector3 Position => position;
    [SerializeField] public bool isWalkable;
    [SerializeField] public bool isBlock;
    public override bool IsWalkable => isWalkable;
    public override bool IsOccupied => character != null;
    [SerializeField] private bool blockLineOfSight;
    public override bool BlockLineOfSight => blockLineOfSight || character != null || isBlock;
    public override bool IsBlock => isBlock;
    [SerializeField] private bool isStartTile;
    public bool IsStartTile => isStartTile;
    [SerializeField] private int teamId;
    public int TeamId => teamId;
    [SerializeField] private SpriteRenderer visualTop;
    public SpriteRenderer VisualTop => visualTop;
    [SerializeField] private SpriteRenderer highlight;
    public SpriteRenderer Highlight => highlight;

    [SerializeField] private SpriteRenderer highlightTips;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite blindSprite;

    public Character character;

    public override void Start()
    {
        base.Start();
        position = gameObject.transform.position;
        if(highlight != null) highlight.gameObject.SetActive(false);
        if(highlightTips != null) highlightTips.gameObject.SetActive(false);
    }

    public void DisplayHighlight(bool _display = true, Color _color = default, bool _withTips = false, TipsType _tipsType = TipsType.Default)
    {
        if (highlight != null)
        {
            highlight.color = _color == default ? Colors.I.DefaultHightlight : _color;
            highlight.gameObject.SetActive(_display);

            SetHighlightTips(_withTips, _tipsType);
        }
    }

    internal void ColorTop(Color _color)
    {
        if (visualTop != null)
        {
            visualTop.color = _color;
        }
    }
    internal void HideStartTile()
    {
        ColorBaseTopByIndex();
    }

    internal void ColorBaseTopByIndex()
    {
        if (visualTop != null)
        {
            bool _odd = (MatrixPositionWorld.x + MatrixPositionWorld.y) % 2 == 0;
            VisualTop.color = _odd ? Colors.I.FloorOdd : Colors.I.FloorEven;
        }
    }

    private void SetHighlightTips(bool _active, TipsType _tipsType)
    {
        if(highlightTips == null) return;
        highlightTips.gameObject.SetActive(_active);
        switch (_tipsType)
        {
            case TipsType.Default:
                highlightTips.sprite = defaultSprite;
                break;
            case TipsType.Blind:
                highlightTips.sprite = blindSprite;
                break;
        }
    }
}
