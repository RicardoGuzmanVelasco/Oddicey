using UnityEngine;
using Utils.Extensions;

public class Vane : Notificable
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			TurnPlayer(collision.GetComponent<RollingCube>());
	}

	protected virtual void TurnPlayer(RollingCube rollingCube)
	{
		rollingCube.Turn();
	}

	public override void OnTurn()
	{
		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}
}
