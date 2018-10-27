﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents any item that can be instantiated in a fixed square of the gameplay world.
/// </summary>
/// <remarks>
/// Despite the fact that some of them are quite "positive" items,
/// if a prefab interacts with player and can be fixedly put on world,
/// then it is considered as an obstacle.
/// </remarks>
/// <example>
/// <see cref="Vane"/> is an obstacle.
/// <see cref="CowBuilder"/> builds cows, also obstacles.
/// <see cref="Post"/> is an obstacle too, regardless the sense of obstacle it has.
/// </example>
/// 
[RequireComponent(typeof(SquareTransform))]
public class Obstacle : MonoBehaviour
{
    static readonly Dictionary<Vector2, Obstacle> grid = new Dictionary<Vector2, Obstacle>();
    public static Dictionary<Vector2, Obstacle> Grid
    {
        get
        {
            return grid;
        }
    }

    void Start ()
	{
        Grid.Add(GetComponent<SquareTransform>().Position, this);
	}
}
