using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected string characterName;
    public string CharacterName => characterName;
    [SerializeField] protected List<SpellData> spells;
    public List<SpellData> Spells => spells;
    [SerializeField] protected CharacterData data;
    private CharacterUI characterUI;
    private CharacterData currentData;
    public CharacterData CurrentData => currentData;

    

    public FightMapTile CurrentTile { get; set; }

    public int CurrentActionPoints => data.currentActionPoints;

    public bool isMyTurn = false;
    public bool isHumanController = false;

    private void Awake() {
        characterUI = GetComponent<CharacterUI>();
    }

    internal void StartTurn()
    {
        isMyTurn = true;
        UpdateAllUI();
    }

    internal void EndTurn()
    {
        isMyTurn = false;
        ReloadComsumableData();
    }

    private void ReloadComsumableData()
    {
        CurrentData.currentActionPoints = CurrentData.maxActionPoints;
        CurrentData.currentMovementPoints = CurrentData.maxMovementPoints;
        UpdateAllUI();
    }

    internal void UpdateAllUI()
    {
        CharacterDataUIManager.Instance.SetHudValues(currentData.currentHealth, currentData.currentActionPoints, currentData.currentMovementPoints);
        characterUI.SetHealthBar(currentData.currentHealth,currentData.maxHealth);
        characterUI.SetTurnArrow(isMyTurn);
    }

    internal void InitData()
    {
        currentData = new();
        currentData.Copy(data);
        currentData.Init();
        UpdateAllUI();
    }

    internal void TakeDamage(int damage)
    {
        //Debug.Log("Character " + characterName + " takes " + damage + " damage");
        currentData.currentHealth -= damage;
        currentData.currentHealth = Mathf.Clamp(currentData.currentHealth, 0, currentData.maxHealth);
        if (currentData.currentHealth <= 0)
        {
            Debug.Log("Character " + characterName + " is dead");
            gameObject.SetActive(false);
        }
        UpdateAllUI();
    }
}