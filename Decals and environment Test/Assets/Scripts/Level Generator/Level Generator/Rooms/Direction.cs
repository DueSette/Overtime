using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public static Dictionary<Directions, Directions> directionOpposites = new Dictionary<Directions, Directions>
    {
        {Directions.UP, Directions.DOWN},
        {Directions.DOWN, Directions.UP},
        {Directions.RIGHT, Directions.LEFT},
        {Directions.LEFT, Directions.RIGHT},
    };
}

public enum Directions
{
    UP = 0,
    RIGHT = 90,
    DOWN = 180,
    LEFT = 270
}