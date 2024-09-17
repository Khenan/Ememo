using System.Collections.Generic;
using UnityEngine;

namespace MatrixNavigation
{
    enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
    }
    public static class MatrixDirection
    {
        internal static readonly Dictionary<Direction, Vector2> matrixDirections = new()
        {
            { Direction.UP, new Vector2(0, -1) },
            { Direction.DOWN, new Vector2(0, 1) },
            { Direction.RIGHT, new Vector2(1, 0) },
            { Direction.LEFT, new Vector2(-1, 0) }
        };
    }
}