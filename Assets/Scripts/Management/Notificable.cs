using System;
using UnityEngine;

public abstract class Notificable : MonoBehaviour
{
	bool listening;

	public bool Listening
	{
		get
		{
			return listening;
		}

		set
		{
			if (value)
				FindObjectOfType<Notifier>().Subscribe(this);
			else
				FindObjectOfType<Notifier>().Unsubscribe(this);
			listening = value;
		}
	}

	public virtual void OnBeep() { }
	public virtual void OnFlip() { }
	public virtual void OnFail() { }
	public virtual void OnDead() { }
	public virtual void OnTurn() { }
	/// <summary>
	/// Notificable will selfsubscribe when “awake”.
	/// </summary>
	protected virtual void Start()
	{
		Listening = true;
	}
}
