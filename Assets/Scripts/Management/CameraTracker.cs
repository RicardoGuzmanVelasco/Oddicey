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
	bool y = false;
	/// <summary>
	/// When stopped, if retrack it will take last values.
	/// </summary>
	bool lastX = true, lastY = false;

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
    /// <summary>
    /// <para><see cref="Notification.Turn"/>: recover last options if isn't tracking in any axis.</para>
    /// <para><see cref="Notification.Dead"/>: reset default tracking options.</para>
    /// <para><see cref="Notification.FallingGroup"/>: vertical self-tracking while falling.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Turn | Notification.Dead | Notification.FallingGroup;
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
        x = lastX = true;
        y = lastY = false;
    }

    public override void OnFall()
    {
        Y = true;
    }

    public override void OnLand()
    {
        Y = lastY;
    }
    #endregion
}
