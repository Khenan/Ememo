using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject turnArrow;

    internal void SetHealthBar(int _current, int _max)
    {
        healthBar.Value = _current;
        healthBar.MaxValue = _max;
        healthBar.updateBar();
    }
    internal void SetTurnArrow(bool isActive)
    {
        turnArrow.SetActive(isActive);
    }
}