using System.Collections;
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
        // Placer aléatoirement les characters sur les cases de départ de l'équipe en question
        currentMap.GetStartTiles().ForEach(tile => {
            if (tile.TeamId == 0)
            {
                // Placer un personnage de l'équipe 0
            }
            else
            {
                // Placer un personnage de l'équipe 1
            }
        });
    }
}