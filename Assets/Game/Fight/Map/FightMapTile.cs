using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FightMapTile : MonoBehaviour
{
    private Vector3 position;
    public Vector3 Position => position;
    [SerializeField] private bool isWalkable;
    public bool IsWalkable => isWalkable;
    [SerializeField] private bool isOccupied;
    public bool IsOccupied => isOccupied;
    [SerializeField] private bool isStartTile;
    public bool IsStartTile => isStartTile;
    [SerializeField] private int teamId;
    public int TeamId => teamId;
    [SerializeField] private GameObject highlight;
    public GameObject Highlight => highlight;

    public Character character;

    private void Start() {
        position = gameObject.transform.position;
    }

}
