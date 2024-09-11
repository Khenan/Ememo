using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMapTile : MonoBehaviour
{
    private Vector2 position;
    public Vector2 Position => position;
    [SerializeField] private bool isWalkable;
    public bool IsWalkable => isWalkable;
    [SerializeField] private bool isOccupied;
    public bool IsOccupied => isOccupied;
    [SerializeField] private bool isStartTile;
    public bool IsStartTile => isStartTile;
    [SerializeField] private int teamId;
    public int TeamId => teamId;
}
