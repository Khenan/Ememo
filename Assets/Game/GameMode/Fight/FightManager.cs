using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    // Timer
    private float timerMax = 0.25f;
    private float currentTimer = 0f;

    // Teams
    private int teamCount = 0;

    // Characters
    private List<Character> characters = new();
    private Character currentCharacter;

    // Players
    private List<PlayerController> players = new();

    private bool onFight = false;
    private bool fightIsOver = false;

    // Garbage
    private List<GameObject> garbage = new();

    [SerializeField] private FightData fightData;
    private FightMap currentMap;

    public void EnterFight(FightData _fightData)
    {
        fightData = _fightData;
        InitFight(fightData);
        fightIsOver = false;
    }

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

    private void SetPlayersOnFight()
    {
        foreach (PlayerController _player in players)
        {
            _player.onFight = true;
        }
    }

    private void CheckAllPlayersReady()
    {
        onFight = players.All(player => player.IsReadyToFight);
        if (onFight)
        {
            LaunchFight();
        }
    }

    public void LaunchFight()
    {
        LockAllPlayersOnFight();
        currentCharacter = characters.First();
        currentCharacter.StartTurn();
        UpdateInitiativeUI();
        if (FightMapManager.I != null) FightMapManager.I.StartFight();
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
        _character.CurrentTile.character = null;
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
        ClearGarbage();
        UnlockAllPlayersOnFight();
        GameManager.I.ExitFightMode();
    }

    private void UnlockAllPlayersOnFight()
    {
        foreach (PlayerController _player in players)
        {
            _player.EndFight();
        }
    }

    public void AddGarbage(GameObject _go)
    {
        garbage.Add(_go);
    }

    private void ClearGarbage()
    {
        foreach (GameObject _go in garbage)
        {
            Destroy(_go);
        }
    }

    private void LockAllPlayersOnFight()
    {
        foreach (PlayerController _player in players)
        {
            _player.StartFight();
        }
    }

    private void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        FightMapManager.I.SetCharacterOnTile(_character, _fightMapTile, _map);
    }

    internal void CastSpell(SpellData _currentSpellSelected, FightMapTile _tile)
    {
        Debug.Log("Cast spell " + _currentSpellSelected.name + " on tile " + _tile.Position);
        // On check si il y a un character sur la tile
        if (_tile.character != null)
        {
            Debug.Log("Target: " + _tile.character.CharacterName);
            _tile.character.TakeDamage(_currentSpellSelected.damage);
            UpdateUILocalPlayer();
        }
    }

    #region Initialisation Methods

    public void InitFight(FightData _fightData)
    {
        characters.Clear();
        currentMap = FightMapManager.I.InitMap(_fightData.AreaId);
        InitAllCharactersAndPlayers();
        InitPlayerActions();
        InitAllCharacterDatas();
        InitInitiativeList();
        SetPlayersOnFight();
        UpdateUILocalPlayer();
    }

    private void InitAllCharacterDatas()
    {
        foreach (Character _character in characters)
        {
            _character.InitData();
        }
    }

    private void InitPlayerActions()
    {
        foreach (PlayerController _player in players)
        {
            _player.OnPlayerReady += CheckAllPlayersReady;
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
        List<FightMapTile> _tiles = currentMap.GetStartTiles();

        if (_tiles.Count == 0)
        {
            Debug.LogError("No start tiles found", this);
            return;
        }

        SetAllCharacters(_tiles);
        SetAllPlayers(_tiles);
        teamCount = characters.Max(_character => _character.teamId);
    }

    private void SetAllCharacters(List<FightMapTile> _tiles)
    {
        int _teamCount = _tiles.Max(tile => tile.TeamId) + 1;

        for (int i = 1; i < _teamCount; i++)
        {
            List<FightCharacter> _teamCharacters = fightData.CharactersToInstantiate.Where(fCharacter => fCharacter.teamId == i).ToList();
            List<FightMapTile> _teamTiles = _tiles.Where(tile => tile.TeamId == i).ToList();
            SetAllFightCharactersOfTeam(_teamCharacters, _teamTiles);
        }
    }

    private void SetAllPlayers(List<FightMapTile> _tiles)
    {
        players = FindObjectsOfType<PlayerController>().ToList();
        List<FightMapTile> _teamPlayerTiles = _tiles.Where(tile => tile.TeamId == 0).ToList();
        SetAllCharactersOfPlayerTeam(players, _teamPlayerTiles);
    }
    private void SetAllFightCharactersOfTeam(List<FightCharacter> _fCharacters, List<FightMapTile> _tiles)
    {
        List<Character> _teamCharacters = new();
        foreach (FightCharacter _fCharacter in _fCharacters)
        {
            _fCharacter.character.teamId = _fCharacter.teamId;
            _teamCharacters.Add(_fCharacter.character);
        }
        SetAllCharactersOfTeam(_teamCharacters, _tiles);
    }

    private void SetAllCharactersOfTeam(List<Character> _characters, List<FightMapTile> _tiles)
    {
        List<FightMapTile> _teamTiles = new List<FightMapTile>(_tiles);
        while (_characters.Count > 0)
        {
            int _randomTileIndex = UnityEngine.Random.Range(0, _teamTiles.Count);
            int _randomCharacterIndex = UnityEngine.Random.Range(0, _characters.Count);
            Character _character = InstantiateCharacter(_characters[_randomCharacterIndex], _teamTiles, _randomTileIndex);
            characters.Add(_character);
            AddGarbage(_character.gameObject);

            _characters.RemoveAt(_randomCharacterIndex);
            _teamTiles.RemoveAt(_randomTileIndex);

            if (_teamTiles.Count == 0)
            {
                Debug.LogError("Not enough tiles for characters", this);
                return;
            }
        }
    }

    private void SetAllCharactersOfPlayerTeam(List<PlayerController> _players, List<FightMapTile> _tiles)
    {
        List<PlayerController> _teamPlayers = new List<PlayerController>(_players);
        List<FightMapTile> _teamTiles = new List<FightMapTile>(_tiles);
        while (_teamPlayers.Count > 0)
        {
            int _randomTileIndex = UnityEngine.Random.Range(0, _teamTiles.Count);
            Character _character = InstantiateCharacter(_teamPlayers[0].CharacterToInstantiate, _teamTiles, _randomTileIndex);
            characters.Add(_character);
            _teamPlayers[0].SetCharacter(_character);

            _teamPlayers.RemoveAt(0);
            _teamTiles.RemoveAt(_randomTileIndex);

            if (_teamTiles.Count == 0)
            {
                Debug.LogError("Not enough tiles for characters", this);
                return;
            }
        }
    }

    private Character InstantiateCharacter(Character _characterToInstantiate, List<FightMapTile> _tiles, int _tileIndex)
    {
        Character _character = Instantiate(_characterToInstantiate);
        _character.transform.position = _tiles[_tileIndex].transform.position;
        _tiles[_tileIndex].character = _character;
        _character.CurrentTile = _tiles[_tileIndex];
        SetCharacterOnTile(_character, _tiles[_tileIndex], currentMap);
        AddGarbage(_character.gameObject);
        return _character;
    }
    #endregion

    #region UI

    public void UpdateUILocalPlayer()
    {
        players.Find(player => player.IsLocalPlayer).UpdateHUDUI();
    }
    private void UpdateInitiativeUI()
    {
        if (InitiativeUIManager.I != null) InitiativeUIManager.I.UpdateTurn(characters.IndexOf(currentCharacter));
    }

    #endregion
}