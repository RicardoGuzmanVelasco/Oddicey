using UnityEngine;

public class CameraTracker : Notificable
{
	public Transform pursued;
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

	void Update()
	{
		transform.position = new Vector3(X ? pursued.position.x - distance.x : transform.position.x,
										 Y ? pursued.position.y - distance.y : transform.position.y,
										 transform.position.z);
		//RecalculateDistance(); //TO-DO: Unnecessary if it is called on 'x', 'y' changes.
	}

	private void RecalculateDistance()
	{
		distance = pursued.position - transform.position;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Boundary" && collision.name == "EOL")
		{
			lastX = X;
			X = false;
		}
	}

	public override void OnTurn()
	{
		if (!X && !Y)
		{
			X = lastX;
			Y = lastY;
		}
	}

}
