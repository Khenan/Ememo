using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "SpellData", menuName = "SpellData", order = 1)]
// QUESTION ???
// Pourquoi pas des scriptable Object a inserer directement dans le spell qui finalement fait la meme chose mais tout change selon les data
public class SpellData : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int cost;
    [SerializeField] private int level;
    [SerializeField] private int rangeMin;
    [SerializeField] private int rangeMax;

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