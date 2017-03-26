using System;
using System.Collections;
using UnityEngine;

public class MotorSystem : MonoBehaviour
{
	Die die;
	bool moving = false;
	Vector2 dir = Vector2.right;

	/// <summary>
	/// Tempo in BPM for movement adjustment.
	/// </summary>
	/// <value>
	/// Current music tempo in BPM.
	/// </value>
	[SerializeField] int tempo = 120;
	/// <summary>
	/// Ratio between beeps and player movements.
	/// </summary>
	/// <example>
	/// tempo=120 ^ beep=4 == tempo=30
	/// </example>
	[SerializeField] int beep = 1;
	/// <summary>
	/// Actual freq for player movement in hertzs. Internal use only.
	/// </summary>
	float freq;

	#region Properties
	public bool Moving
	{
		get
		{
			return moving;
		}

		set
		{
			bool pastMoving = moving;
			moving = value;
			if (!pastMoving && value)
				MoveInvoke();
			if (pastMoving && !value)
				StopCoroutine("MovePlayer");
		}
	}

	public Vector2 Dir
	{
		get
		{
			return dir;
		}

		set
		{
			if (value != Vector2.right && value != Vector2.left)
				Debug.LogError("MotorSystem can only automove to left or right");
			dir = value;
		}
	}

	public int Tempo
	{
		get
		{
			return tempo;
		}

		set
		{
			tempo = value;
			MoveInvoke();
		}
	}

	public int Beep
	{
		get
		{
			return beep;
		}

		set
		{
			beep = value;
			MoveInvoke();
		}
	}
	#endregion

	void Awake()
	{
		die = GetComponent<InputManager>().player.GetComponent<Die>();
	}

	void MoveInvoke()
	{
		freq = 60f / tempo * beep;
		die.Speed = freq;
		StartCoroutine(MovePlayer());
	}

	/// <summary>
	/// Send to the player a rolling boost.
	/// Assume it will not be called if the player should not be moved.
	/// </summary>
	IEnumerator MovePlayer()
	{
		while (moving)
		{
			GetComponent<Notifier>().NotificateBeep();
			if (dir == Vector2.right)
				die.rollForward();
			else if (dir == Vector2.left)
				die.rollBackward();
			yield return new WaitForSeconds(freq);
		}
	}
}
