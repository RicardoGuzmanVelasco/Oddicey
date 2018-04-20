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

		//May cause a framedropping. TO-DO?: replace by notification.
		FindObjectOfType<Notifier>().NotificateSave();
		GetComponent<Collider2D>().enabled = false;
	
		//TO-DO: replace by animation play. That animation will make the checked effect.
		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}
}
