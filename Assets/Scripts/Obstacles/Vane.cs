using UnityEngine;
using Utils.Extensions;
using Utils.Directions;

/// <summary>
/// Change player's direction where the vane itself points.
/// </summary>
/// <remarks>
/// Change his own direction when player does, always being against the player.
/// </remarks>
[RequireComponent(typeof(Animator))]
public class Vane : Obstacle
{
	/// <summary>
	/// Course the die will take when collides.
	/// </summary>
    /// <remarks>
    /// CAVEAT: keep this SERIALIZED, preventing coherence fails.
    /// </remarks>
    [SerializeField]
    Direction dir;
	Direction startingDir;

    public Direction Dir
    {
        get
        {
            return dir;
        }
        
        set
        {
            gameObject.GetComponentInChildren<SpriteRenderer>("Arrow").flipX = value == Direction.right;
            dir = value;
        }
    }

    protected override void Start()
	{
        base.Start();
        startingDir = dir;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			TurnPlayer(collision.GetComponent<RollingCube>());
	}

	/// <summary>
	/// Send the player a <see cref="RollingCube.Turn"/> instruction.
	/// </summary>
	/// <param name="rollingCube">Player's <see cref="RollingCube"/> assigned.</param>
	protected virtual void TurnPlayer(RollingCube rollingCube)
	{
		rollingCube.Turn(dir);
	}

    #region Notifications
    /// <summary>
    /// <para><see cref="Notification.Turn"/>: turns as long as the die does.</para>
    /// <para><see cref="Notification.Dead"/>: reset starting direction.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Turn | Notification.Dead;
	}

	public override void OnTurn()
	{
		TurnAround();
	}

	public override void OnDead()
	{
        if(dir != startingDir)
            TurnAround();
	}
	#endregion

	/// <summary>
	/// Reverse <see cref="dir"/> and make visual change.
	/// </summary>
	protected void TurnAround()
	{
		dir = dir.Reverse();
        controller.Event("Turn");
	}
}
