using System;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeUIManager : Singleton<InitiativeUIManager>
{
    [SerializeField] private GameObject root;
    [SerializeField] private InitiativeCharacterVisual initiativeUICharacterPrefab;
    private List<InitiativeCharacterVisual> currentVisuals = new();
    private RectTransform rootRectTransform;
    private Vector2 rootSize = new Vector2(50, 50);

    private int currentTurnIndex = 0;

    public override void Awake()
    {
        base.Awake();
        rootRectTransform = root.GetComponent<RectTransform>();
    }

    public void Init(List<Character> _characters)
    {
        Clear();
        for (int _i = 0; _i < _characters.Count; _i++)
        {
            InitiativeCharacterVisual _visual = Instantiate(initiativeUICharacterPrefab, root.transform);
            _visual.Init(_i - 1, _characters[_i].CharacterName);
            currentVisuals.Add(_visual);
            FightManager.I.AddGarbage(_visual.gameObject);
        }
        currentTurnIndex = 0;
        UpdateVisuals();
    }

    internal void CharacterDead(int _index)
    {
        currentVisuals[_index].Dead();
        UpdateTurn(currentTurnIndex);
    }

    internal void UpdateTurn(int _index)
    {
        UpdateVisuals();
        currentTurnIndex = _index;
        InitiativeCharacterVisual _visual = currentVisuals[_index];
        _visual.transform.localPosition += Vector3.left * 10;
    }

    private void Clear()
    {
        foreach (InitiativeCharacterVisual _visual in currentVisuals)
        {
            if(_visual != null)
                Destroy(_visual.gameObject);
        }
        currentVisuals.Clear();
    }

    private void UpdateVisuals()
    {
        for (int _i = 0; _i < currentVisuals.Count; _i++)
        {
            currentVisuals[_i].UpdatePosition(_i);
        }
        rootSize.y = currentVisuals.Count * InitiativeCharacterVisual.Y_SIZE;
        rootRectTransform.sizeDelta = rootSize;
    }
}