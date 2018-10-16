using System;
using UnityEngine;

public abstract class Notificable : MonoBehaviour
{
	protected Notifier notifier;
	protected Notification subscriptions;
	/// <summary>
	/// If it's subscribed to <see cref="Notifier"/>, regardless <see cref="subscriptions"/> content.
	/// </summary>
	bool listening;

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
	}

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
