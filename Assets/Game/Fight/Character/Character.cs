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

    public int CurrentActionPoints => data.currentActionPoints;
}