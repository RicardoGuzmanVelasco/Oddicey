﻿using UnityEngine;

public class Builder : MonoBehaviour
{
	public const float SquareSize = 4;

	void Awake()
	{
		if (Application.isPlaying)
			Destroy(this);
	}

	public static bool IsSquare(Vector2 position)
	{
		return position.x % SquareSize <= float.Epsilon && position.y % SquareSize <= float.Epsilon;
	}

	public static bool IsSquare(float units)
	{
		return units % SquareSize <= float.Epsilon;
	}

	/// <summary>
	/// Convert from <paramref name="units"/> to game squares.
	/// </summary>
	/// <param name="units">Size in units.</param>
	/// <returns><paramref name="units"/> converted to squares.</returns>
	public static float ToSquares(float units)
	{
		return units / SquareSize;
	}

	/// <summary>
	/// Convert from <paramref name="squares"/> to scene units.
	/// </summary>
	/// <param name="squares">Size in squares.</param>
	/// <returns><paramref name="squares"/> converted to units.</returns>
	public static float ToUnits(float squares)
	{
		return squares * SquareSize;
	}
}
