﻿using System;
using UnityEngine;

/// <summary>
/// A Notificable can both receive a <see cref="Notification"/> event
/// and also request the <see cref="Notifier"/> to raise any of them.
/// </summary>
public abstract class Notificable : MonoBehaviour
{
	protected Notifier notifier;
	protected Notification subscriptions;
	/// <summary>
	/// If it's subscribed to <see cref="Notifier"/>, regardless <see cref="subscriptions"/> content.
	/// </summary>
	bool listening;

#if UNITY_EDITOR
    /// <summary>
    /// Just a remainder. If <see langword="false"/> when play mode, it means that
    /// <see cref="Start"/> has been override and base.Start() doesn't exist.
    /// </summary>
    bool subscriptionConfigured = false;
#endif

    #region Properties
    /// <summary>
    /// Set listening property subscribes or unsubscribes to <see cref="Notifier"/>.
    /// </summary>
    public bool Listening
	{
		get
		{
			return listening;
		}

		set
		{
			if(value && !listening)
				FindObjectOfType<Notifier>().Subscribe(this, subscriptions);
			else if(!value)
				FindObjectOfType<Notifier>().Unsubscribe(this);
			listening = value;
		}
	}
	#endregion

	protected virtual void Awake()
	{
		notifier = FindObjectOfType<Notifier>();
	}

	/// <summary>
	/// Notificable will selfsubscribe when “start”.
	/// </summary>
	protected virtual void Start()
	{
		ConfigureSubscriptions();
		Listening = true;
#if UNITY_EDITOR
        subscriptionConfigured = true;
#endif
    }

#if UNITY_EDITOR
    void LateUpdate()
    {
        if (!subscriptionConfigured)
            Debug.LogException(new InvalidOperationException(gameObject.name +
                " overrides Notificable.Start() and doesn't call its base."));
    }
#endif

    #region Notifications
    public virtual void OnBeep() { }
	public virtual void OnFlip() { }
	public virtual void OnFail() { }
	public virtual void OnDead() { }
	public virtual void OnTurn() { }
	public virtual void OnSave() { }
	public virtual void OnFall() { }
	public virtual void OnLand() { }
	#endregion

	/// <summary>
	/// Set <see cref="subscriptions"/> content, regardless if <see cref="listening"/> or not.
	/// </summary>
	/// <remarks>
	/// Having <see cref="Notification.Any"/> by default avoids children forget to subscribe.
	/// </remarks>
	protected virtual void ConfigureSubscriptions()
	{
		subscriptions = Notification.Any;
	}
}
