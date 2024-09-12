// On pourrait acceder a tout ce que possede un spell via le SpellData et quelque fonctions que je considere mieux d'avoir en acces libre
public abstract class Spell
{
    protected SpellData spellData;
    public SpellData SpellData => spellData;
    public Spell(SpellData spellData)
    {
        this.spellData = spellData;
    }

    public virtual void Effect(Character _target)
    {

    }
    public virtual (int, int) DisplayRange()
    {
        return (spellData.RangeMin, spellData.RangeMax);
    }
    public virtual string DisplayDescription()
    {
        return spellData.Description;
    }
    public virtual string DisplayName()
    {
        return spellData.Name;
    }
}

// QUESTION ???
// Pourquoi pas des scriptable Object a inserer directement dans le spell qui finalement fait la meme chose mais tout change selon les data
public class SpellData
{
    private readonly string name;
    private readonly string description;
    private readonly int cost;
    private readonly int level;
    private readonly int rangeMin;
    private readonly int rangeMax;

    public string Name => name;
    public string Description => description;
    public int Cost => cost;
    public int Level => level;
    public int RangeMin => rangeMin;
    public int RangeMax => rangeMax;

    public SpellData(string name, string description, int cost, int level, int rangeMin, int rangeMax)
    {
        this.name = name;
        this.description = description;
        this.cost = cost;
        this.level = level;
        this.rangeMin = rangeMin;
        this.rangeMax = rangeMax;
    }
}