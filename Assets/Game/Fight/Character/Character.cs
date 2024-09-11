using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private string characterName;
    [SerializeField] private List<Spell> spells;
    [SerializeField] private CharacterData data;
}