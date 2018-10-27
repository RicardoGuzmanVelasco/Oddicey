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
public class PostBuilder : Builder
{
    /// <summary>
    /// Post component to build.
    /// </summary>
    Post attachedPost;

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

        DestroyImmediate(attachedPost);
        switch (type)
        {
            case PostType.Unbreakable:
                attachedPost = gameObject.AddComponent<UnbreakablePost>();
                break;
            case PostType.Weak:
                attachedPost = gameObject.AddComponent<WeakPost>();
                (attachedPost as WeakPost).Attemps = attemps;
                break;
            default:
                attachedPost = gameObject.AddComponent<Post>();
                break;
        }
    }

    private void OnValidate()
    {
        attachedPost = GetComponent<Post>();

        if (attachedPost.GetType() == typeof(WeakPost))
            attemps = (attachedPost as WeakPost).Attemps;
        else
            attemps = 1;

        rebuild = true;
    }
}
