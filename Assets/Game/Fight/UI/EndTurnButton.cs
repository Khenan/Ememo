using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBehaviour : MonoBehaviour
{
    private Button button;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void Update()
    {
        button.interactable = playerController.Character.isMyTurn;
    }

    private void OnClick()
    {
        FightManager.Instance.EndTurn(playerController.Character);
    }
}
