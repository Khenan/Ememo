
using UnityEngine;

public class Fist : Spell
{
    public Fist(SpellData spellData) : base(spellData)
    {
    }

    public override void Effect(Character _target)
    {
        Debug.LogWarning("Frappe du poing");
    }
}
