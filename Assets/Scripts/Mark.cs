using UnityEngine;

public class Mark : Notificable
{
	protected int sideRequired;
	private bool state;

	public Die player;

	public Sprite[] sprites;
#pragma warning disable CS0108
	protected SpriteRenderer renderer;
#pragma warning restore CS0108

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

	protected override void Start()
	{
		sideRequired = Random.Range(1, 6);
		renderer.sprite = sprites[sideRequired - 1];
		base.Start(); //Notificable will selfsuscribe.
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			if (State)
				OnSuccess();
			else
				OnFailure();
	}

	public override void OnBeep()
	{
		CheckRight();
	}

	public override void OnFlip()
	{
		CheckRight();
	}

	protected virtual void CheckRight()
	{
		if (player.Side == sideRequired)
			OnRight();
		else
			OnWrong();
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
