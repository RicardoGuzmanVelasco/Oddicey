using System;
using UnityEngine;

public class TeleporterOutput : MonoBehaviour
{
	/// <summary>
	/// Not collisionable mark! Just floating over the output.
	/// </summary>
	Mark mark;

	void Awake()
	{
		mark = GetComponentInChildren<Mark>();
	}

	void Start()
	{
		if (mark.GetComponent<Collider2D>() != null)
			Destroy(mark.GetComponent<Collider2D>());
	}

	public bool Check()
	{
		return mark.State;
	}
}
