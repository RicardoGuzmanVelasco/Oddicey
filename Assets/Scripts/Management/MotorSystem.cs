using UnityEngine;

public class MotorSystem : MonoBehaviour
{
	Die die;
	bool moving = false;

	float counter = 0;

	/// <summary>
	/// Tempo in BPM for movement adjustment.
	/// </summary>
	/// <value>
	/// Current music tempo in BPM.
	/// </value>
	[Range(30, 200)]
	public int tempo = 120;
	/// <summary>
	/// Ratio between beeps and player movements.
	/// </summary>
	/// <example>
	/// tempo=120 ^ beep=4 == tempo=30
	/// </example>
	[Range(1, 4)]
	public int beat = 1;
	/// <summary>
	/// Actual time for 1 player movement in seconds. Internal use only.
	/// </summary>
	float beepTime;

	bool recalculatePending = false;

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
			{
				RecalculateMovement();
				counter = beepTime; //Force beat.
			}
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
			recalculatePending = true;
		}
	}

	public int Beat
	{
		get
		{
			return beat;
		}

		set
		{
			beat = value;
			recalculatePending = true;
		}
	}
	#endregion

	void Awake()
	{
		die = GetComponent<InputManager>().player.GetComponent<Die>();
	}

	void RecalculateMovement()
	{
		beepTime = 60f / tempo * beat;
		die.GetComponent<RollingCube>().RollingTime = beepTime;
	}

	/// <summary>
	/// Send to the player a rolling boost each beep.
	/// Assume it will not be called if the player should not be moved.
	/// </summary>
	void Update()
	{
		if (!moving)
			return;

		counter += Time.deltaTime;

		if (counter >= beepTime)
		{
			GetComponent<AudioSource>().Play();
			counter -= beepTime;
			if (recalculatePending)
			{
				RecalculateMovement();
				recalculatePending = false;
			}
			GetComponent<Notifier>().NotificateBeep();
			die.GetComponent<RollingCube>().Roll();
		}
	}
}
