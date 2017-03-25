using UnityEngine;

public class Die : MonoBehaviour
{
	public int side = 1;
	public Sprite[] sideSprites;
	public SpriteRenderer sideSpriteRenderer;

	float speed = 1; //Public interface for Cube.rollingTime.

	RollingCube cube;

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

	void Awake()
	{
		sideSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		sideSpriteRenderer.sprite = sideSprites[side - 1]; // Internal value smash inspector change.
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
		sideSpriteRenderer.sprite = sideSprites[side - 1];

		return true;
	}

	#region Delegate to rolling motor.
	public void rollForward()
	{
		cube.rollForward();
	}

	public void rollBackward()
	{
		cube.rollBackward();
	} 
	#endregion
}
