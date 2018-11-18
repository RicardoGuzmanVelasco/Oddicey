using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPost : Post
{
    /// <summary>
    /// How many times player can respawn before this post breaks, thus lose its <see cref="Checkpoint"/>.
    /// </summary>
    [SerializeField]
    [Range(1, 3)]
    int attemps = 1;
    int currentAttemps = 0; //TODO: if a consumable can reset attempts, property here? or notification...

    bool broken = false;

    #region Properties
    public int Attemps
    {
        get
        {
            return attemps;
        }

        set
        {
            attemps = value;
        }
    }
    #endregion

    #region Subscriptions
    /// <summary>
    /// <seealso cref="Post.ConfigureSubscriptions"/>.
    /// <para><see cref="Notification.Dead"/>: break this post when attempts have been consumed.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
    {
        base.ConfigureSubscriptions();
        subscriptions |= Notification.Dead | Notification.Walk;
    }

    public override void OnDead()
    {
        if(!IsLastCheckpoint)
            return;

        if(currentAttemps++ >= attemps)
            Break();
    }
    #endregion

    void Break()
    {
        broken = true;
        controller.Event("Break");
        enabled = false;
        notifier.NotificateUnsave();
    }

    /// <summary>
    /// Reset <see cref="currentAttemps"/> and visual state.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();

        if(!broken)
            return;

        controller.Event("Reset");
        controller.Event("ShowArrow", false);
        broken = false;
        GetComponent<Collider2D>().enabled = true;
        currentAttemps = 0;
        IsLastCheckpoint = false;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if(FindObjectOfType<NotificableGraveyard>())
            FindObjectOfType<NotificableGraveyard>().Add(this);
    }
}
