using System;
using UnityEngine;

public abstract class Notificable : MonoBehaviour
{
	protected News subscriptions;
	bool listening;

	#region Properties
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
	/// Notificable will selfsubscribe when “start”.
	/// </summary>
	protected virtual void Start()
	{
		ConfigureSubscriptions();
		Listening = true;
	}

	protected virtual void ConfigureSubscriptions()
	{
		subscriptions = News.Any;
	}
}
