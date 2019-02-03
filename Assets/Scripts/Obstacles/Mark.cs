using UnityEngine;

/// <summary>
/// Obstacle that switch on when <see cref="Die.side"/> fits with its own. 
/// </summary>
/// <remarks>
/// "Switch on" means here OK state, obstacle passed.
/// </remarks>
public class Mark : Notificable
{
    /// <summary>
    /// If <see cref="sideRequired"/> must be shuffled within [1,6] or it's prefixed.
    /// </summary>
    [HideInInspector]
    public bool randomSide;
    /// <summary>
    /// Side that will switch on the obstacle.
    /// </summary>
    [HideInInspector]
    public int sideRequired;
    /// <summary>
    /// Whether or not switched on (OK or WRONG state).
    /// </summary>
    private bool state;

    /// <summary>
    /// Player's die reference.
    /// </summary>
    [HideInInspector]
    public Die player;

    /// <summary>
    /// Sprites list for each side of the die.
    /// </summary>
    [HideInInspector]
    public Sprite[] sprites;
    /// <summary>
    /// Unique renderer of the current side.
    /// </summary>
    protected SpriteRenderer spriteRenderer;

    #region Properties
    /// <value>
    /// This value change too the mark skin!
    /// </value>
    public bool State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
            //TODO: will be changed by animation play. That animation will make the color change.
            spriteRenderer.color = state ? Color.green : Color.red;
            transform.parent.GetComponent<SpriteRenderer>().color = spriteRenderer.color; //TEST: to see easily cows.
        }
    }
    #endregion

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Die>();
    }

    protected override void Start()
    {
        base.Start(); //Notificable will self-subscribe.
        SelectRandomSide();
    }

    /// <summary>
    /// Chooses a random side if applicable — it is, <see cref="randomSide"/>.
    /// </summary>
	void SelectRandomSide()
    {
        if(!randomSide)
            return;

        sideRequired = Random.Range(1, 7); //'max' exclusive when int randomization.
        spriteRenderer.sprite = sprites[sideRequired - 1];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            CheckSuccess();
    }

    #region Notifications
    /// <summary>
    /// <para><see cref="Notification.Beep"/>: keep mark state updated.</para>
    /// <para><see cref="Notification.Flip"/>: update mark state immediately.</para>
    /// <para><see cref="Notification.Dead"/>: reactivation.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
    {
        subscriptions = Notification.Beep | Notification.Flip | Notification.Dead;
    }

    /// <summary>
    /// Checking state every beep, towards state changes if necessary.
    /// </summary>
    public override void OnBeep()
    {
        CheckRight();
    }

    /// <summary>
    /// Checking state when die changes side, towards skin changes if necessary.
    /// </summary>
    /// <remarks>
    /// Just for better visual response. If player flips die side early, there is
    /// a little offset until next beep, so mark feels taking a long time to set.
    /// </remarks>
    public override void OnFlip()
    {
        CheckRight();
    }

    /// <summary>
    /// Reactivate if was passed, and new side. So far, random selection.
    /// </summary>
	public override void OnDead()
    {
        SelectRandomSide();
        spriteRenderer.enabled = true;
    }
    #endregion

    protected virtual void CheckRight()
    {
        if(IsRight())
            OnRight();
        else
            OnWrong();
    }

    protected bool IsRight()
    {
        return player.Side == sideRequired;
    }

    protected virtual void OnRight()
    {
        State = true;
    }

    protected virtual void OnWrong()
    {
        State = false;
    }

    /// <summary>
    /// Actual checking of mark state. Usually happens when die reaches mark trigger.
    /// </summary>
    public void CheckSuccess()
    {
        if(State)
            OnSuccess();
        else
            OnFailure();
    }

    /// <summary>
    /// Obstacle passed.
    /// </summary>
    protected virtual void OnSuccess()
    {
        //TODO: replay by effect on gameplay.
        Debug.Log("Success");
        //Listening = false;
        spriteRenderer.enabled = false;
    }

    /// <summary>
    /// Obstacle not passed.
    /// </summary>
    protected virtual void OnFailure()
    {
        //TODO: replace by effect on gameplay. Death, rewind, -1 lives... whatever.
        Debug.Log("Fail");
        notifier.NotificateFail();
    }
}
