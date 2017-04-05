using UnityEngine;

public class Tracker : MonoBehaviour
{
	public Transform pursued;
	Vector2 distance;
	/// <summary>
	/// if tracker will follow on X, Y or both. X by default.
	/// </summary>
	public bool x = true, y = false;

	void Start()
	{
		RecalculateDistance();
	}

	void Update()
	{
		transform.position = new Vector3(x ? pursued.position.x - distance.x : transform.position.x,
										 y ? pursued.position.y - distance.y : transform.position.y,
										 transform.position.z);
		RecalculateDistance(); //Unnecessary if it is called on 'x', 'y' changes.
	}

	private void RecalculateDistance()
	{
		distance = pursued.position - transform.position;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "EditorOnly")
		{
			x = false;
			y = false;
		}
	}
}
