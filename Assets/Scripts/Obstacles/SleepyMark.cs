using UnityEngine;

/// <summary>
/// Obstacle that switch on when die side of the player fits with its own. 
/// </summary>
/// <remarks>
/// It remains awaken just a beep.
/// </remarks>
public class SleepyMark : Mark
{
	/// <summary>
	/// SleepyMark awakes just for a beep and falls asleep again.
	/// </summary>
	bool asleep = false;

	public override void OnBeep()
	{
		if (asleep)
			asleep = false;
		else
			OnWrong();
	}

	public override void OnFlip()
	{
		CheckRight();
		if (State)
			asleep = true;
	}
}
