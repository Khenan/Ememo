using System.Collections.Generic;
using UnityEngine;

public partial class ExplorationMapTile : MapTile
{
    public List<Character> characters;
    [SerializeField] private bool isWalkable;
    public override bool IsWalkable => isWalkable;
    public override bool IsOccupied => characters.Count > 0;
}