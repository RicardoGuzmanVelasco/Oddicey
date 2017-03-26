using UnityEngine;

public class SleepyMark : Mark
{
	/// <summary>
	/// SleepyMark awakes just for a beep and falls asleep again.
	/// </summary>
	bool asleep = false;

	public override void OnBeep()
	{
		base.OnBeep();

		if (State)
			if (asleep)
				OnWrong();
			else
				asleep = true;
	}

	protected override void OnWrong()
	{
		asleep = false;
		base.OnWrong();
	}
}
