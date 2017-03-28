using System.Collections.Generic;
using UnityEngine;

public class Notifier : MonoBehaviour
{
	List<Notificable> listeners;

	void Awake()
	{
		listeners = new List<Notificable>();
	}

	public void Subscribe(Notificable listener)
	{
		listeners.Add(listener);
	}

	public void Unsubscribe(Notificable listener)
	{
		if(!listeners.Remove(listener))
			Debug.LogWarning("This listener was not subscribed");
	}

	public void NotificateBeep()
	{
		foreach (var listener in listeners)
			listener.OnBeep();
	}

	public void NotificateFlip()
	{
		foreach (var listener in listeners)
			listener.OnFlip();
	}
}
