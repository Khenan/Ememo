using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private Character character;

    public void Init(Character _character)
    {
        character = _character;
    }

    public void Play()
    {
        // On récupère la liste des actions possibles
        int _pa = character.CurrentActionPoints;
        // On regarde si un Character allié est à portée ou un Character ennemi est à portée
        // Si non, on s'approche d'un ennemi

        // On choisi une action possible aléatoire
        List<Spell> _possibleSpells = character.Spells.FindAll(spell => spell.Data.cost <= _pa);
        // On effectue l'action
        //Spell _spell = _possibleSpells[Random.Range(0, _possibleSpells.Count)];
        // On regarde si on peut encore jouer
        // Si oui, on recommence
        // On fini le tour
    }
}