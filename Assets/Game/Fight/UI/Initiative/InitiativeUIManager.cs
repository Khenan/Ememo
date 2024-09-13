using System;
using System.Collections.Generic;
using UnityEngine;

public class InitiativeUIManager : Singleton<InitiativeUIManager>
{
    [SerializeField] private GameObject root;
    [SerializeField] private InitiativeCharacterVisual initiativeUICharacterPrefab;
    private List<InitiativeCharacterVisual> currentVisuals = new();

    public void Init(List<Character> _characters)
    {
        Clear();
        for (int _i = 0; _i < _characters.Count; _i++)
        {
            InitiativeCharacterVisual _visual = Instantiate(initiativeUICharacterPrefab, root.transform);
            _visual.Init(_i, _characters[_i].CharacterName);
            currentVisuals.Add(_visual);
        }
        UpdateVisuals();
    }

    internal void UpdateTurn(int _index)
    {
        UpdateVisuals();
        InitiativeCharacterVisual _visual = currentVisuals[_index];
        _visual.transform.localPosition += Vector3.left * 10;
    }

    private void Clear()
    {
        foreach (InitiativeCharacterVisual _visual in currentVisuals)
        {
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
    }
}