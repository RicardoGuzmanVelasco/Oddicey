using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
	public Transform output;

	void Awake()
	{
		GetComponent<Collider2D>().enabled = false;

		if (GetComponentInChildren<MarksPile>() != null)
			GetComponentInChildren<MarksPile>().Gate = this;
		else
			Open();
	}

	public void Open()
	{
		Debug.Log("Portal was opened.");
		GetComponent<Collider2D>().enabled = true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			collision.GetComponent<Player>().Teleport(output.position);
	}
}
