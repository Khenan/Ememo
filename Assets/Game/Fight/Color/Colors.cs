using System.Collections.Generic;
using UnityEngine;

public class Colors : Singleton<Colors>
{
    public Color DefaultHightlight = new Color(0, 1, 0, 1);
    public List<Color> TeamColors = new();
    public Color Red = new Color(1, 0, 0, 1);
    public Color SpellHighlight  = new Color(1, 1, 1, 1);
    public Color HoverHighlight = new Color(0, 1, 0, 1);

    public Color FloorOdd = new Color(0, 1, 0, 1);
    public Color FloorEven = new Color(0, 1, 0, 1);
    public Color PMPath = new Color(0, 1, 0, 1);
    public Color PMPathHoverCharacter = new Color(0, 1, 0, 1);
}