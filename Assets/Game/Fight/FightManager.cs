using System;
using System.Collections;
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

    [SerializeField] private FightData fightData;
    private FightMap currentMap;
    private void Start()
    {
        LaunchFight(fightData);
    }
    public void LaunchFight(FightData _fightData)
    {
        currentMap = FightMapManager.Instance.GetMap(_fightData.AreaId);
        InitCharcterPosition();
        // Générer la barre d'initiative
        // On affiche le jeu
    }

    private void InitCharcterPosition()
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
        List<Character> _characters = new();
        foreach (PlayerController _player in _players)
        {
            _characters.Add(_player.Character);
        }
        List<FightMapTile> _teamPlayerTiles = _tiles.Where(tile => tile.TeamId == 0).ToList();
        SetAllCharactersOfTeam(_characters, _teamPlayerTiles);
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

            Debug.Log(_characters[_randomCharacterIndex].CharacterName);

            Character _character = Instantiate(_characters[_randomCharacterIndex]);
            _character.transform.position = _teamTiles[_randomTileIndex].transform.position;
            _teamTiles[_randomTileIndex].character = _character;
            _character.CurrentTile = _teamTiles[_randomTileIndex];

            _characters.RemoveAt(_randomCharacterIndex);
            _teamTiles.RemoveAt(_randomTileIndex);

            SetCharacterOnTile(_character, _teamTiles[_randomTileIndex], currentMap);

            if(_teamTiles.Count == 0)
            {
                Debug.LogError("Not enough tiles for characters", this);
                return;
            }
        }
    }

    private void SetCharacterOnTile(Character _character, FightMapTile _fightMapTile, FightMap _map)
    {
        FightMapManager.Instance.SetCharacterOnTile(_character, _fightMapTile, _map);
    }
}