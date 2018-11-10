using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Notificable
{
	public GameObject player;
	Die die;

	/// <summary>
	/// Amount of sides to flip.
	/// '3' internally represents the no order (flip=0).
	/// '0' internally represents the '3' order (flip to opposite side).
	/// </summary>
	/// <remarks>
	/// Any <=-3 or =>3 order will be ignored.
	/// </remarks>
	enum FlipOrder
	{
		NoFlip = 3,
		Back1 = -1,
		Back2 = -2,
		Flip = 0,
		Plus1 = 1,
		Plus2 = 2,
	}
	FlipOrder flipOrder = FlipOrder.NoFlip;
	/// <summary>
	/// If die can flip its side on current beep.
	/// </summary>
	bool flipEnabled = false;

	#region Properties
	public bool FlipEnabled
	{
		get
		{
			return flipEnabled;
		}

		set
		{
			flipEnabled = value;
		}
	}
	#endregion

	void Awake()
	{
		die = player.GetComponent<Player>().GetComponent<Die>();
	}

	void Update()
	{
		#region Test Controls
		if(Input.GetButtonUp("Test1"))
		{
			GetComponent<MotorSystem>().Moving = !GetComponent<MotorSystem>().Moving;
            if (GetComponent<MotorSystem>().Moving)
            {
                flipEnabled = true;
                notifier.NotificateWalk();
            }
		}
		if(Input.GetButtonUp("Test2"))
			notifier.NotificateDead();
		if(Input.GetButtonUp("Test3"))
			GetComponent<MotorSystem>().Tempo += 10;
		if(Input.GetButtonUp("Test4"))
			GetComponent<MotorSystem>().Tempo -= 10;
		if(Input.GetButtonUp("Test5"))
			die.GetComponent<RollingCube>().Turn();
        if(Input.GetButtonUp("Test6"))
            notifier.NotificateUnsave();
        if (Input.GetButtonUp("Test7"))
            notifier.NotificateDead();
		#endregion

		if(!FlipEnabled)
			return; //TODO: When player try to flip twice in a roll;

		if(Input.GetButtonUp("Flip"))
			flipOrder = FlipOrder.Flip;
		else if(Input.GetButtonUp("Flip+1"))
			flipOrder = FlipOrder.Plus1;
		else if(Input.GetButtonUp("Flip-1"))
			flipOrder = FlipOrder.Back1;
		else if(Input.GetButtonUp("Flip+2"))
			flipOrder = FlipOrder.Plus2;
		else if(Input.GetButtonUp("Flip-2"))
			flipOrder = FlipOrder.Back2;

		if(flipOrder == FlipOrder.NoFlip)
			return;

        if(!die.Flip((int)flipOrder))
            notifier.NotificateLate();
        else
        {
            notifier.NotificateFlip();
            FlipEnabled = false;
        }

		flipOrder = FlipOrder.NoFlip;
	}

    #region Notifications
    /// <remarks>
    /// <para><see cref="Notification.Beep"/>: keep flip disabled while die is falling.</para>
    /// <para><see cref="Notification.Late"/>: how try to flip side out of time impacts on gameplay.</para>
    /// <para><see cref="Notification.FallingGroup"/>: disable flip if die is falling.</para>
    /// </remarks>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Beep | Notification.FallingGroup | Notification.Late;
	}

    public override void OnLate()
    {
        Debug.Log("OutOfTime");
    }

    //This overriding did make the die not properly falling.
    public override void OnBeep()
	{
		FlipEnabled = !player.GetComponent<RollingCube>().Falling;
	}

	public override void OnFall()
	{
		FlipEnabled = false;
	}

	public override void OnLand()
	{
		FlipEnabled = true;
	}
	#endregion
}
