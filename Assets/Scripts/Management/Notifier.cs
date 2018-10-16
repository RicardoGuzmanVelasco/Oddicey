using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Extensions;

[Flags]
public enum Notification
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
	Land = 1 << 7,
	/// <summary>
	/// Try to flip out of time.
	/// </summary>
	Late = 1 << 8,
	/// <summary>
	/// Die marches on after stopped.
	/// </summary>
	Walk = 1 << 9
}

public sealed class Notifier : MonoBehaviour
{
	Dictionary<Notification, HashSet<Notificable>> listeners;

	void Awake()
	{
		listeners = new Dictionary<Notification, HashSet<Notificable>>();

		foreach(Notification notification in Enum.GetValues(typeof(Notification)))
			if(notification != Notification.Any)
				listeners.Add(notification, new HashSet<Notificable>());
	}

	/// <summary>
	/// Includes <paramref name="listener"/> to all <see cref="Notification"/> types within <paramref name="subscriptions"/>.
	/// </summary>
	/// <param name="subscriptions">Subscribed to all notifications by default.</param>
	public void Subscribe(Notificable listener, Notification subscriptions = Notification.Any)
	{
		foreach(Notification notification in listeners.Keys)
			if(subscriptions.HasFlag(notification))
				listeners[notification].Add(listener);
	}

	/// <summary>
	/// Take out <paramref name="listener"/> to all <see cref="Notification"/> types within <paramref name="subscriptions"/>.
	/// </summary>
	/// <param name="subscriptions">Unsubscribed to all notifications by default.</param>
	public void Unsubscribe(Notificable listener, Notification subscriptions = Notification.Any)
	{
		foreach(Notification notification in listeners.Keys)
			if(subscriptions.HasFlag(notification))
				listeners[notification].Remove(listener);
	}

	public void NotificateBeep()
	{
		foreach(var listener in listeners[Notification.Beep].ToArray()) //ToArray() returns a copy, which prevents problems unsubscribing during foreach.
			listener.OnBeep();
	}

	public void NotificateFlip()
	{
		foreach(var listener in listeners[Notification.Flip])
			listener.OnFlip();
	}

	public void NotificateFail()
	{
		foreach(var listener in listeners[Notification.Fail])
			listener.OnFail();
	}

	public void NotificateDead()
	{
		foreach(var listener in listeners[Notification.Dead])
			listener.OnDead();
	}

	public void NotificateTurn()
	{
		foreach(var listener in listeners[Notification.Turn])
			listener.OnTurn();
	}

	public void NotificateSave()
	{
		foreach(var listener in listeners[Notification.Save])
			listener.OnSave();
	}

	public void NotificateFall()
	{
		foreach(var listener in listeners[Notification.Fall])
			listener.OnFall();
	}

	public void NotificateLand()
	{
		foreach(var listener in listeners[Notification.Land])
			listener.OnLand();
	}

	public void Notificate(Notification notification)
	{
		if(notification.HasFlag(Notification.Beep))
			NotificateBeep();
		if(notification.HasFlag(Notification.Flip))
			NotificateFlip();
		if(notification.HasFlag(Notification.Fail))
			NotificateFail();
		if(notification.HasFlag(Notification.Dead))
			NotificateDead();
		if(notification.HasFlag(Notification.Turn))
			NotificateTurn();
		if(notification.HasFlag(Notification.Save))
			NotificateSave();
		if(notification.HasFlag(Notification.Fall))
			NotificateFall();
		if(notification.HasFlag(Notification.Land))
			NotificateLand();
	}
}
