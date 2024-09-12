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
    private void Start() {
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
        // List des character à placer autre que les joueurs
        List<FightCharacter> _charactersToInstantiate = new List<FightCharacter>(fightData.CharactersToInstantiate);

        // List des startTiles
        List<FightMapTile> _tiles = currentMap.GetStartTiles();

        int _teamCount = _tiles.Max(tile => tile.TeamId) + 1;

        for (int i = 0; i < _teamCount; i++)
        {
            List<FightCharacter> _teamCharacters = fightData.CharactersToInstantiate.Where(fCharacter => fCharacter.teamId == i).ToList();
            List<FightMapTile> _teamTiles = _tiles.Where(tile => tile.TeamId == i).ToList();
            SetAllCharacterOfTeam(_teamCharacters, _teamTiles);
        }
    }

    private void SetAllCharacterOfTeam(List<FightCharacter> _fCharacters, List<FightMapTile> _tiles)
    {
        while (_fCharacters.Count > 0)
        {
            int _randomTileIndex = UnityEngine.Random.Range(0, _tiles.Count);
            int _randomCharacterIndex = UnityEngine.Random.Range(0, _fCharacters.Count);

            _fCharacters[_randomCharacterIndex].character.transform.position = _tiles[_randomTileIndex].transform.position;

            _fCharacters.RemoveAt(_randomCharacterIndex);
            _tiles.RemoveAt(_randomTileIndex);
        }
    }
}