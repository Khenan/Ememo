using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class FightRoom : MonoBehaviour
{
    // AI Timer
    private float timerMax = 0.25f;
    private float currentTimer = 0f;

    // Players
    private List<PlayerController> playerControllers;
    public List<PlayerController> PlayerControllers => playerControllers;

    // Teams
    private int teamCount = 0;

    // Characters
    private List<Character> characters = new();
    private Character currentCharacter;

    // Maps
    private List<FightMap> currentMaps = new();
    public List<FightMap> CurrentMaps => currentMaps;

    // Garbage
    public List<PhotonView> photonGarbage = new();
    public List<GameObject> garbage = new();

    private bool onFight = false;
    private bool fightIsOver = false;
    private FightData fightData;
    public FightData FightData => fightData;

    public void Init(List<PlayerController> _playerControllers, FightData _fightData)
    {
        playerControllers = _playerControllers;
        fightData = _fightData;
    }

    public void LaunchFight()
    {
        LockAllPlayersOnFight();
        currentCharacter = characters.First();
        currentCharacter.StartTurn();
        UpdateInitiativeUI();
        if (FightMapManager.I != null) FightMapManager.I.StartFight();
    }

    public void InitFight(FightData _fightData)
    {
        FightMapManager.I.InitMap(this, _fightData.AreaId);
        InitAllCharactersAndPlayers();
        InitReadyPlayerActions();
        InitAllCharacterDatas();
        InitInitiativeList();
        FightManager.I.UpdateUILocalPlayer(this);
    }


    #region Initialisation Methods

    private void InitAllCharacterDatas()
    {
        foreach (Character _character in characters)
        {
            _character.InitData();
        }
    }

    private void InitReadyPlayerActions()
    {
        foreach (PlayerController _player in playerControllers)
        {
            _player.OnPlayerReady += CheckAllPlayersReady;
        }
    }

    private void CheckAllPlayersReady(FightRoom _fightRoom)
    {
        bool _allIsReady = _fightRoom.playerControllers.All(player => player.IsReadyToFight);
        if (_allIsReady)
        {
            _fightRoom.LaunchFight();
        }
    }

    private void InitInitiativeList()
    {
        // On trie les personnages par initiative
        characters = characters.OrderByDescending(character => character.CurrentData.currentInitiative).ToList();
        // On affiche la barre d'initiative
        if (InitiativeUIManager.I != null) InitiativeUIManager.I.Init(characters);
    }

    private void InitAllCharactersAndPlayers()
    {
        List<FightMapTile> _tiles = new();
        foreach (FightMap _map in currentMaps) _tiles.AddRange(_map.GetStartTiles());
        if (_tiles.Count == 0) return;

        SetAllCharacters(_tiles);
        SetAllPlayers(_tiles);
        teamCount = characters.Max(_character => _character.teamId);
    }

    private void SetAllPlayers(List<FightMapTile> _tiles)
    {
        List<FightMapTile> _teamPlayerTiles = _tiles.Where(tile => tile.TeamId == 0).ToList();
        SetAllCharactersOfPlayerTeam(playerControllers, _teamPlayerTiles);
    }

    private void SetAllCharacters(List<FightMapTile> _tiles)
    {
        int _teamCount = _tiles.Max(tile => tile.TeamId) + 1;

        for (int i = 1; i < _teamCount; i++)
        {
            List<FightCharacter> _teamCharacters = fightData.charactersToInstantiate.Where(fCharacter => fCharacter.teamId == i).ToList();
            List<FightMapTile> _teamTiles = _tiles.Where(tile => tile.TeamId == i).ToList();
            SetAllFightCharactersOfTeam(_teamCharacters, _teamTiles);
        }
    }

    private void SetAllFightCharactersOfTeam(List<FightCharacter> _fCharacters, List<FightMapTile> _tiles)
    {
        List<Character> _teamCharacters = new();
        foreach (FightCharacter _fCharacter in _fCharacters)
        {
            if (_fCharacter.character == null) continue;
            _fCharacter.character.teamId = _fCharacter.teamId;
            _teamCharacters.Add(_fCharacter.character);
        }
        SetAllCharactersOfTeam(_teamCharacters, _tiles);
    }

    private void SetAllCharactersOfTeam(List<Character> _charactersToInstantiate, List<FightMapTile> _tiles)
    {
        List<FightMapTile> _teamTiles = new List<FightMapTile>(_tiles);
        while (_charactersToInstantiate.Count > 0)
        {
            int _randomTileIndex = UnityEngine.Random.Range(0, _teamTiles.Count);
            int _randomCharacterIndex = UnityEngine.Random.Range(0, _charactersToInstantiate.Count);
            Character _character = InstantiateCharacter(_charactersToInstantiate[_randomCharacterIndex], _teamTiles, _randomTileIndex);
            characters.Add(_character);

            _charactersToInstantiate.RemoveAt(_randomCharacterIndex);
            _teamTiles.RemoveAt(_randomTileIndex);

            if (_teamTiles.Count == 0)
            {
                Debug.LogError("Not enough tiles for characters", this);
                return;
            }
        }
    }

    private void AddPhotonGarbage(PhotonView _view)
    {
        photonGarbage.Add(_view);
    }

    private Character InstantiateCharacter(Character _characterToInstantiate, List<FightMapTile> _tiles, int _tileIndex)
    {
        Character _character = Instantiate(_characterToInstantiate);
        _character.fightRoom = this;
        _character.transform.position = _tiles[_tileIndex].transform.position;
        _tiles[_tileIndex].character = _character;
        _character.CurrentTile = _tiles[_tileIndex];
        FightManager.I.SetCharacterOnTile(_character, _tiles[_tileIndex], (FightMap)_tiles[_tileIndex].map);
        AddPhotonGarbage(_character.GetComponent<PhotonView>());
        _character.SetCharacterMode(CharacterMode.Fight);
        return _character;
    }

    private void SetAllCharactersOfPlayerTeam(List<PlayerController> _players, List<FightMapTile> _tiles)
    {
        List<PlayerController> _teamPlayers = new List<PlayerController>(_players);
        List<FightMapTile> _teamTiles = new List<FightMapTile>(_tiles);
        while (_teamPlayers.Count > 0)
        {
            int _randomTileIndex = Random.Range(0, _teamTiles.Count);
            Character _character = _teamPlayers[0].Character;
            _character.teamId = 0;
            FightMapManager.I.SetCharacterOnTile(_character, _teamTiles[_randomTileIndex], (FightMap)_teamTiles[_randomTileIndex].map, true);
            characters.Add(_character);

            _teamPlayers.RemoveAt(0);
            _teamTiles.RemoveAt(_randomTileIndex);

            if (_teamTiles.Count == 0 && _teamPlayers.Count > 0)
            {
                Debug.LogError("Not enough tiles for characters", this);
                return;
            }
        }
    }
    #endregion

    private void Update()
    {
        if (currentCharacter != null && !currentCharacter.isHumanController)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= timerMax)
            {
                EndTurn(currentCharacter);
                currentTimer = 0f;
            }
        }
    }

    private void LockAllPlayersOnFight()
    {
        foreach (PlayerController _player in playerControllers)
        {
            _player.StartFight();
        }
    }

    public void EndTurn(Character _character)
    {
        if (fightIsOver) return;
        _character.EndTurn();

        int _currentCharacterIndex = characters.IndexOf(_character);
        int _nextCharacterIndex = _currentCharacterIndex + 1;
        if (_nextCharacterIndex >= characters.Count)
        {
            _nextCharacterIndex = 0;
        }

        currentCharacter = characters[_nextCharacterIndex];
        if (!currentCharacter.IsDead)
        {
            currentCharacter.StartTurn();
            UpdateInitiativeUI();
        }
        else
        {
            EndTurn(currentCharacter);
        }
    }

    internal void OnCharacterDead(Character _character)
    {
        if (InitiativeUIManager.I != null) InitiativeUIManager.I.CharacterDead(characters.IndexOf(_character));
        FightMapTile _tile = (FightMapTile)_character.CurrentTile;
        _tile.character = null;
        CheckEndFight();
    }

    private void CheckEndFight()
    {
        // Check le nombre de team en vie
        int _teamsAlive = characters.Where(character => !character.IsDead).Select(character => character.teamId).Distinct().Count();
        if (_teamsAlive == 1)
        {
            fightIsOver = true;
            Debug.Log("Fight is over");
            Debug.Log("Team " + characters.First(character => !character.IsDead).teamId + " wins");
            EndFight();
        }
    }

    private void EndFight()
    {
        FightManager.I.EndFight(this);
    }

    internal void UpdateInitiativeUI()
    {
        if (InitiativeUIManager.I != null) InitiativeUIManager.I.UpdateTurn(characters.IndexOf(currentCharacter));
    }
}
