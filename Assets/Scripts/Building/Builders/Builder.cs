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
    /// An optional name the GameObject built by this script will own.
    /// </summary>
    [SerializeField]
    string hierarchyName;

    /// <summary>
    /// <see cref="Builder"/> will self-destroy on play mode.
    /// </summary>
    /// <remarks>
    /// TODO: eventually this code could be restricted to UNITY_EDITOR,
    /// and also any builder won't exist in !UNITY_EDITOR environments. 
    /// </remarks>
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

    #region Name in hierarchy
    /// <summary>
    /// Keeps <see cref="UnityEngine.Object.name"/> updated with <see cref="SquareTransform.ToString"/>,
    /// both when <see cref="attached"/> or <see cref="SquareTransform"/> changes.
    /// </summary>
    /// <remarks>
    /// It helps designer roles to shorty see the whole picture in hierarchy view,
    /// following the trail of the object thanks to their types and grid positions.
    /// </remarks>
    public void UpdateName()
    {
        gameObject.name = GetComponent<SquareTransform>() + " ";
        gameObject.name += hierarchyName.HasContent() ? hierarchyName : BaseName();
    }

    /// <summary>
    /// The name this game object will have in hierarchy beside <see cref="SquareTransform.ToString"/>.
    /// </summary>
    /// <remarks>
    /// Override this in a child class if the base hierarchy name must be specialized.
    /// </remarks>
    protected virtual string BaseName()
    {
        return GetType().ToString().Trim("Builder");
    }
    #endregion

    #region Static methods
    /// <summary>
    /// Giving a space point, returns whether or not its the center of a square in this builder space.
    /// </summary>
    public static bool IsSquare(Vector2 position)
    {
        return position.x % SquareSize <= float.Epsilon &&
               position.y % SquareSize <= float.Epsilon;
    }
    /// <summary>
    /// Giving a space single axis, returns whether or not its the center of a square in this builder space.
    /// </summary>
    public static bool IsSquare(float units)
    {
        return units % SquareSize <= float.Epsilon;
    }

    /// <summary>
    /// Converts from <paramref name="units"/> to game squares.
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
