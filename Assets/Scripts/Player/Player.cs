using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	RollingCube cube;

	void Awake()
	{
		cube = GetComponent<RollingCube>();
	}

	public void Teleport(Vector3 position)
	{
		StartCoroutine(DelayTeleport(position));
	}

	/// <summary>
	/// Wait until rotation is completed before teleport.
	/// </summary>
	IEnumerator DelayTeleport(Vector3 position)
	{
		yield return new WaitWhile(() => cube.rolling);
		transform.position = position;
		cube.floor = position.y;
	}
}
