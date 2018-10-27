using System.Collections;
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

    protected override void Build()
    {
        base.Build();
        
        if(attached.GetType() == typeof(WeakPost))
            attemps = (attached as WeakPost).Attemps;
        else
            attemps = 1;

        DestroyImmediate(attached);
        switch (type)
        {
            case PostType.Unbreakable:
                attached = gameObject.AddComponent<UnbreakablePost>();
                break;
            case PostType.Weak:
                attached = gameObject.AddComponent<WeakPost>();
                (attached as WeakPost).Attemps = attemps;
                break;
            default:
                attached = gameObject.AddComponent<Post>();
                break;
        }
    }
}
