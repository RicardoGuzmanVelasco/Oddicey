using UnityEngine;

public class Mark : Notificable
{
	protected int sideRequired;
	private bool state;

	public Die player;

	public Sprite[] sprites;
	protected SpriteRenderer spriteRenderer;

	public bool State
	{
		get
		{
			return state;
		}

		set
		{
			state = value;
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
		base.Start(); //Notificable will selfsuscribe.
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			CheckSuccess();
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

	public void CheckSuccess()
	{
		if (State)
			OnSuccess();
		else
			OnFailure();
	}

	protected virtual void OnSuccess()
	{
		Debug.Log("Success");
	}

	protected virtual void OnFailure()
	{
		Debug.Log("Fail");
		FindObjectOfType<Notifier>().NotificateFail();
	}
}
