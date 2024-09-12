using System.Collections.Generic;
using UnityEngine;

public class FightData : MonoBehaviour
{
    [SerializeField] private int areaId;
    public int AreaId => areaId;
    [SerializeField] private List<FightCharacter> charactersToInstantiate;
    public List<FightCharacter> CharactersToInstantiate => charactersToInstantiate;
}

public struct FightCharacter
{
    public int teamId;
    public Character character;
}