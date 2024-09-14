using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class SpellBarUISlot : MonoBehaviour, IPointerClickHandler
{
    private int cost;
    private int index;
    private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Vector2 size;
    [SerializeField] private Image disableImage;

    private PlayerController playerController;
    private void Awake()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    internal void Init(PlayerController _playerController, int _cost, string _spellName, int _index)
    {
        playerController = _playerController;
        cost = _cost;
        textMeshProUGUI.text = _spellName;
        index = _index;
        transform.localPosition = Vector3.right * size.x * _index + Vector3.up * 5 + Vector3.right * 5;
    }
    internal void UpdateAble(int _currentPAOfCharacter)
    {
        disableImage.gameObject.SetActive(_currentPAOfCharacter < cost);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (disableImage.gameObject.activeSelf) return;
        playerController.SelectionSpell(index);
    }
}