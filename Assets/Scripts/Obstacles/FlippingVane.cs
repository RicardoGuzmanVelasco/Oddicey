using UnityEngine;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Change his own direction every beep, and NOT when player does.
/// </remarks>
public class FlippingVane : Vane
{
	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = News.Beep;
	}

	public override void OnBeep()
	{
		TurnAround();
	}
	//Unnecesary override because of new subscription scheme.
	//As FlippingVane will be only listening BEEP, OnTurn() will not be reached.
	///// <summary>
	///// Overriding in order to avoid turn around when player does. 
	///// </summary>
	//public override void OnTurn() { }
	#endregion
}
