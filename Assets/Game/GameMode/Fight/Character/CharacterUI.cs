using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject turnArrow;

    internal void SetHealthBar(int _current, int _max)
    {
        healthBar.value = _current;
        healthBar.maxValue = _max;
        healthBar.UpdateBar();
    }
    internal void SetTurnArrow(bool isActive)
    {
        turnArrow.SetActive(isActive);
    }
}