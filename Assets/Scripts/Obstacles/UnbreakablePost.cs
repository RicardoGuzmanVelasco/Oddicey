using System.Collections;
using UnityEngine;

/// <summary>
/// Checkpoint which persist after save player position.
/// </summary>
public class UnbreakablePost : Post
{
	#region Notifications
	protected override void ConfigureSubscriptions()
	{
        base.ConfigureSubscriptions();
		subscriptions |= Notification.Walk | Notification.Dead | Notification.Turn;
	}

	/// <summary>
	/// It just listens first beep.
	/// This avoids multiple saves within the same beep.
	/// </summary>
	public override void OnWalk()
    {
        StartCoroutine(ReactivateCollider());
    }

    /// <summary>
    /// When death, post stops listening collisions, in case die overlap after teleporting to checkpoint.
    /// This avoids multiple saves within the same checkpoint due to death and not to rewalking.
    /// </summary>
    public override void OnDead()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// If the player turns before it has dead, collider must be reactivated.
    /// Maybe Teleport event will require same reactivation.
    /// </summary>
    public override void OnTurn()
    {
        StartCoroutine(ReactivateCollider());
    }
    #endregion
}
