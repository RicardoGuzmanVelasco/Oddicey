using UnityEngine;

public class InputManager : MonoBehaviour
{
	public GameObject player;
	Die die;

	int flipOrder = 3; //flip >3 or <(-3) will not be ordered.

	void Awake()
	{
		die = player.GetComponent<Die>();
	}

	void Update()
	{
		if (Input.GetButtonUp("Test1"))
			GetComponent<MotorSystem>().Moving = !GetComponent<MotorSystem>().Moving;

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
			Debug.Log("Out of time!");//TO-DO. When player try to flip out of time;

		flipOrder = 3; //Reset to no flip order.
	}
}
