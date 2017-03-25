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
		//TEST PURPOSES
		if (Input.GetButtonUp(KeyCode.B.ToString()))
			die.rollForward();
		if (Input.GetButtonUp(KeyCode.V.ToString()))
			die.rollBackward();


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
			Debug.Log("Fail");

		flipOrder = 3; //Reset to no flip order.
	}
}
