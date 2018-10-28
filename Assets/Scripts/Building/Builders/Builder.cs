using System.Collections;
using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Any builder is a helper with square, tiles info.
/// Lets a designer build on edit mode, while it will be deleted when playing.
/// </summary>
public abstract class Builder : MonoBehaviour
{
    /// <summary>
    /// Relation between Unity default units and grid tile units.
    /// </summary>
    public const float SquareSize = 4;

    /// <summary>
    /// If a property of an instance has any change which needs rebuilt.
    /// </summary>
    protected bool rebuild = false;

    /// <summary>
    /// <see cref="Builder"/> will self-destroy on play mode.
    /// </summary>
    void Awake()
    {
        if(Application.isPlaying)
            Destroy(this);
    }

    /// <summary>
    /// This check encapsulates a <see cref="Build"/> call after validate change of property in editor.
    /// </summary>
    void Update()
    {
        if(rebuild)
            Build();
    }

    /// <summary>
    /// Deactivates <see cref="rebuild"/> bool-flag.
    /// </summary>
    /// <remarks>
    /// The actual build functionality must be override by child builders.
    /// </remarks>
    protected virtual void Build()
    {
        rebuild = false;
    }

    /// <summary>
    /// Keeps <see cref="UnityEngine.Object.name"/> updated,
    /// both when <see cref="attached"/> or <see cref="SquareTransform"/> changes.
    /// </summary>
    /// <remarks>
    /// It helps designer roles to shorty see the whole picture in hierarchy view,
    /// following the trail of the object thanks to their types and grid positions.
    /// </remarks>
    public virtual void UpdateName()
    {
        gameObject.name = GetComponent<SquareTransform>() + " " + GetType().ToString().Trim("Builder");
    }

    #region Static methods
    public static bool IsSquare(Vector2 position)
    {
        return position.x % SquareSize <= float.Epsilon && position.y % SquareSize <= float.Epsilon;
    }

    public static bool IsSquare(float units)
    {
        return units % SquareSize <= float.Epsilon;
    }

    /// <summary>
    /// Convert from <paramref name="units"/> to game squares.
    /// </summary>
    /// <param name="units">Size in units.</param>
    /// <returns><paramref name="units"/> converted to squares.</returns>
    public static float ToSquares(float units)
    {
        return units / SquareSize;
    }

    /// <summary>
    /// Convert from <paramref name="squares"/> to scene units.
    /// </summary>
    /// <param name="squares">Size in squares.</param>
    /// <returns><paramref name="squares"/> converted to units.</returns>
    public static float ToUnits(float squares)
    {
        return squares * SquareSize;
    }
    #endregion
}
