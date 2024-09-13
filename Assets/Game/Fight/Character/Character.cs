using System;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    public string CharacterName => characterName;
    [SerializeField] protected List<Spell> spells;
    public List<Spell> Spells => spells;
    [SerializeField] protected CharacterData data;
    public CharacterData Data => data;

    public FightMapTile CurrentTile { get; set; }

    public int CurrentActionPoints => data.currentActionPoints;

    public bool isMyTurn = false;
    public bool isHumanController = false;

    [SerializeField] private GameObject isMyTurnArrow;

    private void Update()
    {
        isMyTurnArrow.SetActive(isMyTurn);
    }

    internal void InitData()
    {
        data.Init();
    }
}