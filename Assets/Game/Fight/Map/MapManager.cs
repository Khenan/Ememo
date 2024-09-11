using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    // Liste des maps possibles
    [SerializeField] private List<Map> maps;
    // Génère une map
    // Retourne une map parmi la liste des maps possibles
}