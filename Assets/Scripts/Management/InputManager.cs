using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Notificable
{
	public GameObject player;
	Die die;

	/// <summary>
	/// Amount of sides to flip.
	/// '3' internally represents the no order (flip=0).
	/// '0' internally represents the '3' order (flip to opposite side).
	/// </summary>
	/// <remarks>
	/// Any <=-3 or =>3 order will be ignored.
	/// </remarks>
	enum FlipOrder
	{
		NoFlip = 3,
		Back1 = -1,
		Back2 = -2,
		Flip = 0,
		Plus1 = 1,
		Plus2 = 2,
	}
	FlipOrder flipOrder = FlipOrder.NoFlip;
	/// <summary>
	/// If die can flip its side on current beep.
	/// </summary>
	bool flipEnabled = false;

	#region Properties
	public bool FlipEnabled
	{
		get
		{
			return flipEnabled;
		}

		set
		{
			flipEnabled = value;
		}
	}
	#endregion

	void Awake()
	{
		die = player.GetComponent<Player>().GetComponent<Die>();
	}

	void Update()
	{
		#region Test Controls
		if(Input.GetButtonUp("Test1"))
		{
			GetComponent<MotorSystem>().Moving = !GetComponent<MotorSystem>().Moving;
			if(GetComponent<MotorSystem>().Moving)
				flipEnabled = true;
		}
		if(Input.GetButtonUp("Test2"))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if(Input.GetButtonUp("Test3"))
			GetComponent<MotorSystem>().Tempo += 10;
		if(Input.GetButtonUp("Test4"))
			GetComponent<MotorSystem>().Tempo -= 10;
		if(Input.GetButtonUp("Test5"))
			die.GetComponent<RollingCube>().Turn();
		#endregion

		if(!FlipEnabled)
			return; //TODO: When player try to flip twice in a roll;

		if(Input.GetButtonUp("Flip"))
			flipOrder = FlipOrder.Flip;
		else if(Input.GetButtonUp("Flip+1"))
			flipOrder = FlipOrder.Plus1;
		else if(Input.GetButtonUp("Flip-1"))
			flipOrder = FlipOrder.Back1;
		else if(Input.GetButtonUp("Flip+2"))
			flipOrder = FlipOrder.Plus2;
		else if(Input.GetButtonUp("Flip-2"))
			flipOrder = FlipOrder.Back2;

		if(flipOrder == FlipOrder.NoFlip)
			return;

		if(!die.Flip((int)flipOrder))
			Debug.Log("Out of time"); //TODO: when player try to flip out of time;
		else
		{
			GetComponent<Notifier>().NotificateFlip();
			FlipEnabled = false;
		}

		flipOrder = FlipOrder.NoFlip;
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Beep | Notification.Fall | Notification.Land;
	}

	//This overriding did make the die not properly falling.
	public override void OnBeep()
	{
		FlipEnabled = !player.GetComponent<RollingCube>().Falling;
	}

	public override void OnFall()
	{
		FlipEnabled = false;
	}

	public override void OnLand()
	{
		FlipEnabled = true;
	}
	#endregion
}
