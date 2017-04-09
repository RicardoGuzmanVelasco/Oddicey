using UnityEngine;
using Utils.Extensions;

public class Post : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;

		FindObjectOfType<PlayManager>().checkpoint = transform.position;
		GetComponent<Collider2D>().enabled = false;

		transform.localScale = transform.localScale.X(transform.localScale.x * -1);
	}
}
