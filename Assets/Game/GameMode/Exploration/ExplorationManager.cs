using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ExplorationManager : Singleton<ExplorationManager>
{
    private PlayerController playerControllerLocal;
    public List<GameObject> garbage = new();
    public List<PhotonView> photonGarbage = new();

    public override void Awake()
    {
        base.Awake();
    }

    public void StartExploration()
    {
        playerControllerLocal = GameManager.I.PlayerControllerLocal;
        if(playerControllerLocal.GetComponent<PhotonView>().IsMine) Debug.Log("PlayerControllerLocal: " + playerControllerLocal.name, playerControllerLocal);
        else Debug.LogError("PlayerControllerLocal is not mine");
        InitWorldMaps();
        InitPlayerCharacter();
    }

    private void InitWorldMaps()
    {
        if(playerControllerLocal == null) Debug.LogError("PlayerControllerLocal is null");
        else WorldMapManager.I.LoadMapAndAroundByMatrixPosition(playerControllerLocal.WorlMapMatrixPosition);
    }

    private void InitPlayerCharacter()
    {
        Map _map = GetMapAssetByMatrixPosition(playerControllerLocal.WorlMapMatrixPosition);
        if (playerControllerLocal == null)
        {
            Debug.LogError("PlayerController is null");
        }
        else
        {
            Character _characterToInstantiate = playerControllerLocal.CharacterToInstantiate;
            if (_characterToInstantiate == null)
            {
                Debug.LogError("CharacterToInstantiate is null");
            }
            else
            {
                int _x = PlayerPrefs.GetInt("PlayerXWorldPosition", 0);
                int _z = PlayerPrefs.GetInt("PlayerYWorldPosition", 0);
                Character _character = PhotonNetwork.Instantiate(_characterToInstantiate.name, new Vector3(_x, 0, _z), Quaternion.identity).GetComponent<Character>();
                playerControllerLocal.SetCharacter(_character);
                _character.teamId = 0;
                _character.SetCharacterName(PhotonNetwork.NickName);
                ExplorationMapTile _tile = (ExplorationMapTile)_map.GetTileByMatrixPositionWorld(playerControllerLocal.WorldTileMatrixPositionBase);
                if (_tile != null) SwitchTileCharacter(_character, _tile);
                else Debug.LogError("Player WorldTile is null");
                _character.SetCharacterMode(CharacterMode.Exploration);
                AddPhotonGarbage(_character.GetComponent<PhotonView>());
            }
        }
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
        if (tile.map.matrixPosition != playerControllerLocal.WorlMapMatrixPosition)
        {
            playerControllerLocal.WorlMapMatrixPosition = tile.map.matrixPosition;
            InitWorldMaps();
        }
    }

    public void GoToFight(FightData _fightData)
    {
        ClearGarbage();
        GameManager.I.GoToFight(_fightData);
    }

    public FightData GetFightDataByMatrixPositionWorld(Vector2Int _matrixPositionWorld)
    {
        FightData _fightDataToReturn = null;
        Map _map = WorldMapManager.I.GetMapByMatrixPosition(playerControllerLocal.WorlMapMatrixPosition);
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
    internal bool CheckIfFightOnWorldTile(Vector2Int _matrixPositionWorld)
    {
        FightData _fightData = GetFightDataByMatrixPositionWorld(_matrixPositionWorld);
        if (_fightData != null)
        {
            Debug.Log(_fightData.name, _fightData);
            GoToFight(_fightData);
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