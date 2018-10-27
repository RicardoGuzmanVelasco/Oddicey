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
    int currentAttemps = 0; //TODO: if a consumable can reset attemps, property here? or notification...

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
    protected override void ConfigureSubscriptions()
    {
        base.ConfigureSubscriptions();
        subscriptions |= Notification.Dead;
    }

    public override void OnDead()
    {
        if (!isLastCheckpoint)
            return;

        if(++currentAttemps >= attemps)
            Destroy(gameObject); //TODO: will be an animation and maybe particles before destroys.
    }

    protected override void OnDestroy()
    {
        base.OnDestroy(); //Notificable will self-unsubscribe when it's destroyed.
        notifier.NotificateUnsave();
    }

    #endregion
}
