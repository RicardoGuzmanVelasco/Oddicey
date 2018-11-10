using System.Collections;
using System.Linq;
using UnityEngine;
using Utils.Directions;
using Utils.Extensions;

/// <summary>
/// Checkpoint. It stops being interactable after save player position.
/// </summary>
[RequireComponent(typeof(PostController))]
public class Post : Notificable
{
    /// <summary>
    /// Direction the die was pointing to when it reached this checkpoint.
    /// </summary>
    Direction direction;
    /// <summary>
    /// If this post is the last checkpoint on <see cref="PlayManager.checkpoints"/>.
    /// </summary>
    protected bool isLastCheckpoint;

    #region Properties
    public Direction Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
            GetComponent<PostController>().SetDirection(direction);
        }
    }
    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            return;

        Direction = collision.GetComponent<RollingCube>().direction;
        GetComponent<Collider2D>().enabled = false;

        notifier.NotificateSave(); //All posts, including this, will set isLastCheckpoint to false.
        isLastCheckpoint = true; //Just this post will set to true.

        GetComponent<PostController>().ShowArrow(true);
    }

    #region Notifications
    /// <summary>
    /// <para><see cref="Notification.SavingGroup"/>: check if handle saving changes.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
    {
        subscriptions = Notification.SavingGroup;
    }

    public override void OnSave()
    {
        isLastCheckpoint = false;
    }

    public override void OnUnsave()
    {
        StartCoroutine(CheckIfNewLastCheckpoint());
    }
    #endregion

    IEnumerator CheckIfNewLastCheckpoint()
    {
        yield return new WaitForEndOfFrame();

        var playManager = FindObjectOfType<PlayManager>();

        isLastCheckpoint = (Vector2)transform.position == playManager.LastCheckpoint.position;

        bool isSavedCheckpoint = playManager.IsSavedCheckpoint(transform.position);
        GetComponent<PostController>().ShowArrow(isSavedCheckpoint);
        if(!isSavedCheckpoint)
            StartCoroutine(ReactivateCollider());

        if(isLastCheckpoint)
            Direction = playManager.LastCheckpoint.direction;
    }

    protected IEnumerator ReactivateCollider()
    {
        //This waits safely avoids a new save every death teleporting.
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        GetComponent<Collider2D>().enabled = true;
    }
}
