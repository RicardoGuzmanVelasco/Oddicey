using UnityEngine;
using Utils.Directions;

/// <summary>
/// Represents a 2D checkpoint data in world space.
/// </summary>
public struct Checkpoint
{
    /// <summary>
    /// Space point the checkpoint is saved.
    /// </summary>
    public Vector2 position;
    /// <summary>
    /// Towards the player was directing to when it reached the checkpoint.
    /// </summary>
    public Direction direction;

    /// <summary>
    /// Makes a new checkpoint with <see cref="Player"/> current position and direction.
    /// </summary>
    /// <param name="player"></param>
    public Checkpoint(Vector2 position, Direction direction)
    {
        this.position = position;
        this.direction = direction;
    }
}
