	using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

[Flags]
public enum News
{
	/// <summary>
	/// All news, identity element.
	/// </summary>
	Any = 255,
	/// <summary>
	/// Zero, absorbing element.
	/// </summary>
	None = 0,
	/// <summary>
	/// Musical beat.
	/// </summary>
	Beep = 1 << 0,
	/// <summary>
	/// Active side on die changed.
	/// </summary>
	Flip = 1 << 1,
	/// <summary>
	/// Failure when reaching a mark.
	/// </summary>
	Fail = 1 << 2,
	/// <summary>
	/// Any death condition.
	/// </summary>
	Dead = 1 << 3,
	/// <summary>
	/// Direction inverted.
	/// </summary>
	Turn = 1 << 4,
	/// <summary>
	/// Checkpoint reached.
	/// </summary>
	Save = 1 << 5,
	/// <summary>
	/// Ground lost.
	/// </summary>
	Fall = 1 << 6,
	/// <summary>
	/// Ground gained.
	/// </summary>
	Land = 1 << 7
}

public class Notifier : MonoBehaviour
{
	Dictionary<News, HashSet<Notificable>> listeners;

	void Awake()
	{
		listeners = new Dictionary<News, HashSet<Notificable>>();

		foreach(News notification in Enum.GetValues(typeof(News)))
			if(notification != News.Any)
				listeners.Add(notification, new HashSet<Notificable>());
	}

	public void Subscribe(Notificable listener, News subscriptions = News.Any)
	{
		//foreach(News notification in Enum.GetValues(typeof(News)))
		//	if(notification == News.None)
		//		continue;
		foreach(News notification in listeners.Keys)
			if(subscriptions.HasFlag(notification))
				listeners[notification].Add(listener);
		//if(!listeners[notification].Add(listener))
		//			Debug.LogError(listener.name + " / " + listener.GetType() + " has already subscribed to " + notification);
		//		else
		//			Debug.Log(listener.name + "/" + listener.GetType() + " successfully subscribed to " + notification);
	}

	public void Unsubscribe(Notificable listener, News subscriptions = News.Any)
	{
		//foreach(News notification in Enum.GetValues(typeof(News)))
		//	if(notification == News.None)
		//		continue;
		foreach(News notification in listeners.Keys)
			if(subscriptions.HasFlag(notification))
				listeners[notification].Remove(listener);
		//if(!subscriptionLists[notification].Remove(listener))
		//	Debug.Log("<color=red>  " + listener.name + " was not subscribed to " + notification + ".</color>");
		//else
		//	Debug.Log("<color=green>" + listener.name + " was unsubscribed to " + notification + ".</color>");
	}

	public void NotificateBeep()
	{
		foreach(var listener in listeners[News.Beep].ToArray()) //ToArray() returns a copy, which prevents problems unsubscribing during foreach.
			listener.OnBeep();
	}

	public void NotificateFlip()
	{
		foreach(var listener in listeners[News.Flip])
			listener.OnFlip();
	}

	public void NotificateFail()
	{
		foreach(var listener in listeners[News.Fail])
			listener.OnFail();
	}

	public void NotificateDead()
	{
		foreach(var listener in listeners[News.Dead])
			listener.OnDead();
	}

	public void NotificateTurn()
	{
		foreach(var listener in listeners[News.Turn])
			listener.OnTurn();
	}

	public void NotificateSave()
	{
		foreach(var listener in listeners[News.Save])
			listener.OnSave();
	}

	public void NotificateFall()
	{
		foreach(var listener in listeners[News.Fall])
			listener.OnFall();
	}

	public void NotificateLand()
	{
		foreach(var listener in listeners[News.Land])
			listener.OnLand();
	}

	public void Notificate(News notification)
	{
		if(notification.HasFlag(News.Beep))
			NotificateBeep();
		if(notification.HasFlag(News.Flip))
			NotificateFlip();
		if(notification.HasFlag(News.Fail))
			NotificateFail();
		if(notification.HasFlag(News.Dead))
			NotificateDead();
		if(notification.HasFlag(News.Turn))
			NotificateTurn();
		if(notification.HasFlag(News.Save))
			NotificateSave();
		if(notification.HasFlag(News.Fall))
			NotificateFall();
		if(notification.HasFlag(News.Land))
			NotificateLand();
	}
}
