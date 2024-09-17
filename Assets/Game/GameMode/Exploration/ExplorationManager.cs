using System;
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
        playerController = GameManager.I.PlayerController;
        InitWorldMaps();
        InitPlayerCharacter();
    }

    private void InitWorldMaps()
    {
        WorldMapManager.I.LoadMapAndAroundByMatrixPosition(playerController.WorlMapMatrixPosition);
    }

    private void InitPlayerCharacter()
    {
        Map _map = GetMapAssetByMatrixPosition(playerController.WorlMapMatrixPosition);
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
                ExplorationMapTile _tile = (ExplorationMapTile)_map.GetTileByMatrixPosition(playerController.WorldTileMatrixPositionBase);
                SwitchTileCharacter(_character, _tile);
                _character.SetCharacterMode(CharacterMode.Exploration);
                AddGarbage(_character.gameObject);
            }
        }
    }

    private Map GetMapAssetByMatrixPosition(Vector2Int _matrixPosition)
    {
        return WorldMapManager.I.GetMapByMatrixPosition(_matrixPosition);
    }

    internal void SwitchTileCharacter(Character _character, ExplorationMapTile _tile)
    {
        if(_character == null || _tile == null)
        {
            Debug.LogError("Character or Tile is null");
            return;
        }

        ExplorationMapTile _oldTile = (ExplorationMapTile)_character.CurrentTile;

        _character.CurrentTile = _tile;
        _tile.characters.Add(_character);
        _character.transform.position = _tile.transform.position;
        if(_oldTile != null && _oldTile.characters.Contains(_character)) _oldTile.characters.Remove(_character);

        CheckIfMapChange(_tile);
    }

    private void CheckIfMapChange(ExplorationMapTile tile)
    {
        if(tile.map.matrixPosition != playerController.WorlMapMatrixPosition)
        {
            playerController.WorlMapMatrixPosition = tile.map.matrixPosition;
            InitWorldMaps();
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