using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Directions;
using Utils.Extensions;

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

    #region Build assembly line
    protected override void UpdateAttachedTypeName()
    {
        AttachedTypeName = type.ToString();
    }

    protected override void AfterUpdateAttached()
    {
        attached.Dir = direction;
    }
    #endregion
}
