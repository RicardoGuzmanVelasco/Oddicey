using UnityEngine;

public class Die : MonoBehaviour
{
	int side = 1;
	public Sprite[] sprites;
	SpriteRenderer renderer;

	float speed = 1; //Public interface for Cube.rollingTime.

	RollingCube cube;

	#region Properties
	public float Speed
	{
		get
		{
			return speed;
		}

		set
		{
			cube.rollingTime = value;
			speed = value;
		}
	}

	public int Side
	{
		get
		{
			return side;
		}
	} 
	#endregion

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		renderer.sprite = sprites[side - 1]; // Internal value smash inspector change.
		cube = GetComponent<RollingCube>();
	}

	public bool flip(int increment = 0)
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
		renderer.sprite = sprites[side - 1];

		return true;
	}

	#region Delegate to rolling motor.
	public void RollForward()
	{
		cube.RollForward();
	}

	public void RollBackward()
	{
		cube.RollBackward();
	}
	#endregion
}
