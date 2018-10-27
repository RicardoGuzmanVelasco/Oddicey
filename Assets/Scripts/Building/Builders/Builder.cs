using System.Collections;
using UnityEngine;
using Utils.Extensions;

/// <summary>
/// Any builder is a helper with square, tiles info
/// which will be deleted when playing.
/// </summary>
public abstract class Builder : MonoBehaviour
{
    public const float SquareSize = 4;

    /// <summary>
    /// If OnValidate() saw any change which needs rebuilt.
    /// </summary>
    protected bool rebuild = false;

    void Awake()
    {
        if(Application.isPlaying)
            Destroy(this);
    }

    void Update()
    {
        if(rebuild)
            Build();
    }

    protected virtual void Build()
    {
        rebuild = false;
    }

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
