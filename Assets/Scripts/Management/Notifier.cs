using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Notifier : MonoBehaviour
{
	//List<Notificable> listeners;
	HashSet<Notificable> listeners;

	void Awake()
	{
		//listeners = new List<Notificable>();
		listeners = new HashSet<Notificable>();
	}

	public void Subscribe(Notificable listener)
	{
		//if (listeners.Contains(listener))
		//	Debug.LogWarning(listener.name + " already subscribed.");
		//else
		//	listeners.Add(listener);
		if(!listeners.Add(listener))
			Debug.LogWarning(listener.name + " already subscribed.");
	}

	public void Unsubscribe(Notificable listener)
	{
		if (!listeners.Remove(listener))
			Debug.LogWarning(name + " was not subscribed");
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

	public void NotificateTurn()
	{
		foreach (var listener in listeners)
		{
			listener.OnTurn();
		}
	}
}
