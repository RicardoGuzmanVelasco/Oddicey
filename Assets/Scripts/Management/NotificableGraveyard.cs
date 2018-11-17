using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificableGraveyard : MonoBehaviour
{
    HashSet<Notificable> undeads;

    void Awake()
    {
        undeads = new HashSet<Notificable>();
    }

    public void Add(Notificable notificable)
    {
        if(!undeads.Add(notificable))
            Debug.LogError(notificable + " have already subscribed into the graveyard");
    }

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
}
