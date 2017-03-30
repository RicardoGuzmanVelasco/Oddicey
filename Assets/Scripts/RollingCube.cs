using Extensions; //Own namespace for extension methods.
using System.Collections;
using UnityEngine;

public class RollingCube : MonoBehaviour
{
	Vector2 pivot, extents;
	public float floor; //'y' value before roll.
	bool rolling = false;
	public float rollingTime = 1;

	public Vector2 dir = Vector2.right;

	public bool grounding = false;
	float threshold = 30; //Min degrees difference to consider grounding.

	void Awake()
	{
		/// <remarks>
		/// The collider, not the sprite, determines the actual size of the figure.
		/// </remarks>
		extents = GetComponent<Collider2D>().bounds.extents;
	}

	public void Roll()
	{
		if (dir == Vector2.right)
			RollForward();
		else if (dir == Vector2.left)
			RollBackward();
	}

	void RollForward()
	{
		if (rolling)
			return;
		pivot = new Vector2(transform.position.x + extents.x, transform.position.y - extents.y);
		StartCoroutine(Roll(Vector3.forward));
	}

	void RollBackward()
	{
		if (rolling)
			return;
		pivot = new Vector2(transform.position.x - extents.x, transform.position.y - extents.y);
		StartCoroutine(Roll(Vector3.back));
	}

	IEnumerator Roll(Vector3 axis)
	{
		float instants = Mathf.Ceil(rollingTime * 1 / Time.fixedDeltaTime); // The constant normalizes angular operations and frames.
		float instantAngle = 90f / instants;

		//Set and take values.
		rolling = true;
		floor = transform.position.y;

		grounding = false;
		for (int i = 0; i < instants; i++)
		{
			transform.RotateAround(pivot, axis, -instantAngle); //Left-hand rule.
			if (instantAngle * i >= 90 - threshold)
				grounding = true;
			yield return new WaitForEndOfFrame();
		}

		//Reset, free and snap properly.
		pivot = transform.position;
		transform.position = new Vector3(transform.position.x, floor, transform.position.z);
		transform.eulerAngles = transform.eulerAngles.Snap(90);
		rolling = false;
	}

	public void Turn()
	{
		dir *= -1;
	}

	public void Turn(Vector2 newDir)
	{
		dir = newDir;
	}
}