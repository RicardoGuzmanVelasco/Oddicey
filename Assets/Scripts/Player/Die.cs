using System;
using UnityEngine;

public class Die : MonoBehaviour
{
	int side = 1;
	public Sprite[] sprites;
	SpriteRenderer spriteRenderer;

	RollingCube cube;

	#region Properties

	public int Side
	{
		get { return side; }
	}

	#endregion

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = sprites[side - 1]; //Internal value smashes inspector change.
		cube = GetComponent<RollingCube>();
	}

	public bool Flip(int increment = 0)
	{
		if (Mathf.Abs(increment) > 2)
			Debug.LogError("Unable to flip an increment of " + increment + "sides.");

		//Check whether flip is enabled or disable it.
		if (!cube.grounding)
			return false;
		else
			cube.grounding = false;

		if (increment == 0)
			increment = 3;

		side += increment;
		if (side < 1)
			side += 6;
		else if (side > 6)
			side -= 6;
		spriteRenderer.sprite = sprites[side - 1];

		return true;
	}
}
