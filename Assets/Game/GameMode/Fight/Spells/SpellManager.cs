using System.Collections.Generic;
using UnityEngine;

public class SpellManager : Singleton<SpellManager>
{
    [SerializeField] private List<SpellData> spellDatas;

    public SpellData GetSpell(string spellName)
    {
        foreach (SpellData spellData in spellDatas)
        {
            if (spellName == spellData.spellName)
            {
                return spellData;
            }
        }
        Debug.Log($"The spell {spellName} not exist");
        return null;
    }

    public SpellData GetSpell(int indexSpell)
    {
        if (indexSpell >= 0 && indexSpell < spellDatas.Count)
            return spellDatas[indexSpell];
        Debug.Log($"Index {indexSpell} in spellDatas not found, out of bounds");
        return null;
    }
}
