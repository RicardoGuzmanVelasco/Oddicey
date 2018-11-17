using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPost : Post
{
    /// <summary>
    /// How many times player can respawn before this post breaks, thus lose its <see cref="Checkpoint"/>.
    /// </summary>
    [SerializeField]
    [Range(1,3)]
    int attemps = 1;
    int currentAttemps = 0; //TODO: if a consumable can reset attempts, property here? or notification...

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
        if (!IsLastCheckpoint)
            return;

        if(currentAttemps++ >= attemps)
            Break();
    }

    void Break()
    {
        GetComponent<PostController>().Break();
        enabled = false;
        notifier.NotificateUnsave();
    }
    #endregion
}
