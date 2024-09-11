using UnityEngine;

public class CharacterData
{
    [Header("Health")]
    private int defaultHealth = 10;
    private int currentHealth;
    
    [Header("Initiative")]
    private int defaultInitiative = 0;
    private int currentInitiative;
    
    [Header("Action Points")]
    private int defaultActionPoints = 6;
    private int currentActionPoints;
    
    [Header("Movement Points")]
    private int defaultMovementPoints = 3;
    private int currentMovementPoints;

    private int level = 1;
}
