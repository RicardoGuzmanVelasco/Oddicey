using System.Collections;
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
    /// What type the mark to build will be.
    /// </summary>
    [SerializeField]
    MarkType type = MarkType.Default;

    protected override void Build()
    {
        base.Build();

        var sprites = attachedMark.sprites;
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
    }

    private void OnValidate()
    {
        attachedMark = GetComponent<Mark>();
        rebuild = true;
    }
}
