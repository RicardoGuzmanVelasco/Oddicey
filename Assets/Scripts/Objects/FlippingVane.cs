using UnityEngine;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Change his own direction every beep, and NOT when player does.
/// </remarks>
public class FlippingVane : Vane
{

	public override void OnBeep()
	{
		TurnAround();
	}

	/// <summary>
	/// Overriding in order to avoid turn around when player does. 
	/// </summary>
	public override void OnTurn() { }
}
