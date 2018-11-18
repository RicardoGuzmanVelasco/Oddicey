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

    public override void Event(string trigger, bool state)
    {
        switch(trigger)
        {
            case "ShowArrow":
                ShowArrow(state);
                break;
            case "LastCheckpoint":
                SetLastCheckpoint(state);
                break;
            case "RightDirection":
                SetDirection(state ? Direction.right : Direction.left);
                break;
            default:
                throw new NotImplementedException();
                //break;
        }
    }

    void SetDirection(Direction direction)
    {
        animator.SetTrigger("Bounce");
        arrowSprite.flipX = direction == Direction.left;
    }

    void ShowArrow(bool show)
    {
        if(!show && arrowSprite.gameObject.activeSelf)
            Event("Wobble");
        arrowSprite.gameObject.SetActive(show);
    }

    void SetLastCheckpoint(bool active)
    {
        signSprite.color = active ? lastCheckpointColor : Color.white;
    }
}
