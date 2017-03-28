using UnityEngine;

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
