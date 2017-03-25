using UnityEngine;

public class Tracker : MonoBehaviour
{
	public Transform pursued;
	/// <summary>
	/// if tracker will follow on X, Y or both. X by default.
	/// </summary>
	public bool x = true, y = false;

	void Update ()
	{
		transform.position = new Vector3(x ? pursued.position.x : transform.position.x,
										 y ? pursued.position.y : transform.position.y,
										 transform.position.z);
	}
}
