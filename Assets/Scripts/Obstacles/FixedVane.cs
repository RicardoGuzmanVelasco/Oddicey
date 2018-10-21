using UnityEngine;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Never change this direction.
/// </remarks>
public class FixedVane : Vane
{
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.None;
	}
}
