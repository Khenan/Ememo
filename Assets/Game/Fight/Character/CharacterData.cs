using System;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Level")]
    [SerializeField] internal int level = 1;

    [Header("Health")]
    [SerializeField] internal int defaultHealth = 10;
    internal int maxHealth;
    internal int currentHealth;
    
    [Header("Initiative")]
    [SerializeField] internal int defaultInitiative = 0;
    internal int maxInitiative;
    internal int currentInitiative;
    
    [Header("Action Points")]
    [SerializeField] internal int defaultActionPoints = 6;
    internal int maxActionPoints;
    internal int currentActionPoints;
    
    [Header("Movement Points")]
    [SerializeField] internal int defaultMovementPoints = 3;
    internal int maxMovementPoints;
    internal int currentMovementPoints;

    public void Init() {
        maxHealth = defaultHealth;
        maxInitiative = defaultInitiative;
        maxActionPoints = defaultActionPoints;
        maxMovementPoints = defaultMovementPoints;

        currentHealth = maxHealth;
        currentInitiative = maxInitiative;
        currentActionPoints = maxActionPoints;
        currentMovementPoints = maxMovementPoints;
    }

    internal void Copy(CharacterData data)
    {
        level = data.level;
        defaultHealth = data.defaultHealth;
        defaultInitiative = data.defaultInitiative;
        defaultActionPoints = data.defaultActionPoints;
        defaultMovementPoints = data.defaultMovementPoints;
    }
}
