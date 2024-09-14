using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] private Bar healthBar;
    [SerializeField] private GameObject turnArrow;

    internal void Dead()
    {
        gameObject.SetActive(false);
    }

    internal void SetHealthBar(int _current, int _max)
    {
        healthBar.UpdateBar(_current, _max);
    }
    internal void SetTurnArrow(bool isActive)
    {
        turnArrow.SetActive(isActive);
    }
}