using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class FightManager : Singleton<FightManager>
{

    // FightRooms
    [SerializeField] FightRoom fightRoomPrefab;
    private List<FightRoom> fightRooms = new();

    // Garbage
    private List<GameObject> garbage = new();

    public void EnterFight(List<PlayerController> _playerControllers, FightData _fightData)
    {
        FightRoom _fightRoom = CreateFightRoom(_playerControllers, _fightData);
        fightRooms.Add(_fightRoom);
    }

    private FightRoom CreateFightRoom(List<PlayerController> _playerControllers, FightData _fightData)
    {
        FightRoom _fightRoom = Instantiate(fightRoomPrefab);
        _fightRoom.Init(_playerControllers, _fightData);
        SetPlayersOnFight(_playerControllers, _fightRoom);
        _fightRoom.InitFight(_fightData);
        return _fightRoom;
    }

    private void SetPlayersOnFight(List<PlayerController> _playerControllers, FightRoom _fightRoom)
    {
        foreach (PlayerController _player in _playerControllers)
        {
            _player.onFight = true;
            _player.fightRoom = _fightRoom;
        }
    }

    public void EndFight(FightRoom _fightRoom)
    {
        ClearPhotonGarbage(_fightRoom);
        ClearGarbage(_fightRoom);
        UnlockAllPlayersOnFight(_fightRoom.PlayerControllers);
        GameManager.I.ExitFightMode();
    }

    private void UnlockAllPlayersOnFight(List<PlayerController> _playerControllers)
    {
        foreach (PlayerController _player in _playerControllers)
        {
            _player.EndFight();
        }
    }

    public void AddGarbage(GameObject _go)
    {
        garbage.Add(_go);
    }

    private void ClearGarbage(FightRoom _fightRoom)
    {
        foreach (GameObject _go in _fightRoom.garbage)
        {
            Destroy(_go);
        }
    }
    private void ClearPhotonGarbage(FightRoom _fightRoom)
    {
        foreach (PhotonView _view in _fightRoom.photonGarbage)
        {
            PhotonNetwork.Destroy(_view);
        }
    }

    public void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        FightMapManager.I.SetCharacterOnTile(_character, _fightMapTile, _map);
    }

    internal void CastSpell(FightRoom _fightRoom, SpellData _currentSpellSelected, FightMapTile _tile)
    {
        SpellEffectData _data = new()
        {
            target = _tile.character
        };
        _currentSpellSelected.CastSpell(_data);
        UpdateUILocalPlayer(_fightRoom);
    }

    #region UI

    public void UpdateUILocalPlayer(FightRoom _fightRoom)
    {
        _fightRoom.PlayerControllers.Find(_player => _player.IsLocalPlayer).UpdateHUDUI();
    }

    #endregion
}