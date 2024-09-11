using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class Character : ScriptableObject
{
    [SerializeField] private string characterName;
    [SerializeField] private List<Spell> spells;
    public List<Spell> Spells => spells;
    [SerializeField] private CharacterData data;
    public CharacterData Data => data;

    public int CurrentActionPoints => data.currentActionPoints;
}