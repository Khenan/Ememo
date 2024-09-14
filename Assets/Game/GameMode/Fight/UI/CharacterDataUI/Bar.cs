using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] public int maxValue;
    [SerializeField] public int value;
    [SerializeField] private Image barCursor;

    public void UpdateBar()
    {
        barCursor.fillAmount = (float) value/ (float) maxValue;
    }
}