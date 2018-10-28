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
	Beep    = 1 << 00,
	/// <summary>
	/// Active side on die changed.
	/// </summary>
	Flip    = 1 << 01,
	/// <summary>
	/// Failure when reaching a mark.
	/// </summary>
	Fail    = 1 << 02,
	/// <summary>
	/// Any death condition.
	/// </summary>
	Dead    = 1 << 03,
	/// <summary>
	/// Direction inverted.
	/// </summary>
	Turn    = 1 << 04,
	/// <summary>
	/// Checkpoint reached.
	/// </summary>
	Save    = 1 << 05,
	/// <summary>
	/// Ground lost.
	/// </summary>
	Unsave  = 1 << 06,
	/// <summary>
	/// Ground gained.
	/// </summary>
	Land    = 1 << 07,
	/// <summary>
	/// Try to flip out of time.
	/// </summary>
	Fall    = 1 << 08,
	/// <summary>
	/// Die marches on. Consider this a kind of "JustFirstBeepNotification".
	/// </summary>
	Walk    = 1 << 09,
    /// <summary>
	/// Ground lost.
	/// </summary>
	Late    = 1 << 10
}

public sealed class Notifier : MonoBehaviour
{
    /// <summary>
    /// A set of <see cref="Notificable"/> instances for each <see cref="Notification"/>.
    /// </summary>
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

    public void NotificateUnsave()
    {
        foreach(var listener in listeners[Notification.Unsave])
            listener.OnUnsave();
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

    public void NotificateWalk()
    {
        foreach(var listener in listeners[Notification.Walk])
            listener.OnWalk();
    }

    /// <summary>
    /// Public interface to notificate some <see cref="Notification"/> at once.
    /// </summary>
    /// <param name="notification">
    /// Set of flags whose <see cref="Notification"/> will be called.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="notification"/> equals <see cref="Notification.None>"/>.
    /// That is considered an unintended call to this method, since no action will be taken.
    /// Throw this exception prevents an incorrect call or using '&' bitwise operator instead of '|'.
    /// </exception>
    public void Notificate(Notification notification)
    {
        if(notification == Notification.None)
            throw new ArgumentNullException("No notification is going to be called.");
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
        if(notification.HasFlag(Notification.Unsave))
            NotificateUnsave();
        if(notification.HasFlag(Notification.Fall))
            NotificateFall();
        if(notification.HasFlag(Notification.Land))
            NotificateLand();
        if(notification.HasFlag(Notification.Walk))
            NotificateWalk();
    }
}
