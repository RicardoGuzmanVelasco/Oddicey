using UnityEngine;

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
	}
}
