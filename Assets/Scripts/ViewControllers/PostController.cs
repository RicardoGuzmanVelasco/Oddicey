using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;
using Utils.Directions;
using System;

public class PostController : Controller
{
    /// <summary>
    /// Sprite of the arrow that points the saved direction.
    /// </summary>
    SpriteRenderer arrowSprite;
    /// <summary>
    /// Sprite of the sign where the arrow is drawn.
    /// </summary>
    SpriteRenderer signSprite;
    [SerializeField]
    Color lastCheckpointColor;

    protected override void Awake()
    {
        base.Awake();
        arrowSprite = gameObject.GetComponentInChildren<SpriteRenderer>("Arrow");
        signSprite = gameObject.GetComponentInChildren<SpriteRenderer>("Sign");
    }

    public void SetDirection(Direction direction)
    {
        animator.SetTrigger("Bounce");
        arrowSprite.flipX = direction == Direction.left;
    }

    public void ShowArrow(bool show)
    {
        if(!show && arrowSprite.gameObject.activeSelf)
            Event("Wobble");
        arrowSprite.gameObject.SetActive(show);
    }

    public void SetLastCheckpoint(bool active)
    {
        signSprite.color = active ? lastCheckpointColor : Color.white;
    }
}
