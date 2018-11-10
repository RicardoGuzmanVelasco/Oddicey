using UnityEngine;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Never change this direction.
/// </remarks>
public class FixedVane : Vane
{
    /// <remarks>
    /// <para><see cref="Notification.None"/>: as long as it doesn't interacts.</para>
    /// </remarks>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.None;
	}
}
