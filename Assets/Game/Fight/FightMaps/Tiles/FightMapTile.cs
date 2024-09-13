using UnityEngine;

public class FightMapTile : MonoBehaviour
{
    private Vector3 position;
    public Vector3 Position => position;
    public Vector2 MatrixPosition { get; set; }
    [SerializeField] public int tileID;
    [SerializeField] private bool isWalkable;
    public bool IsWalkable => isWalkable;
    [SerializeField] private bool isOccupied;
    public bool IsOccupied => isOccupied;
    [SerializeField] private bool isStartTile;
    public bool IsStartTile => isStartTile;
    [SerializeField] private int teamId;
    public int TeamId => teamId;
    [SerializeField] private SpriteRenderer visualTop;
    public SpriteRenderer VisualTop => visualTop;
    [SerializeField] private SpriteRenderer highlight;
    public SpriteRenderer Highlight => highlight;

    public Character character;

    private void Start()
    {
        position = gameObject.transform.position;
    }

    public void ChangeColorHighlight(Color _color)
    {
        if (highlight != null)
            highlight.color = _color;
    }

    public void DisplayHighlight(bool _display = true)
    {
        if (highlight != null)
            highlight.gameObject.SetActive(_display);
        return;
    }
}
