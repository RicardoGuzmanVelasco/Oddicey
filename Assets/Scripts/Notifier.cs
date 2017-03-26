using System.Collections.Generic;
using UnityEngine;

public class Notifier : MonoBehaviour
{
	List<Mark> listeners;

	void Awake()
	{
		listeners = new List<Mark>();
	}

	public void Subscribe(Mark listener)
	{
		listeners.Add(listener);
	}

	public void Unsubscribe(Mark listener)
	{
		if(!listeners.Remove(listener))
			Debug.LogWarning("This listener was not subscribed");
	}

	public void NotificateBeep()
	{
		foreach (var listener in listeners)
			listener.OnBeep();
	}
}
