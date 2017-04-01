using System.Collections.Generic;
using UnityEngine;

public class TeleporterCabin : MonoBehaviour
{
	public List<TeleporterOutput> outputs;
	public Transform sink;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			foreach (TeleporterOutput output in outputs)
				if (output.Check())
				{
					collision.GetComponent<Player>().Teleport(output.transform.position);
					return;
				}
		if (sink != null)
			collision.GetComponent<Player>().Teleport(sink.position);
	}
}
