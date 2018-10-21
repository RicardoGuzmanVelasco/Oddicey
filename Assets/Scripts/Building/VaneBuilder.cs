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
public class VaneBuilder : Builder
{
    /// <summary>
    /// Vane component to build.
    /// </summary>
    Vane attachedVane;

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

        DestroyImmediate(attachedVane);
        switch (type)
        {
            case VaneType.Flipping:
                attachedVane = gameObject.AddComponent<FlippingVane>();
                break;
            case VaneType.Fixed:
                attachedVane = gameObject.AddComponent<FixedVane>();
                break;
            default:
                attachedVane = gameObject.AddComponent<Vane>();
                break;
        }

        attachedVane.Dir = direction;
    }

    private void OnValidate()
    {
        attachedVane = GetComponent<Vane>();
        rebuild = true;
    }
}
