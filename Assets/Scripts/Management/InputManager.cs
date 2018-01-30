using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Notificable
{
	public GameObject player;
	Die die;

	/// <summary>
	/// If die can flip its side.
	/// </summary>
	private bool flipEnabled = false;
	/// <summary>
	/// Amount of sides to flip.
	/// '3' internally represents the no order (flip=0).
	/// '0' internally represents the '3' order (flip to opposite side).
	/// </summary>
	/// <remarks>
	/// Any <=-3 or =>3 order will be ignored.
	/// </remarks>
	int flipOrder = 3;

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
		if (Input.GetButtonUp("Test1"))
		{
			GetComponent<MotorSystem>().Moving = !GetComponent<MotorSystem>().Moving;
			if (GetComponent<MotorSystem>().Moving)
				flipEnabled = true;
		}
		if (Input.GetButtonUp("Test2"))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if (Input.GetButtonUp("Test3"))
			GetComponent<MotorSystem>().Tempo += 10;
		if (Input.GetButtonUp("Test4"))
			GetComponent<MotorSystem>().Tempo -= 10;
		if (Input.GetButtonUp("Test5"))
			die.GetComponent<RollingCube>().Turn();
		#endregion

		if (!FlipEnabled)
			return; //TO-DO: When player try to flip twice in a roll;

		if (Input.GetButtonUp("Flip"))
			flipOrder = 0;
		else if (Input.GetButtonUp("Flip+1"))
			flipOrder = 1;
		else if (Input.GetButtonUp("Flip-1"))
			flipOrder = -1;
		else if (Input.GetButtonUp("Flip+2"))
			flipOrder = 2;
		else if (Input.GetButtonUp("Flip-2"))
			flipOrder = -2;

		if (flipOrder > 2 || flipOrder < -2)
			return; //No flip is ordered.

		if (!die.flip(flipOrder))
			return; //TO-DO: when player try to flip out of time;

		GetComponent<Notifier>().NotificateFlip();
		FlipEnabled = false;

		flipOrder = 3; //Reset to no flip order.
	}

	//This overriding makes the die not properly falling.
	//public override void OnBeep()
	//{
	//	FlipEnabled = true;
	//}
}
