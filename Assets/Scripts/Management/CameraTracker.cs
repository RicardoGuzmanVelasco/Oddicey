using UnityEngine;

public class CameraTracker : Notificable
{
	public Transform pursued;
	/// <summary>
	/// Initial offset between camera center and pursued center.
	/// </summary>
	Vector2 distance;

	[SerializeField]
	bool x = true;
	[SerializeField]
	bool y = true;
	/// <summary>
	/// When stopped, if retrack it will take last values.
	/// </summary>
	bool lastX = true, lastY = true;

	#region Properties
	public bool X
	{
		get
		{
			return x;
		}

		set
		{
			x = value;
			RecalculateDistance();
		}
	}

	public bool Y
	{
		get
		{
			return y;
		}

		set
		{
			y = value;
			RecalculateDistance();
		}
	} 
	#endregion

	override protected void Start()
	{
		base.Start(); // Subscribing to the notifier.
		RecalculateDistance();
	}

	void RecalculateDistance()
	{
		distance = pursued.position - transform.position;
	}

	void Update()
	{
		transform.position = new Vector3(X ? pursued.position.x - distance.x : transform.position.x,
										 Y ? pursued.position.y - distance.y : transform.position.y,
										 transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Boundary" && collision.name == "EOL")
		{
			lastX = X;
			X = false;
		}
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Turn | Notification.Dead;
	}

	public override void OnTurn()
	{
		if (!X && !Y)
		{
			X = lastX;
			Y = lastY;
		}
	}

    public override void OnDead()
    {
        x = y = lastX = lastY = true;
    }
    #endregion
}
