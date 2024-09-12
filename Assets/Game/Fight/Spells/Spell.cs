using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private SpellData data;
    public SpellData Data => data;
    public void Effect(Character target)
    {
        target.Data.currentHealth -= Data.damage;
        if (target.Data.currentHealth <= 0)
        {
            target.Data.currentHealth = 0;
        }
    }

    // gapCase = (Nombre de case entre personnage lanceur et case survolée par la souris)
    //public bool CanBeLaunch()
    //{
    //    return data.rangeMin <= gapCase && data.rangeMax >= gapCase;
    //}



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
