using UnityEngine;

/// <summary>
/// Obstacle that switch on when die side of the player fits with its own. 
/// </summary>
/// <remarks>
/// It takes a beep more on waking up and then remains active forever.
/// </remarks>
public class LazyMark : Mark
{
	/// <summary>
	/// If awaken state (it is, it will be awake the next beep).
	/// </summary>
	bool awaken = false;

	public override void OnBeep()
	{
		if (awaken)
			State = true;
		base.OnBeep();
	}

	public override void OnFlip()
	{
		if (IsRight())
			//TO-DO: replace by animation. That animation will change the color.
			spriteRenderer.color = Color.yellow;
	}

	protected override void OnRight()
	{
		awaken = true;
	}

	protected override void OnWrong()
	{
		awaken = false;
		base.OnWrong();
	}
}
