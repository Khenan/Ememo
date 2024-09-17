using System.Collections.Generic;
using UnityEngine;

public partial class ExplorationMapTile : MapTile
{
    public List<Character> characters = new();
    [SerializeField] private bool isWalkable;
    [SerializeField] private bool isBlock;
    public override bool IsWalkable => isWalkable;
    public override bool IsOccupied => characters.Count > 0;
    public override bool BlockLineOfSight => isBlock;
    public override bool IsBlock => isBlock;
}