using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Checkpoint which persist after save player position.
/// </summary>
public class UnbreakablePost : Post
{

	protected override void Start()
	{
		base.Start();
		Listening = false; // UnbreakablePost needn't listen until collision. 
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag != "Player")
			return;

		notifier.NotificateSave();
		GetComponent<Collider2D>().enabled = false;

		Listening = true;

		//TODO: add animation play. That animation will make the checked effect but no break as the standard Post.
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Beep;
	}

	/// <summary>
	/// It just listens one beep, exactly after the player collides with it.
	/// This avoids multiple saves within the same beep.
	/// </summary>
	public override void OnBeep()
	{
		GetComponent<Collider2D>().enabled = true;
		Listening = false;
	}
	#endregion
}
