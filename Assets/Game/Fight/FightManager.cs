using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightManager : Singleton<FightManager>
{
    // Demande au MapManager de générer une map
    // Il va donner une map parmi la liste des maps possibles

    // Génère les ennemis avec le FightData

    // Met la barre d'initiative des personnages pour le visuel

    // Si tout le monde est prêt (bouton), on lance le combat
    // Le combat commence par le personnage ayant l'initiative la plus haute

    // On sait qui est le personnage qui joue actuellement

    // On fini le combat si il ne reste plus que une équipe en vie

    // Timer
    private float timerMax = 1f;
    private float currentTimer = 0f;

    List<Character> characters = new();
    private Character currentCharacter;

    [SerializeField] private FightData fightData;
    private FightMap currentMap;
    private void Start()
    {
        InitFight(fightData);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn(currentCharacter);
        }

        if (!currentCharacter.isHumanController)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= timerMax)
            {
                EndTurn(currentCharacter);
                currentTimer = 0f;
            }
        }
    }

    public void InitFight(FightData _fightData)
    {
        characters.Clear();
        currentMap = FightMapManager.Instance.InitMap(_fightData.AreaId);
        InitCharacterPosition();
        InitAllCharacterDatas();
        InitInitiativeList();
        LaunchFight();
    }

    public void LaunchFight()
    {
        currentCharacter = characters.First();
        currentCharacter.isMyTurn = true;
        Debug.Log("StartTurn " + currentCharacter.CharacterName);
    }

    private void InitAllCharacterDatas()
    {
        foreach (Character _character in characters)
        {
            _character.InitData();
        }
    }

    private void InitInitiativeList()
    {
        // On trie les personnages par initiative
        characters = characters.OrderByDescending(character => character.Data.currentInitiative).ToList();
        // On affiche la barre d'initiative
    }

    private void InitCharacterPosition()
    {
        // List des startTiles
        List<FightMapTile> _tiles = currentMap.GetStartTiles();

        if (_tiles.Count == 0)
        {
            Debug.LogError("No start tiles found", this);
            return;
        }

        SetAllCharacters(_tiles);
        SetAllPlayers(_tiles);
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
        List<PlayerController> _players = FindObjectsOfType<PlayerController>().ToList();
        List<FightMapTile> _teamPlayerTiles = _tiles.Where(tile => tile.TeamId == 0).ToList();
        SetAllCharactersOfPlayerTeam(_players, _teamPlayerTiles);
    }
    private void SetAllFightCharactersOfTeam(List<FightCharacter> _fCharacters, List<FightMapTile> _tiles)
    {
        List<Character> _teamCharacters = new();
        foreach (FightCharacter _fCharacter in _fCharacters)
        {
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

            Character _character = Instantiate(_characters[_randomCharacterIndex]);
            _character.transform.position = _teamTiles[_randomTileIndex].transform.position;
            _teamTiles[_randomTileIndex].character = _character;
            _character.CurrentTile = _teamTiles[_randomTileIndex];
            SetCharacterOnTile(_character, _teamTiles[_randomTileIndex], currentMap);
            characters.Add(_character);

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
        return _character;
    }

    private void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        FightMapManager.Instance.SetCharacterOnTile(_character, _fightMapTile, _map);
    }

    public void EndTurn(Character _character)
    {
        Debug.Log("EndTurn " + _character.CharacterName + " | Initiative: " + _character.Data.currentInitiative + " | IsHuman: " + _character.isHumanController);
        _character.isMyTurn = false;

        int _currentCharacterIndex = characters.IndexOf(_character);
        int _nextCharacterIndex = _currentCharacterIndex + 1;
        if (_nextCharacterIndex >= characters.Count)
        {
            _nextCharacterIndex = 0;
        }

        currentCharacter = characters[_nextCharacterIndex];

        Debug.Log("StartTurn " + currentCharacter.CharacterName);
        currentCharacter.isMyTurn = true;
    }
}