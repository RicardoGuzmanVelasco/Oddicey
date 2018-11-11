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
    /// <summary>
    /// <para><see cref="Notification.Beep"/>: turn around endlessly.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Beep;
	}

	public override void OnBeep()
	{
		TurnAround();
	}
	#endregion
}
