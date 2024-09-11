public abstract class Spell
{
    protected SpellData spellData;

    public abstract void Effect();
}
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