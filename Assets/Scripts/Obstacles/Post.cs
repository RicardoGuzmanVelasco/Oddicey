using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Checkpoint.
/// </summary>
public class Post : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;

		//May cause a framedropping. TODO?: replace by notification.
		FindObjectOfType<Notifier>().NotificateSave();
		GetComponent<Collider2D>().enabled = false;
	
		//TODO: replace by animation play. That animation will make the checked effect.
		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}
}
