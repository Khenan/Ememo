// On pourrait acceder a tout ce que possede un spell via le SpellData et quelque fonctions que je considere mieux d'avoir en acces libre

public class Spell
{
    protected SpellData data;
    public SpellData Data => data;

    public Spell(SpellData spellData)
    {
        this.data = spellData;
    }

    public void Effect(Character target)
    {
        target.Data.currentHealth -= Data.damage;
        if (target.Data.currentHealth <= 0)
        {
            target.Data.currentHealth = 0;
        }
    }
    public (int, int) DisplayRange()
    {
        return (data.rangeMin, data.rangeMax);
    }
    public string DisplayDescription()
    {
        return data.description;
    }
    public string DisplayName()
    {
        return data.name;
    }
}
