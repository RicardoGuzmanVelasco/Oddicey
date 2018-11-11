using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions;

public class MarksPile : Notificable
{
	/// <summary>
	/// Marks shaping the pile.
	/// </summary>
	List<Mark> marks = new List<Mark>();
	/// <summary>
	/// Index of mark that is at the bottom of the pile (the only one listening).
	/// </summary>
	int foot = 0;
	/// <summary>
	/// If marks remaining.
	/// </summary>
	/// <value>
	/// If <see langword="false"/>, still not passed. Marks remaining.
    /// <see langword="true"/> otherwise.
	/// </value>
	bool state;

	/// <summary>
	/// Portal linked with this pile, if any.
	/// </summary>
	public Portal gate;

    #region Properties
    /// <summary>
    /// Setting State to true will send Open instruction to the linked gate.
    /// </summary>
    public bool State
	{
		get
		{
			return state;
		}

		private set
		{
			if (gate != null)
				gate.Open();
			state = value;
            //TODO: close.
		}
	}
	#endregion

	void Awake()
	{
		foreach (var mark in GetComponentsInChildren<Mark>() )
		{
			marks.Add(mark);
			mark.GetComponent<Collider2D>().enabled = false;
		}
	}

	protected override void Start()
	{
        base.Start(); //The pile self-subscribes.
        StackMarks();
	}

	/// <summary>
	/// Set the marks shaping a pile, one on top of the other.
	/// </summary>
	void StackMarks()
	{
		for (int i = foot; i < marks.Count; i++)
		{
            //Stacking marks containers, like cows, by 'hierarchy' order.
            Transform containerTransform = marks[i].GetComponentInParent<MarkContainer>().transform;
            containerTransform.position = transform.position.Y(Builder.ToUnits(i - foot));
			if (marks[i].Listening) //TODO: unnecessary conditional.
				marks[i].Listening = false;
		}
		if (!State)
			marks[foot].Listening = true; //Just the foot of the pile subscribes.
	}

    #region Notifications
    /// <summary>
    /// <para><see cref="Notification.Beep"/>: reshape when popping out a mark.</para>
    /// </summary>
    protected override void ConfigureSubscriptions()
	{
		subscriptions = Notification.Beep;
	}

	/// <summary>
	/// Reshaping if foot mark is passed, changing State if was the last.
	/// </summary>
	public override void OnBeep()
	{
		if (State)
			return;
		//May cause throughput decreases. TODO?: avoid generate aux Mark each beep.
		Mark footMark = marks[foot];
		if (footMark.State)
		{
			footMark.Listening = false;
			footMark.gameObject.SetActive(false);
			if (++foot >= marks.Count)
			{
				State = true;
				return;
			}
			StackMarks();
		}
	}
	#endregion
}
