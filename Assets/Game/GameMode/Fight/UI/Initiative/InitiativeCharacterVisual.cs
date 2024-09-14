using System;
using TMPro;
using UnityEngine;

public class InitiativeCharacterVisual : MonoBehaviour
{
    [NonSerialized] public Vector3 targetPosition;
    [NonSerialized] public string characterName = "Character";
    [NonSerialized] public static int Y_SIZE = 70;

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
        targetPosition = _index * Vector3.up * Y_SIZE + Vector3.up * 5 + Vector3.left * 5;
        transform.localPosition = targetPosition;
    }
}
