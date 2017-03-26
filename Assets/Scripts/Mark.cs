using System;
using UnityEngine;

public class Mark : MonoBehaviour
{
	protected int sideRequired;
	private bool state;

	public Die player;

	public Sprite[] sprites;
	protected SpriteRenderer renderer;

	protected bool State
	{
		get
		{
			return state;
		}

		set
		{
			state = value;
			renderer.color = state ? Color.green : Color.red;
		}
	}

	void Awake()
	{
		renderer = GetComponent<SpriteRenderer>();
		player = FindObjectOfType<Die>();
	}

	void Start()
	{
		sideRequired = UnityEngine.Random.Range(1, 6);
		renderer.sprite = sprites[sideRequired - 1];
		FindObjectOfType<Notifier>().Subscribe(this);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			if (State)
				OnSuccess();
			else
				OnFailure();
	}

	public virtual void OnBeep()
	{
		if (IsRight(player.Side))
			OnRight();
		else
			OnWrong();
	}

	protected virtual bool IsRight(int side)
	{
		return side == sideRequired;
	}

	protected virtual void OnRight()
	{
		State = true;
	}

	protected virtual void OnWrong()
	{
		State = false;
	}

	protected virtual void OnSuccess()
	{
		Debug.Log("Success");
		FindObjectOfType<Notifier>().Unsubscribe(this);
	}

	protected virtual void OnFailure()
	{
		Debug.Log("Fail");
		FindObjectOfType<Notifier>().Unsubscribe(this);
	}
}
