﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PostType
{
    Default,
    Unbreakable,
    Weak
}

[ExecuteInEditMode]
public class PostBuilder : ObstacleBuilder<Post>
{
    /// <summary>
    /// What type the vane to build will be.
    /// </summary>
    [SerializeField]
    PostType type = PostType.Default;
    /// <summary>
    /// If a <see cref="WeakPost"/>, how many attemps are set.
    /// </summary>
    int attemps = 1;

    #region Build assembly line
    protected override void UpdateAttachedTypeName()
    {
        AttachedTypeName = type.ToString();
    }

    protected override void BeforeUpdateAttached()
    {
        if(attached.GetType() == typeof(WeakPost))
            attemps = (attached as WeakPost).Attemps;
        else
            attemps = 1;
    }

    protected override void AfterUpdateAttached()
    {
        if((attached as WeakPost)!=null)
            (attached as WeakPost).Attemps = attemps;
    }
    #endregion
}
