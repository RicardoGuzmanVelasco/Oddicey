using UnityEngine;

public class Post : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;

		Debug.Log("Checkpoint");

		FindObjectOfType<PlayManager>().checkpoint = transform.position;
		GetComponent<Collider2D>().enabled = false;
	}
}
