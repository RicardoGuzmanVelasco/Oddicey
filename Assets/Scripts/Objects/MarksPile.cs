using System.Collections.Generic;
using UnityEngine;
using Utils.Extensions.VectorExtensions;

public class MarksPile : Notificable
{
	List<Mark> marks = new List<Mark>();
	int foot = 0; //Index of marks that is in foot.
	bool state;

	public Portal Gate;

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

	private void StackMarks()
	{
		for (int i = foot; i < marks.Count; i++)
		{
			//Stacking marks by hierarchy order.
			marks[i].transform.position = transform.position.Y(-4 + (i - foot) * 4);
			if (marks[i].Listening)
				marks[i].Listening = false;
		}
		if (!State)
			marks[foot].Listening = true; //Just the foot of the pile subscribes.
	}

	public override void OnBeep()
	{
		if (State)
			return;
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
}
