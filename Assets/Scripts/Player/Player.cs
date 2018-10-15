using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	RollingCube cube;
	Rigidbody2D body;

	void Awake()
	{
		cube = GetComponent<RollingCube>();
		body = GetComponent<Rigidbody2D>();
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

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "NotFloor")
			OnGroundLost();
		else if (collision.tag == "Floor" && cube.Falling)
			OnGroundGained();
		else if (collision.tag == "Boundary" && collision.name == "BOTTOM")
			FindObjectOfType<Notifier>().NotificateFail(); //TODO: change: If EOL, no fail!
	}

	void OnGroundLost()
	{
		cube.Falling = true;
		FindObjectOfType<Notifier>().NotificateFall();
	}

	public void OnGroundGained()
	{
		cube.Falling = false;
		body.velocity = Vector2.zero;
		cube.floor = transform.position.y;
		cube.Snap();

		FindObjectOfType<Notifier>().NotificateLand();
	}
}
