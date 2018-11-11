using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows a rect representing some positions relevant to the player as die, last checkpoint, etc.
/// </summary>
/// <remarks>
/// EXECUTION ORDER delayed!!!
/// <see cref="OnUnsave"/> must be called after <see cref="PlayManager.OnUnsave"/>.
/// This way, last checkpoint will always be the updated last checkpoint but one.
/// </remarks>
[RequireComponent(typeof(Slider))]
public class LevelCircuit : Notificable
{
    private Vector3 startingPoint, endingPoint;

    /// <summary>
    /// Main slider handler, used as die sprite.
    /// </summary>
    [SerializeField]
    Slider playerSlider;
    /// <summary>
    /// Second slider handler, used as post sprite. 
    /// </summary>
    [SerializeField]
    Slider checkpointSlider;

    GameObject player;
    /// <summary>
    /// Reference to <see cref="PlayManager"/> to update when <see cref="Notification.Unsave"/>.
    /// </summary>
    PlayManager playManager;

    private void Awake()
    {
        player = FindObjectOfType<Die>().gameObject;
        playManager = FindObjectOfType<PlayManager>();
    }

    protected override void Start()
    {
        base.Start();

        startingPoint = player.transform.position;
        endingPoint = GameObject.Find("EOL").transform.position;
        UpdateCheckpointSlider();
    }

    private void Update()
    {
        playerSlider.value = NormalizeIntoCircuit(player.transform.position.x);
    }

    /// <summary>
    /// Maps a value bounded within [<see cref="startingPoint"/>,<see cref="endingPoint"/>].
    /// </summary>
    /// <param name="x">Value to normalize into circuit rect.</param>
    /// <returns>Normalize value within [0,1].</returns>
    private float NormalizeIntoCircuit(float x)
    {
        return (x - startingPoint.x) / (endingPoint.x - startingPoint.x);
    }

    #region Notifications
    /// <summary>
    /// <para><see cref="Notification.SavingGroup"/>: set checkpoint flag icon if necessary.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
    {
        subscriptions = Notification.SavingGroup;
    }

    public override void OnSave()
    {
        checkpointSlider.gameObject.SetActive(true); //TODO: will be an animation making "pop" effect or something.
        checkpointSlider.value = playerSlider.value;
    }

    public override void OnUnsave()
    {
        UpdateCheckpointSlider();
    }

    private void UpdateCheckpointSlider()
    {
        float lastCheckpointX = playManager.LastCheckpoint.position.x;
        checkpointSlider.value = NormalizeIntoCircuit(lastCheckpointX);
        checkpointSlider.gameObject.SetActive(lastCheckpointX != startingPoint.x);
    }
    #endregion
}
