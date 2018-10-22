using UnityEngine;
using Utils.Extensions;

public class PlayManager : Notificable
{
	Player player;
	public Vector3 checkpoint;

	void Awake()
	{
		player = FindObjectOfType<Player>();
		checkpoint = player.transform.position;
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Fail | Notification.Save;
	}

	public override void OnFail()
	{
		notifier.NotificateDead();
		player.Teleport(checkpoint);
		//TODO: may be a method. 'dir' will be private or Property.
		player.GetComponent<RollingCube>().dir = Vector2.right.ToDirection();
		GetComponent<MotorSystem>().Moving = false;
	}

	public override void OnSave()
	{
		Debug.Log("Saving");
		checkpoint = player.transform.position;
	}
	#endregion
}
