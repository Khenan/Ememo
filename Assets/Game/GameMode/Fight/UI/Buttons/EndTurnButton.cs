using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBehaviour : Singleton
{
    private Button button;
    private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        button = GetComponent<Button>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        button.onClick.AddListener(OnClick);
        SetPlayerController(GameManager.I.PlayerController);
    }

    public void SetPlayerController(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    private void Update()
    {
        if(playerController == null) return;
        if (playerController.onFight && !playerController.lockOnFight)
        {
            textMeshProUGUI.text = "Ready";
            button.interactable = true;
        }
        else if (playerController.onFight && playerController.lockOnFight)
        {
            textMeshProUGUI.text = "End Turn";
            button.interactable = playerController.Character.isMyTurn;
        }
        else
        {
            textMeshProUGUI.text = "Waiting...";
            button.interactable = false;
        }
    }

    private void OnClick()
    {
        if (playerController.onFight && !playerController.IsReadyToFight)
        {
            playerController.ReadyToFight();
        }
        else if (playerController.onFight && playerController.lockOnFight)
        {
            FightManager.I.EndTurn(playerController.Character);
        }
    }
}
