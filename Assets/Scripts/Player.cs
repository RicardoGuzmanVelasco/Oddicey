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
		transform.position = position;
		cube.floor = position.y;
	}

}
