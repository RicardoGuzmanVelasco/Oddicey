using UnityEngine;

/// <summary>
/// Trigger which change the Y tracking of the main camera.
/// </summary>
public class YTrack : MonoBehaviour
{
	/// <summary>
	/// More semantic on the inspector.
	/// Just a bool could be enough.
	/// </summary>
	enum TrackTriggerType { TrackOn, TrackOff }
	[SerializeField]
	TrackTriggerType type = TrackTriggerType.TrackOn;

	CameraTracker cam;

	private void Awake()
	{
		cam = Camera.main.transform.GetComponent<CameraTracker>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(!(collision.tag == "Player"))
			return;
	
		cam.Y = type == TrackTriggerType.TrackOn;
        cam.AutoY = false;
	}
}
