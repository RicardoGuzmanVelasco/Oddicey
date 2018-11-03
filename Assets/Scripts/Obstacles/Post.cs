using System.Collections;
using System.Linq;
using UnityEngine;
using Utils.Directions;
using Utils.Extensions;

/// <summary>
/// Checkpoint. It breaks after save player position.
/// </summary>
public class Post : Notificable
{
    /// <summary>
    /// Direction the die was pointing to when it reached this checkpoint.
    /// </summary>
    Direction direction;
    /// <summary>
    /// Sprite of the arrow that points the saved direction.
    /// </summary>
    SpriteRenderer arrowSprite;
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
            //TODO: replace by animation play. That animation will make the checked effect.
            arrowSprite.enabled = true;
            arrowSprite.flipX = direction == Direction.left;
        }
    }
    #endregion

    void Awake()
    {
        arrowSprite = gameObject.GetComponentInChildren<SpriteRenderer>("Arrow");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
            return;

        Direction = collision.GetComponent<RollingCube>().direction;
        GetComponent<Collider2D>().enabled = false;

        notifier.NotificateSave(); //All posts will set isLastCheckpoint to false.
        isLastCheckpoint = true; //Just this post will set isLastCheckpoint to true.
    }

    #region Notifications
    protected override void ConfigureSubscriptions()
    {
        subscriptions = Notification.Save | Notification.Unsave;
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
        isLastCheckpoint = playManager.LastCheckpoint.position == (Vector2)transform.position;
        Direction = playManager.LastCheckpoint.direction;

        arrowSprite.enabled = playManager.IsSavedCheckpoint(transform.position);
        if(!arrowSprite.enabled)
            StartCoroutine(ReactivateCollider());
    }

    protected IEnumerator ReactivateCollider()
    {
        yield return new WaitForFixedUpdate(); //This wait saferly avoids a new save every death teleporting.
        GetComponent<Collider2D>().enabled = true;
    }
}
