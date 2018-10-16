using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LevelCircuit : Notificable
{
	private Vector3 startingPoint, endingPoint;

	/// <summary>
	/// Main slider handler, used as die sprite.
	/// </summary>
	[SerializeField]
	private Slider playerSlider;
	/// <summary>
	/// Second slider handler, used as post sprite. 
	/// </summary>
	[SerializeField]
	private Slider checkpointSlider;

	private GameObject player;

	protected override void Start()
	{
		base.Start();

		player = FindObjectOfType<Die>().gameObject;

		startingPoint = player.transform.position;
		endingPoint = GameObject.Find("EOL").transform.position;
	}

	private void Update()
	{
		playerSlider.value = (player.transform.position.x - startingPoint.x) / (endingPoint.x - startingPoint.x);
	}

	#region Notifications
	protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Save;
	}

	public override void OnSave()
	{
		checkpointSlider.value = playerSlider.value;
	}
	#endregion
}
