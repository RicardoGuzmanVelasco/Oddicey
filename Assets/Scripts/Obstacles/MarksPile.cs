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
	/// False: still not passed. Marks remaining.
	/// </value>
	bool state;

	/// <summary>
	/// Portal linked with this pile, if any.
	/// </summary>
	public Portal Gate;

	/// <summary>
	/// Setting State to true will send Open instruction to the linked gate.
	/// </summary>
	#region Properties
	public bool State
	{
		get
		{
			return state;
		}

		private set
		{
			if (Gate != null)
				Gate.Open();
			state = value;
		}
	}
	#endregion

	void Awake()
	{
		foreach (var mark in GetComponentsInChildren<Mark>())
		{
			marks.Add(mark);
			mark.GetComponent<Collider2D>().enabled = false;
		}
	}

	protected override void Start()
	{
		StackMarks();
		base.Start(); //The pile selfsubscribes.
	}

	/// <summary>
	/// Set the marks shaping a pile, one over other.
	/// </summary>
	private void StackMarks()
	{
		for (int i = foot; i < marks.Count; i++)
		{
			//Stacking marks by 'hierarchy' order.
			marks[i].transform.position = transform.position.Y(-Builder.SquareSize + Builder.ToUnits(i - foot));
			if (marks[i].Listening)
				marks[i].Listening = false;
		}
		if (!State)
			marks[foot].Listening = true; //Just the foot of the pile subscribes.
	}

	#region Notifications
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
