﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MarkType
{
    Default,
    Lazy,
    Sleepy
}

[ExecuteInEditMode]
public class MarkBuilder : Builder
{
    /// <summary>
    /// Mark component to build.
    /// </summary>
    Mark attachedMark;

    /// <summary>
    /// Target side of the mark when play starts.
    /// 0 means random side.
    /// </summary>
    [Tooltip("0 = randomize on play")]
    [SerializeField]
    [Range(0, 6)]
    int sideRequired = 0;
    /// <summary>
    /// What type the mark to build will be.
    /// </summary>
    [SerializeField]
    MarkType type = MarkType.Default;
    /// <summary>
    /// Sprites list the mark to build will have.
    /// </summary>
    /// <remarks>
    /// Must have size=6 and sprites orderer from 1 side to 6 side skins.
    /// </remarks>
    [SerializeField]
    Sprite[] sprites = new Sprite[6];

    protected override void Build()
    {
        base.Build();

        DestroyImmediate(attachedMark);
        switch (type)
        {
            case MarkType.Lazy:
                attachedMark = gameObject.AddComponent<LazyMark>();
                break;
            case MarkType.Sleepy:
                attachedMark = gameObject.AddComponent<SleepyMark>();
                break;
            default:
                attachedMark = gameObject.AddComponent<Mark>();
                break;
        }
        attachedMark.sprites = sprites;

        attachedMark.randomSide = sideRequired == 0;
        attachedMark.sideRequired = sideRequired;
        //Draw on edit mode if no random side.
        attachedMark.GetComponent<SpriteRenderer>().sprite = sideRequired != 0 ? sprites[sideRequired - 1] : null;
    }

    private void OnValidate()
    {
        attachedMark = GetComponent<Mark>();
        rebuild = true;
    }
}