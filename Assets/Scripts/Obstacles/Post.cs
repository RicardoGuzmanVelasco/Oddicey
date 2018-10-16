using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Checkpoint. It breaks after save player position.
/// </summary>
public class Post : Notificable
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;

		notifier.NotificateSave();
		GetComponent<Collider2D>().enabled = false;
	
		//TODO: replace by animation play. That animation will make the checked effect.
		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.None;
	}
	#endregion
}
