using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save a collection of <see cref="Notificable"/> as a cache, in order to reactivate them.
/// </summary>
/// <remarks>
/// Insofar as <see cref="Notificable.Listening"/> is the only way to keep subscribed,
/// this class will store all objects which want to resubscribe in the future.
/// </remarks>
/// <example>
/// A <see cref="WeakPost"/> is consumed so it breaks and falls. It is unsubscribed, but
/// in the next death, it must reset is state, so player can save its checkpoint again.
/// That's made by the graveyard because this weak post isn't listening death anymore.
/// </example>
public class NotificableGraveyard : Notificable
{
    HashSet<Notificable> undeads;
    HashSet<Notificable> undeadsBuffer;

    void Awake()
    {
        undeads = new HashSet<Notificable>();
        undeadsBuffer = new HashSet<Notificable>();
    }

    #region Notifications
    protected override void ConfigureSubscriptions()
    {
        subscriptions = Notification.Dead | Notification.Walk;
    }

    public override void OnDead()
    {
        Revive();
    }

    public override void OnWalk()
    {
        DrawOffBuffer();
    }
    #endregion

    /// <summary>
    /// Keeps <paramref name="notificable"/> waiting for <see cref="Revive"/>.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// If it had already added. It mustn't occur by flow, so if it's thrown,
    /// then there is any unintentional call or code design.
    /// </exception>
    public void Add(Notificable notificable)
    {
        if(undeads.Contains(notificable) || !undeadsBuffer.Add(notificable))
            throw new ArgumentException(notificable + " have already subscribed into the graveyard");
    }

    /// <summary>
    /// Enable all <see cref="Notificable"/> subscribed,
    /// and reset <see cref="undeads"/> list.
    /// </summary>
    void Revive()
    {
        foreach(var undead in undeads)
            undead.enabled = true;
        undeads.Clear();
    }

    /// <summary>
    /// Discards all current <see cref="Notificable"/> awaiting for reactivation.
    /// </summary>
    /// <example>
    /// When die reaches a new persistent checkpoint, all past <see cref="undeads"/> will never be revived.
    /// </example>
    public void Purgue()
    {
        //TODO: if necessary, destroy all Notificable components to optimization purposes.
        undeads.Clear();
    }

    void DrawOffBuffer()
    {
        foreach(var awaitingUndead in undeadsBuffer)
            undeads.Add(awaitingUndead);
        undeadsBuffer.Clear();
    }
}
