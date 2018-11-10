using System.Collections.Generic;
using UnityEngine;

public class PlayManager : Notificable
{
	Player player;
    /// <summary>
    /// Collection of saved checkpoints the player passed through.
    /// </summary>
    /// <remarks>
    /// Being a <see cref="Stack{T}"/> lets the player recursively delete last checkpoint.
    /// </remarks>
	Stack<Checkpoint> checkpoints;
    public Checkpoint LastCheckpoint
    {
        get
        {
            return checkpoints.Peek();
        }
    }

    void Awake()
	{
		player = FindObjectOfType<Player>();
        checkpoints = new Stack<Checkpoint>();
	}

    protected override void Start()
    {
        base.Start(); //Notificable will self-subscribe when "start".

        checkpoints.Push(new Checkpoint(player));
    }

    /// <summary>
    /// Whether or not there is any checkpoint in <paramref name="position"/> saved on <see cref="checkpoints"/>.
    /// </summary>
    public bool IsSavedCheckpoint(Vector2 position)
    {
        foreach (var checkpoint in checkpoints)
            if (checkpoint.position == position)
                return true;
        return false;
    }

    #region Notifications
    /// <remarks>
    /// <para><see cref="Notification.Fail"/>: how fail a mark obstacle impacts in gameplay.</para>
    /// <para><see cref="Notification.Dead"/>: handle dead. Teleport to checkpoint, stop motor system...</para>
    /// <para><see cref="Notification.SavingGroup"/>: store or discard element in checkpoints stack.</para>
    /// </remarks>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Fail | Notification.Dead | Notification.SavingGroup;
	}

	public override void OnFail()
    {
        notifier.NotificateDead();
    }

    public override void OnDead()
    {
        player.Teleport(checkpoints.Peek().position);
        player.GetComponent<RollingCube>().direction = checkpoints.Peek().direction;

        GetComponent<MotorSystem>().Moving = false;
    }

    public override void OnSave()
	{
		checkpoints.Push(new Checkpoint(player));
	}

    /// <summary>
    /// Deletes the last saved checkpoint if exists, excluding starting point.
    /// </summary>
    public override void OnUnsave()
    {
        if(checkpoints.Count>1)
            checkpoints.Pop();
    }

    #endregion
}
