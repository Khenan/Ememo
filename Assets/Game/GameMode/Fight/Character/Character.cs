using System;
using System.Collections.Generic;
using System.Reflection;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Character : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] protected string characterName;
    public string CharacterName => characterName;
    [SerializeField] private Transform characterNameTransform;
    private TextMeshProUGUI characterNameText;
    public Transform CharacterNameTransform => characterNameTransform;
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
    public TargetType targetType = new();

    // Fight Room
    public FightRoom fightRoom;

    // Visual
    [SerializeField] private Transform visualRoot;

    // Actions
    public Action OnStartTurn;
    public Action OnEndTurn;
    public Action OnTakeDamage;

    private void Awake()
    {
        characterUI = GetComponent<CharacterUI>();
        if (characterNameTransform != null) characterNameText = characterNameTransform.GetComponentInChildren<TextMeshProUGUI>();
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
        Debug.Log("Character Take Damage");
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
            fightRoom.OnCharacterDead(this);
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

    // Animation
    [SerializeField] private AnimationCurve idleYScaleCurve;
    [SerializeField] private AnimationCurve idleXZScaleCurve;
    [SerializeField] private float idleScaleSpeed = 1f;
    private float idleScaleTime = 0f;
    private float idleScaleMaxTime = 1f;

    private void IdleAnimationStretchAndSquashScale()
    {
        if (visualRoot == null) return;

        idleScaleTime += Time.deltaTime * idleScaleSpeed;
        if (idleScaleTime > idleScaleMaxTime)
        {
            idleScaleTime = 0f;
        }

        float _yScale = idleYScaleCurve.Evaluate(idleScaleTime);
        float _xzScale = idleXZScaleCurve.Evaluate(idleScaleTime);
        visualRoot.localScale = new Vector3(_xzScale, _yScale, _xzScale);
    }

    private void Update()
    {
        IdleAnimationStretchAndSquashScale();
    }

    internal void ChangeVisualDirection(Direction _direction)
    {
        if (visualRoot == null) return;

        switch (_direction)
        {
            case Direction.Up:
                visualRoot.forward = Vector3.forward;
                break;
            case Direction.Down:
                visualRoot.forward = Vector3.back;
                break;
            case Direction.Left:
                visualRoot.forward = Vector3.left;
                break;
            case Direction.Right:
                visualRoot.forward = Vector3.right;
                break;
        }
    }

    // Nickname
    internal void SetCharacterName(string _name)
    {
        characterName = _name;
        if (characterNameText != null) characterNameText.text = _name;
    }
    public void DisplayCharacterName(bool _display)
    {
        if (characterNameTransform == null) return;
        characterNameTransform.gameObject.SetActive(_display);
    }

    public void OnPhotonSerializeView(Photon.Pun.PhotonStream stream, PhotonMessageInfo info)
    {
        if(currentData == null) return;
        PhotonStream.I.StreamIntProperty(stream, ref currentData.maxHealth);
        PhotonStream.I.StreamIntProperty(stream, ref currentData.currentHealth);
        PhotonStream.I.StreamIntProperty(stream, ref currentData.maxActionPoints);
        PhotonStream.I.StreamIntProperty(stream, ref currentData.currentActionPoints);
        PhotonStream.I.StreamIntProperty(stream, ref currentData.maxMovementPoints);
        PhotonStream.I.StreamIntProperty(stream, ref currentData.currentMovementPoints);
        PhotonStream.I.StreamBoolProperty(stream, ref isDead);
        PhotonStream.I.StreamBoolProperty(stream, ref isMyTurn);
        PhotonStream.I.StreamIntProperty(stream, ref teamId);
        PhotonStream.I.StreamStringProperty(stream, ref characterName);
    }
}