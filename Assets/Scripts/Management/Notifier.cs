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
		if (!listeners.Remove(listener))
			Debug.LogWarning("This listener was not subscribed");
	}

	public void NotificateBeep()
	{
		//ToArray() returns a copy, which prevents problems unsubscribing during foreach.
		foreach (var listener in listeners.ToArray())
			listener.OnBeep();
	}

	public void NotificateFlip()
	{
		foreach (var listener in listeners)
			listener.OnFlip();
	}

	public void NotificateFail()
	{
		foreach (var listener in listeners)
			listener.OnFail();
	}

	public void NotificateDead()
	{
		foreach (var listener in listeners)
			listener.OnDead();
	}
}
