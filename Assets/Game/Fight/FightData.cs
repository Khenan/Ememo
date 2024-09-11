using System.Collections.Generic;
using UnityEngine;

public class FightData : MonoBehaviour
{
    [SerializeField] private int areaId;
    public int AreaId => areaId;
    [SerializeField] private List<Character> characters;
    
}