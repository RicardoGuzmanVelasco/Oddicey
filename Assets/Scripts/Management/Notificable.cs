using System;
using UnityEngine;

/// <summary>
/// <see cref="Notificable"/> can both receive a <see cref="Notification"/> event
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
				notifier.Subscribe(this, subscriptions);
			else if(!value)
				notifier.Unsubscribe(this);
			listening = value;
		}
	}
	#endregion

	/// <summary>
	/// Will self-subscribe when “start”.
	/// </summary>
	protected virtual void Start()
	{
        notifier = FindObjectOfType<Notifier>();
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
    public virtual void OnBeep()    { }
	public virtual void OnFlip()    { }
	public virtual void OnFail()    { }
	public virtual void OnDead()    { }
	public virtual void OnTurn()    { }
	public virtual void OnSave()    { }
    public virtual void OnUnsave()  { }
	public virtual void OnFall()    { }
	public virtual void OnLand()    { }
    public virtual void OnWalk()    { }
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

    /// <summary>
    /// Will self-unsubscribe when it's destroyed.
    /// </summary>
    protected virtual void OnDestroy()
    {
        Listening = false;
    }
}
