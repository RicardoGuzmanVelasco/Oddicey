using System;
using UnityEngine;

public abstract class Notificable : MonoBehaviour
{
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
			if (value && !listening)
				FindObjectOfType<Notifier>().Subscribe(this);
			else if (!value)
				FindObjectOfType<Notifier>().Unsubscribe(this);
			listening = value;
		}
	}
	#endregion

	public virtual void OnBeep() { }
	public virtual void OnFlip() { }
	public virtual void OnFail() { }
	public virtual void OnDead() { }
	public virtual void OnTurn() { }
	/// <summary>
	/// Notificable will selfsubscribe when “start”.
	/// </summary>
	protected virtual void Start()
	{
		Listening = true;
	}
}
