using UnityEngine;

/// <summary>
/// Gate that teleport player to one determined outpit if is open.
/// </summary>
/// <remarks>
/// Collider2D will be active when portal is open.
/// </remarks>
[RequireComponent(typeof(Collider2D))]
public class Portal : MonoBehaviour
{
	/// <summary>
	/// Determined and necessary output to teleport the player.
	/// </summary>
	public Transform output;

	void Awake()
	{
		GetComponent<Collider2D>().enabled = false;

		if (GetComponentInChildren<MarksPile>() != null)
			GetComponentInChildren<MarksPile>().Gate = this;
		else
			Open();
	}

	//TODO: control if an output exists!
	public void Open()
	{
		//TODO: replace by an animation.
		Debug.Log("Portal was opened.");
		GetComponent<Collider2D>().enabled = true;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			collision.GetComponent<Player>().Teleport(output.position);
	}
}
