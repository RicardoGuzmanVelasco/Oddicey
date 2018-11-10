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

    protected override void Awake()
    {
        base.Awake();
        arrowSprite = gameObject.GetComponentInChildren<SpriteRenderer>("Arrow");
    }

    public void SetDirection(Direction direction)
    {
        animator.SetTrigger("Bounce");
        arrowSprite.flipX = direction == Direction.left;
    }

    public void Unsave()
    {
        animator.SetTrigger("Wobble");
    }

    public void ShowArrow(bool show)
    {
        arrowSprite.gameObject.SetActive(show);
    }

    public void Break()
    {
        animator.SetTrigger("Break");
    }
}
