// On pourrait acceder a tout ce que possede un spell via le SpellData et quelque fonctions que je considere mieux d'avoir en acces libre

public class Spell
{
    protected SpellData spellData;
    public SpellData SpellData => spellData;
    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
    }

    public void Effect(Character _target)
    {

    }
    public (int, int) DisplayRange()
    {
        return (spellData.RangeMin, spellData.RangeMax);
    }
    public string DisplayDescription()
    {
        return spellData.Description;
    }
    public string DisplayName()
    {
        return spellData.Name;
    }
}
