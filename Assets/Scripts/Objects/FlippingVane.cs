using UnityEngine;

public class FlippingVane : Vane
{
	/// <summary>
	/// Course the die will take when collides.
	/// </summary>
	public Vector2 dir = Vector2.left;

	public override void OnBeep()
	{
		dir *= -1;
	}

	protected override void TurnPlayer(RollingCube rollingCube)
	{
		rollingCube.Turn(dir);
	}

	public override void OnTurn() { }
}
