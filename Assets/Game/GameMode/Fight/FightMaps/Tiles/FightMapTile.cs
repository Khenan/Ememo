using UnityEngine;

public partial class FightMapTile : MonoBehaviour
{
    private Vector3 position;
    public Vector3 Position => position;
    public Vector2 MatrixPosition { get; set; }
    [SerializeField] public int tileID;
    [SerializeField] private bool isWalkable;
    public bool IsWalkable => isWalkable;
    [SerializeField] private bool isOccupied;
    public bool IsOccupied => isOccupied || character != null;
    [SerializeField] private bool blockLineOfSight;
    public bool BlockLineOfSight => blockLineOfSight || character != null;
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

    private void Start()
    {
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
            bool _odd = (MatrixPosition.x + MatrixPosition.y) % 2 == 0;
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
