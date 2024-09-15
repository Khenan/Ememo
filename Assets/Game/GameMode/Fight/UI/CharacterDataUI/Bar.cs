using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image barCursor;

    public void UpdateBar(float _current, float _max)
    {
        barCursor.fillAmount = _current / _max;
    }
}