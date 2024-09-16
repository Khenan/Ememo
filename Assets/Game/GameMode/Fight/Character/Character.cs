using System;
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

    public CharacterMode mode = CharacterMode.Exploration;

    private bool isDead = false;
    public bool IsDead => isDead;

    public MapTile CurrentTile { get; set; }

    public int CurrentActionPoints => data.currentActionPoints;

    public int teamId = 0;

    public bool isMyTurn = false;
    public bool isHumanController = false;

    // Actions
    public Action OnStartTurn;
    public Action OnEndTurn;
    public Action OnTakeDamage;

    private void Awake()
    {
        characterUI = GetComponent<CharacterUI>();
    }

    internal void StartFight()
    {
        UpdateAllUI();
    }

    internal void StartTurn()
    {
        isMyTurn = true;
        UpdateAllUI();
        OnStartTurn?.Invoke();
    }

    internal void EndTurn()
    {
        isMyTurn = false;
        ReloadComsumableData();
        OnEndTurn?.Invoke();
    }

    private void ReloadComsumableData()
    {
        CurrentData.currentActionPoints = CurrentData.maxActionPoints;
        CurrentData.currentMovementPoints = CurrentData.maxMovementPoints;
        UpdateAllUI();
    }

    public void SetCharacterMode(CharacterMode _mode)
    {
        mode = _mode;
    }

    internal void UpdateAllUI()
    {
        if (characterUI != null && currentData != null)
        {
            characterUI.SetHealthBar(currentData.currentHealth, currentData.maxHealth);
            if (mode == CharacterMode.Fight)
            {
                characterUI.SetTurnArrow(isMyTurn);
                UpdateSpellBar();
            }
        }
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
        currentData.currentHealth -= damage;
        currentData.currentHealth = Mathf.Clamp(currentData.currentHealth, 0, currentData.maxHealth);
        if (currentData.currentHealth <= 0)
        {
            Debug.Log("Character " + characterName + " is dead");
            Dead();
        }
        OnTakeDamage?.Invoke();
        UpdateAllUI();
    }

    private void Dead()
    {
        isDead = true;
        characterUI.Dead();
        if (mode == CharacterMode.Fight)
        {
            FightManager.I.OnCharacterDead(this);
        }
    }

    // SpellBar
    public void InitSpellBar(PlayerController _playerController)
    {
        int _currentPA = CurrentData.currentActionPoints;
        SpellBarUIManager.I.Init(_playerController, Spells, _currentPA);
    }

    private void UpdateSpellBar()
    {
        int _currentPA = CurrentData.currentActionPoints;
        SpellBarUIManager.I.UpdateSpellBar(_currentPA);
    }
}