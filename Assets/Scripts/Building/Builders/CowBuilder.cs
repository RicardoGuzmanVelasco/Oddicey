using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set on edit time the sorting layer wherein the cow lives.
/// </summary>
/// <remarks>
/// Uses the <see cref="SquareTransform"/> to inversely order layers.
/// This let any cow located to the left of other cow draw above.
/// </remarks>
[ExecuteInEditMode]
[RequireComponent(typeof(SquareTransform))]
[RequireComponent(typeof(SpriteRenderer))]
public class CowBuilder : Builder
{
    /// <summary>
    /// Cow sprite. TODO: It will need rewording if cow sprite is split.
    /// For example, a head sprite and a body sprite split with animate purposes.
    /// </summary>
    SpriteRenderer sprite;

    void OnEnable()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Set the <see cref="sprite"/> sorting order according to -<see cref="SquareTransform.X"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="OnDrawGizmos"/> is called when any sibling component calls its OnValidate().
    /// </remarks>
    private void OnDrawGizmos()
    {
        int square = GetComponent<SquareTransform>().X;
        sprite.sortingOrder = -square;
    }
}
