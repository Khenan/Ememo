using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "SpellData", menuName = "SpellData", order = 1)]
// QUESTION ???
// Pourquoi pas des scriptable Object a inserer directement dans le spell qui finalement fait la meme chose mais tout change selon les data
public class SpellData : ScriptableObject
{
    public new string name;
    public string description;
    public int cost;
    public int level;
    public bool withSight;
    public int rangeMin;
    public int rangeMax;
    public int damage;
}