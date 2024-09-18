using System.Collections.Generic;
using UnityEngine;

public class FightData : MonoBehaviour
{
    public int areaId;
    public int AreaId => areaId;
    public Vector2Int matrixPositionWorld = Vector2Int.zero;
    public List<FightCharacter> charactersToInstantiate;
}

[System.Serializable]
public struct FightCharacter
{
    public int teamId;
    public Character character;

    public FightCharacter(int _teamId, Character _character)
    {
        teamId = _teamId;
        character = _character;
    }
}