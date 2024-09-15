using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "SpellData", menuName = "SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    public string spellName;
    public string description;
    public int apCost;
    public List<Cost> extraCosts;
    public bool isLignOfSight;
    public bool isChangableRange;
    public int rangeMin;
    public int rangeMax;
    public List<Character> targets;
    public bool isTargetRequired;
    public int castPerTurn;
    public int castPerTarget;
    public int cooldown;
    public List<Zone> zones;
    public Character caster;
    public List<FightMapTile> targetedTiles;
    public int damage;
}

public class Cost {
    // récupère une statistique qui pourrait être un coup (genre PM ou HP ou Etats) et le fait payer.
}

public class Zone {
    public List<Vector2> tilesImpacted;
    // fait la liste des tiles impactés par le sort:
        // Exemple pour une croix : (0,0), (-1,0), (0,-1), (0,1), (1,0)
}

public class Effect {
    // Liste les effects des sorts, peut être une classe abstracte avec chaque sort ayant des effets différents
}