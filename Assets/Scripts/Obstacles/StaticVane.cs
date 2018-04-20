using UnityEngine;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Never change this direction.
/// </remarks>
public class StaticVane : Vane
{
	protected override void ConfigureSubscriptions()
	{
		subscriptions = News.None;
	}
	//Unnecesary, as FlippingVane's comment explain.
	//public override void OnTurn()
	//{
	//}
}
