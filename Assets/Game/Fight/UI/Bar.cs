using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] public int MaxValue;
    [SerializeField] public int Value;
    [SerializeField] private Image barCursor;

    public void updateBar()
    {
        barCursor.fillAmount = (float) Value/ (float) MaxValue;
    }
}