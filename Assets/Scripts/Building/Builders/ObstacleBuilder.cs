using System.Collections;
using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Any builder is a helper with square, tiles info
/// which will be deleted when playing.
/// </summary>
[ExecuteInEditMode]
public abstract class ObstacleBuilder<T> : Builder
{
    /// <summary>
    /// <see cref="T"/> component to build.
    /// </summary>
    protected T attached;

    void OnValidate()
    {
        attached = GetComponent<T>();
        rebuild = true;
        UpdateName();
    }

    public override void UpdateName()
    {
        gameObject.name = GetComponent<SquareTransform>() + " " + attached.GetType();
    }
}
