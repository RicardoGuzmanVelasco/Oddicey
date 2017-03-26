using UnityEngine;

public class LazyMark : Mark
{
	/// <summary>
	/// LazyMark takes a beep more on waking up and then remains active forever.
	/// </summary>
	bool awaken = false;

	public override void OnBeep()
	{
		if (awaken)
			State = true;
		base.OnBeep();
	}

	protected override void OnRight()
	{
		awaken = true;
		if (!State)
			renderer.color = Color.yellow;
	}

	protected override void OnWrong()
	{
		awaken = false;
		base.OnWrong();
	}
}
