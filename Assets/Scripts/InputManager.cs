using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Notificable
{
	public GameObject player;
	Die die;

	int flipOrder = 3; //flip >3 or <(-3) will not be ordered.
	bool flipEnabled = false;

	void Awake()
	{
		die = player.GetComponent<Die>();
	}

	void Update()
	{
		if (Input.GetButtonUp("Test1"))
			GetComponent<MotorSystem>().Moving = !GetComponent<MotorSystem>().Moving;
		if (Input.GetButtonUp("Test2"))
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if (Input.GetButtonUp("Test3"))
			GetComponent<MotorSystem>().Tempo += GetComponent<MotorSystem>().Tempo / 2;
		if (Input.GetButtonUp("Test4"))
			GetComponent<MotorSystem>().Tempo -= GetComponent<MotorSystem>().Tempo / 2;


		if (!flipEnabled)
			return; //TO-DO. When player try to flip twice in a roll;

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
			return; //TO-DO. When player try to flip out of time;

		GetComponent<Notifier>().NotificateFlip();
		flipEnabled = false;

		flipOrder = 3; //Reset to no flip order.
	}

	public override void OnBeep()
	{
		flipEnabled = true;
	}
}
