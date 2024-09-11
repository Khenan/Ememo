using UnityEngine;

[System.Serializable]
public class CharacterData
{
    [Header("Level")]
    [SerializeField] internal int level = 1;

    [Header("Health")]
    [SerializeField] internal int defaultHealth = 10;
    internal int currentHealth;
    
    [Header("Initiative")]
    [SerializeField] internal int defaultInitiative = 0;
    internal int currentInitiative;
    
    [Header("Action Points")]
    [SerializeField] internal int defaultActionPoints = 6;
    internal int currentActionPoints;
    
    [Header("Movement Points")]
    [SerializeField] internal int defaultMovementPoints = 3;
    internal int currentMovementPoints;
}
