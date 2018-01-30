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

	public override void OnFail()
	{
		FindObjectOfType<Notifier>().NotificateDead();
		player.Teleport(checkpoint);
		//TO-DO: may be a method. 'dir' will be private or Property.
		player.GetComponent<RollingCube>().dir = Vector2.right.ToDirection();
		GetComponent<MotorSystem>().Moving = false;
	}
}
