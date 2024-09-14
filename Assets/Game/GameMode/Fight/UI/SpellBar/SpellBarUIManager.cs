using System.Collections.Generic;
using UnityEngine;

public class SpellBarUIManager : Singleton<SpellBarUIManager>
{
    [SerializeField] private SpellBarUISlot spellBarUISlotPrefab;
    [SerializeField] private Transform root;
    List<SpellBarUISlot> spellBarUISlots = new List<SpellBarUISlot>();

    public void Init(PlayerController _playerController, List<SpellData> _spellDatas, int _currentPAOfCharacter)
    {
        ClearSpellBar();
        for (int _i = 0; _i < _spellDatas.Count; _i++)
        {
            SpellBarUISlot spellBarUISlot = Instantiate(spellBarUISlotPrefab, root);
            spellBarUISlot.Init(_playerController, _spellDatas[_i].cost, _spellDatas[_i].name, _i);
            spellBarUISlots.Add(spellBarUISlot);
        }
        UpdateSpellBar(_currentPAOfCharacter);
    }

    public void UpdateSpellBar(int _currentPAOfCharacter)
    {
        for (int i = 0; i < spellBarUISlots.Count; i++)
        {
            spellBarUISlots[i].UpdateAble(_currentPAOfCharacter);
        }
    }

    private void ClearSpellBar()
    {
        foreach (var _spellBarUISlot in spellBarUISlots)
        {
            Destroy(_spellBarUISlot.gameObject);
        }
        spellBarUISlots.Clear();
    }
}