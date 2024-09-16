using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : Singleton<ExplorationManager>
{
    private PlayerController playerController;
    public List<GameObject> garbage = new();
    public override void Awake()
    {
        base.Awake();
    }

    public void StartExploration()
    {
        InitPlayerCharacter();
    }

    private void InitPlayerCharacter()
    {
        Debug.Log("InitPlayerCharacter");
        playerController = GameManager.I.PlayerController;
        if (playerController == null)
        {
            Debug.LogError("PlayerController is null");
        }
        else
        {
            Character _characterToInstantiate = playerController.CharacterToInstantiate;
            if (_characterToInstantiate == null)
            {
                Debug.LogError("CharacterToInstantiate is null");
            }
            else
            {
                Character _character = Instantiate(_characterToInstantiate.gameObject).GetComponent<Character>();
                playerController.SetCharacter(_character);
                _character.SetCharacterMode(CharacterMode.Exploration);
                AddGarbage(_character.gameObject);
            }
        }
    }

    public void GoToFight(FightData _fightData)
    {
        ClearGarbage();
        GameManager.I.GoToFight(_fightData);
    }

    public void AddGarbage(GameObject _go)
    {
        garbage.Add(_go);
    }

    public void ClearGarbage()
    {
        foreach (GameObject _go in garbage)
        {
            Destroy(_go);
        }
    }
}