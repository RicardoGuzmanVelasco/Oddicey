﻿using System;
using UnityEngine;

public class Turner : Notificable
{
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
			TurnPlayer(collision.GetComponent<RollingCube>());
	}

	protected virtual void TurnPlayer(RollingCube rollingCube)
	{
		rollingCube.Turn();
	}
}
