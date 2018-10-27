using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Directions;

public enum VaneType
{
    Default,
    Flipping,
    Fixed
}

[ExecuteInEditMode]
public class VaneBuilder : ObstacleBuilder<Vane>
{
    /// <summary>
    /// What type the vane to build will be.
    /// </summary>
    [SerializeField]
    VaneType type = VaneType.Default;
    [SerializeField]
    Direction direction = Direction.left;

    protected override void Build()
    {
        base.Build();

        DestroyImmediate(attached);
        switch (type)
        {
            case VaneType.Flipping:
                attached = gameObject.AddComponent<FlippingVane>();
                break;
            case VaneType.Fixed:
                attached = gameObject.AddComponent<FixedVane>();
                break;
            default:
                attached = gameObject.AddComponent<Vane>();
                break;
        }

        attached.Dir = direction;
    }
}
