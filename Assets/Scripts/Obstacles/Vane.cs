using UnityEngine;
using Utils.Extensions;
using Utils.Directions;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Change his own direction when player does, always being against the player.
/// </remarks>
public class Vane : Notificable
{
	/// <summary>
	/// Course the die will take when collides.
	/// </summary>
	public Direction dir;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			TurnPlayer(collision.GetComponent<RollingCube>());
	}

	/// <summary>
	/// Send the player a Turn instruction.
	/// </summary>
	/// <param name="rollingCube">Player's RollingCube assigned.</param>
	protected virtual void TurnPlayer(RollingCube rollingCube)
	{
		rollingCube.Turn(dir.ToVector2());
	}

	public override void OnTurn()
	{
		TurnAround();
	}

	/// <summary>
	/// Reverse 'dir' attribute and make visual change.
	/// </summary>
	protected void TurnAround()
	{
		dir = dir.Reverse();
		//TO-DO: replace by animation play. That animation will make the inversion.
		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}
}
