using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabaseManager : Singleton<CharacterDatabaseManager>
{
    public List<Character> characters = new();
    public Character GetCharacterById(int _id)
    {
        if (_id < 0 || _id >= characters.Count)
        {
            Debug.LogError("Character id " + _id + " is out of range.");
            return null;
        }
        return characters[_id];
    }
}