using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]

[CreateAssetMenu(fileName = "SpellData", menuName = "SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    // Strings to adapt based on the language:
    public string spellName;
    public string description;

    // Costs to use the spell
    public int apCost;
    public List<SpellCost> extraCosts = new();

    // Everything about where and when you can cast the spell
    public bool isLignOfSight = true;
    public bool isChangeableRange = true;
    public int rangeMin;
    public int rangeMax;
    public bool isTargetRequired = false;
    public int castPerTurn;
    public int castPerTarget;
    public int cooldown;

    // Who is affected by the spell
    public TargetType targetType;
    public List<SpellZone> zones = new();
    public Character caster;
    public List<FightMapTile> targetedTiles = new();
    public List<SpellEffect> effects = new();
    public AttributeType scalingAttribute;
    public float scalingFactor;

    public void CastSpell(SpellEffectData _data)
    {
        foreach (SpellEffect _effect in effects)
        {
            _effect.ApplyEffect(_data);
        }
    }
}

[System.Serializable]
public enum SpellResourceType
{
    AP,
    MP,
    Health,
    Pressure,
    Piercing,
    Corruption
}

public enum AttributeType
{
    Fire,
    Water,
    Earth,
    Air
}

public enum TargetType
{
    Enemy,
    Ally,
    Caster,
    EmptyCell
}

public class SpellCost : ScriptableObject
{
    public SpellResourceType type = SpellResourceType.AP;
    public int amount;
}

[System.Serializable]
public class SpellZone
{
    public List<Vector2> tilesImpacted;
    // fait la liste des tiles impactés par le sort:
    // Exemple pour une croix : (0,0), (-1,0), (0,-1), (0,1), (1,0)
}

[System.Serializable]
public class SpellEffect
{

    public SpellEffectType type;
    public int value;
    public void ApplyEffect(SpellEffectData _data)
    {
        switch (type)
        {
            case SpellEffectType.Damage:
                Damage(_data);
                break;
            case SpellEffectType.Healing:
                Heal(_data);
                break;
            case SpellEffectType.Push:
                break;
            case SpellEffectType.Pull:
                break;
        }
    }
    public void Damage(SpellEffectData _data)
    {
        _data.target.TakeDamage(value);
    }
    public void Heal(SpellEffectData _data)
    {
        _data.target.TakeDamage(-value);
    }
}

public struct SpellEffectData
{
    public Character target;
}
