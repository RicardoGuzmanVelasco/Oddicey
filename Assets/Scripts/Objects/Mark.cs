﻿using UnityEngine;

/// <summary>
/// Obstacle that switch on when die side of the player fits with its own. 
/// </summary>
/// <remarks>
/// "Switch on" means here OK state, obstacle passed.
/// </remarks>
public class Mark : Notificable
{
	/// <summary>
	/// Side that will switch on the obstacle.
	/// </summary>
	protected int sideRequired;
	/// <summary>
	/// Wether or not switched on (OK or WRONG state).
	/// </summary>
	private bool state;

	/// <summary>
	/// Player's die reference.
	/// </summary>
	public Die player;

	/// <summary>
	/// Sprites list for each side of the die.
	/// </summary>
	public Sprite[] sprites;
	/// <summary>
	/// Unique renderer of the current side.
	/// </summary>
	protected SpriteRenderer spriteRenderer;

	/// <value>
	/// This value change too the mark skin!
	/// </value>
	public bool State
	{
		get
		{
			return state;
		}

		set
		{
			state = value;
			//Will be changed by animation play. That animation will make the color change.
			spriteRenderer.color = state ? Color.green : Color.red;
		}
	}

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Die>();
	}

	protected override void Start()
	{
		sideRequired = Random.Range(1, 6);
		spriteRenderer.sprite = sprites[sideRequired - 1];
		base.Start(); //Notificable will selfsubscribe.
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			CheckSuccess();
	}

	/// <summary>
	/// Checking state every beep, towards state changes if neccesary.
	/// </summary>
	public override void OnBeep()
	{
		CheckRight();
	}

	/// <summary>
	/// Checking state when die changes side, towards skin changes if neccesary.
	/// </summary>
	public override void OnFlip()
	{
		CheckRight();
	}

	protected virtual void CheckRight()
	{
		if (IsRight())
			OnRight();
		else
			OnWrong();
	}

	protected bool IsRight()
	{
		return player.Side == sideRequired;
	}

	protected virtual void OnRight()
	{
		State = true;
	}

	protected virtual void OnWrong()
	{
		State = false;
	}

	/// <summary>
	/// Actual checking of mark state. Usually happens when die reaches mark trigger.
	/// </summary>
	public void CheckSuccess()
	{
		if (State)
			OnSuccess();
		else
			OnFailure();
	}

	/// <summary>
	/// Obstacle passed.
	/// </summary>
	protected virtual void OnSuccess()
	{
		//TO-DO: replay by effect on gameplay.
		Debug.Log("Success");
	}
	
	/// <summary>
	/// Obstacle not passed.
	/// </summary>
	protected virtual void OnFailure()
	{
		//TO-DO: replace by effect on gameplay.
		Debug.Log("Fail");
		FindObjectOfType<Notifier>().NotificateFail();
	}
}
