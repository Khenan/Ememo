using System.Collections.Generic;
using UnityEngine;

public partial class ExplorationMapTile : MonoBehaviour
{
    public Vector2 MatrixPosition { get; set; }
    private int tileID;
    public List<Character> characters;
    public bool IsWalkable { get; set; }

    private void Start()
    {
        
    }
}