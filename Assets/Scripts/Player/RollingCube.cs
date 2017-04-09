using Utils.Extensions;
using System.Collections;
using UnityEngine;
using System;

public class RollingCube : MonoBehaviour
{
	private bool falling = false;

	Vector2 pivot, extents;
	public float floor; //'y' value before roll.
	public bool rolling = false;

	const float error = 0.95f;
	float rollingTime; //Inverse of speed with error constant. Time to do a roll.

	public Vector2 dir = Vector2.right;

	public bool grounding = false;
	float threshold = 30; //Min degrees difference to consider grounding.

	#region Properties
	public float RollingTime
	{
		get
		{
			return rollingTime;
		}

		set
		{
			rollingTime = value * error;
		}
	}

	public bool Falling
	{
		get
		{
			return falling;
		}

		set
		{
			if (value && !falling)
				StartCoroutine(Fall());
			falling = value;
		}
	}
	#endregion

	void Awake()
	{
		/// <remarks>
		/// The collider, not the sprite, determines the actual size of the figure.
		/// </remarks>
		extents = GetComponent<Collider2D>().bounds.extents;
	}

	public void Roll()
	{
		if (Falling)
			return;

		if (dir == Vector2.right)
			RollForward();
		else if (dir == Vector2.left)
			RollBackward();
	}

	void RollForward()
	{
		if (rolling)
		{
			Debug.LogError("LOST BEEP");
			return;
		}
		pivot = new Vector2(transform.position.x + extents.x, transform.position.y - extents.y);
		StartCoroutine(Roll(Vector3.forward));
	}

	void RollBackward()
	{
		if (rolling)
		{
			Debug.LogError("LOST BEEP");
			return;
		}
		pivot = new Vector2(transform.position.x - extents.x, transform.position.y - extents.y);
		StartCoroutine(Roll(Vector3.back));
	}

	IEnumerator Roll(Vector3 axis)
	{
		float instants = Mathf.Ceil(RollingTime / Time.fixedDeltaTime);
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
			yield return new WaitForFixedUpdate();
		}

		//Reset, free and snap properly.
		pivot = transform.position;
		Snap();
		rolling = false;
	}

	public void Snap()
	{
		transform.position = transform.position.XY(Mathf.RoundToInt(transform.position.x), floor);
		transform.eulerAngles = transform.eulerAngles.Snap(90);
	}

	IEnumerator Fall()
	{
		do
		{
			transform.Translate(Vector2.down * Builder.ToUnits(error / rollingTime) * Time.fixedDeltaTime, Space.World);
			yield return new WaitForFixedUpdate();
		} while (falling);
	}

	public void Turn()
	{
		dir *= -1;
		FindObjectOfType<Notifier>().NotificateTurn();
	}

	public void Turn(Vector2 newDir)
	{
		dir = newDir;
		FindObjectOfType<Notifier>().NotificateTurn();
	}
}