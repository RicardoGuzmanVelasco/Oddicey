using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ParallaxBackground : MonoBehaviour
{
	public enum DirectionEnum { Left, Right, Up, Down };
	[Tooltip("Trajectory that the object will follow")]
	public DirectionEnum direction = DirectionEnum.Left;
	private Vector2 dir = Vector2.left;
	[Tooltip("Positive scalar that multiplies to the direction")]
	public float speed = 1;
	[Tooltip("If false, similar to speed=0")]
	public bool moving = true;

	private Vector2 size;
	private Vector2 originalPosition, edge;

	[Tooltip("Copies of itself that the object will do to cover all the area")]
	public int clones = 1;
	private GameObject[] clonesList;

	#region Properties
	public float Speed
	{
		get
		{
			return speed;
		}

		set
		{
			speed = value;
		}
	}

	public bool Moving
	{
		get
		{
			return moving;
		}

		set
		{
			moving = value;
		}
	}

	public DirectionEnum Direction
	{
		get
		{
			return direction;
		}

		set
		{
			direction = value;
			dir = EnumToVector(direction);
		}
	}

	public Vector2 Edge
	{
		get
		{
			return edge;
		}
	}
	#endregion

	private void Awake()
	{
		if (GetComponent<SpriteRenderer>().sprite == null)
			Debug.LogError("Sprite required on SpriteRendererComponent (" + name + ").");
		var scale = Vector2.Scale(transform.localScale, transform.lossyScale);
		size = Vector2.Scale(GetComponent<SpriteRenderer>().sprite.bounds.size, scale);

		if (clones > 0)
			clonesList = new GameObject[clones];
	}

	private void Start()
	{
		originalPosition = transform.position;
		edge = originalPosition + Vector2.Scale(dir, size);

		dir = EnumToVector(direction);

		for (int i = 0; i < clones; i++)
			Clone(i);
		if (clones > 0)
			name += 1;
	}

	private Vector2 EnumToVector(DirectionEnum direction)
	{
		Vector2 vector = Vector2.zero;
		switch (direction)
		{
			case DirectionEnum.Left:
				vector = Vector2.left;
				break;
			case DirectionEnum.Right:
				vector = Vector2.right;
				break;
			case DirectionEnum.Up:
				vector = Vector2.up;
				break;
			case DirectionEnum.Down:
				vector = Vector2.down;
				break;
			default:
				Debug.LogError("Unknown direction option.");
				break;
		}
		return vector;
	}

	private void Clone(int i)
	{
		clonesList[i] = Instantiate(gameObject);
		Destroy(clonesList[i].GetComponent<ParallaxBackground>());
		clonesList[i].name = name + (i + 2);
		clonesList[i].transform.position = originalPosition + (i + 1) * Vector2.Scale(-dir, size);
		clonesList[i].transform.parent = transform.parent;
	}

	private void Update()
	{
		//Moving itself.
		if (Vector2.Dot(edge, dir) < Vector2.Dot(transform.position, dir))
			transform.position = originalPosition - Vector2.Scale(size, dir * clones);
		if (moving)
			transform.Translate(dir * speed * Time.smoothDeltaTime);
		//Moving clones.
		for (int i = 0; i < clonesList.Length; i++)
		{
			if (Vector2.Dot(edge, dir) < Vector2.Dot(clonesList[i].transform.position, dir))
				clonesList[i].transform.position = originalPosition - Vector2.Scale(size, dir * clones);
			if (moving)
				clonesList[i].transform.Translate(dir * speed * Time.smoothDeltaTime);
		}
	}

	[ExecuteInEditMode]
	void OnValidate()
	{
		speed = Mathf.Clamp(speed, 0, speed);
		Speed = speed;
		Direction = direction;
		Moving = moving;
	}
}
