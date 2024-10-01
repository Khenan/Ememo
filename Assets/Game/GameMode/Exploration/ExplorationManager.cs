using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ExplorationManager : Singleton<ExplorationManager>
{
    public List<GameObject> garbage = new();
    public List<PhotonView> photonGarbage = new();
    private bool _init = false;

    private void Update()
    {
        if (!_init)
        {
            PlayerController _playerControllerLocal = GameManager.I.GetLocalPlayerController();
            if (_playerControllerLocal != null)
            {
                InitWorldMaps(_playerControllerLocal);
                InitPlayerCharacter(_playerControllerLocal);
                _init = true;
            }
        }
    }

    private void InitWorldMaps(PlayerController _playerController)
    {
        if (_playerController == null) Debug.LogError("PlayerControllerLocal is null");
        else
        {
            WorldMapManager.I.LoadMapAndAroundByMatrixPosition(_playerController.WorlMapMatrixPosition);
        }
    }

    private void InitPlayerCharacter(PlayerController _playerController)
    {
        if (_playerController != null)
        {
            Character _character = _playerController.Character;
            if (_character != null)
            {
                Map _map = GetMapAssetByMatrixPosition(_playerController.WorlMapMatrixPosition);
                ExplorationMapTile _tile = (ExplorationMapTile)_map.GetTileByMatrixPositionWorld(_playerController.WorldTileMatrixPositionBase);
                if (_tile != null) SwitchTileCharacter(_character, _tile);
                else Debug.LogError("Player WorldTile is null");
                _character.SetCharacterMode(CharacterMode.Exploration);
            }
            else Debug.LogError("Character is null");
        }
        else Debug.LogError("PlayerControllerLocal is null");
    }

    private Map GetMapAssetByMatrixPosition(Vector2Int _matrixPosition)
    {
        return WorldMapManager.I.GetMapByMatrixPosition(_matrixPosition);
    }

    internal void SwitchTileCharacter(Character _character, ExplorationMapTile _tile, bool _moveCharacter = true)
    {
        if (_character == null || _tile == null)
        {
            Debug.LogError("Character or Tile is null");
            return;
        }

        ExplorationMapTile _oldTile = (ExplorationMapTile)_character.CurrentTile;

        _character.CurrentTile = _tile;
        _tile.characters.Add(_character);
        if (_moveCharacter) _character.transform.position = _tile.transform.position;
        if (_oldTile != null && _oldTile.characters.Contains(_character)) _oldTile.characters.Remove(_character);

        CheckIfMapChange(_tile);
    }

    private void CheckIfMapChange(ExplorationMapTile tile)
    {
        PlayerController _playerControllerLocal = GameManager.I.GetLocalPlayerController();
        if (tile.map.matrixPosition != _playerControllerLocal.WorlMapMatrixPosition)
        {
            _playerControllerLocal.WorlMapMatrixPosition = tile.map.matrixPosition;
            InitWorldMaps(_playerControllerLocal);
        }
    }

    public void GoToFight(PlayerController _playerController, FightData _fightData)
    {
        ClearGarbage();
        List<PlayerController> _playerControllers = _playerController.groupPlayerControllers;
        _playerControllers.Add(_playerController);
        GameManager.I.GoToFight(_playerControllers, _fightData);
    }

    public FightData GetFightDataByMatrixPositionWorld(Vector2Int _matrixPositionWorld)
    {
        FightData _fightDataToReturn = null;
        PlayerController _playerControllerLocal = GameManager.I.GetLocalPlayerController();
        Map _map = WorldMapManager.I.GetMapByMatrixPosition(_playerControllerLocal.WorlMapMatrixPosition);
        if (_map != null)
        {
            List<FightData> _fightDatas = ((ExplorationMap)_map).CurrentFightDatas;
            foreach (FightData _fightData in _fightDatas)
            {
                if (_fightData.matrixPositionWorld == _matrixPositionWorld)
                {
                    _fightDataToReturn = _fightData;
                    break;
                }
            }
        }
        return _fightDataToReturn;
    }
    internal bool CheckIfFightOnWorldTile(PlayerController _playerController, Vector2Int _matrixPositionWorld)
    {
        FightData _fightData = GetFightDataByMatrixPositionWorld(_matrixPositionWorld);
        if (_fightData != null)
        {
            Debug.Log(_fightData.name, _fightData);
            GoToFight(_playerController, _fightData);
        }
        return _fightData != null;
    }

    public void CheckLineOfSightExploration(MapTile _centerTile)
    {
        if (_centerTile != null)
        {
            int _range = 20;
            int _lineOfSight = 10;
            List<MapTile> _allTiles = ConcatenatorMapList.ConcatenateMaps(WorldMapManager.I.CurrentMaps);
            List<MapTile> _tiles = MapManager.I.GetTilesByRangeInTemporaryList(_allTiles, _centerTile, 0, _range);
            Debug.Log("_tiles.Count: " + _tiles.Count);
            foreach (MapTile _tile in _tiles)
            {
                if (_tile != null && _tile != _centerTile)
                {
                    if (!MapManager.I.IsTileInRange(_tiles, _centerTile, _tile, 0, _lineOfSight, true))
                    {
                        _tile.DisplayTips(true, Colors.I.Dark, TipsType.Blind);
                        _tile.IsVisible = false;
                    }
                    else
                    {
                        _tile.DisplayTips(false);
                        _tile.IsVisible = true;
                    }
                }
            }
        }
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
    public void AddPhotonGarbage(PhotonView _view)
    {
        photonGarbage.Add(_view);
    }

    public void ClearPhotonGarbage()
    {
        foreach (PhotonView _view in photonGarbage)
        {
            PhotonNetwork.Destroy(_view);
        }
    }
    public void ClearAllGarbage()
    {
        ClearGarbage();
        ClearPhotonGarbage();
    }
}