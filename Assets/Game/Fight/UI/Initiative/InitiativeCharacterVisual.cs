using System;
using TMPro;
using UnityEngine;

public class InitiativeCharacterVisual : MonoBehaviour
{
    [NonSerialized] public Vector3 targetPosition;
    [NonSerialized] public string characterName = "Character";

    [NonSerialized] public Vector3 Y_POSITION = Vector3.up * 70;

    private TextMeshProUGUI textMeshProUGUI;

    private void Awake() {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    internal void Init(int _index, string _characterName)
    {
        UpdatePosition(_index);
        characterName = _characterName;
        textMeshProUGUI.text = _characterName;
    }

    public void UpdatePosition(int _index)
    {
        targetPosition = _index * Y_POSITION;
        transform.localPosition = targetPosition;
    }
}
