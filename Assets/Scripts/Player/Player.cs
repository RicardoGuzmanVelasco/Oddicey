using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
	RollingCube cube;
	Rigidbody2D body;

	void Awake()
	{
		cube = GetComponent<RollingCube>();
		body = GetComponent<Rigidbody2D>();

        //Prevents rolling problems when this GameObject is selected while play mode.
        Destroy(GetComponent<SquareTransform>());
	}

	public void Teleport(Vector3 position)
	{
		StartCoroutine(DelayTeleport(position));
	}

	/// <summary>
	/// Wait until rotation is completed before teleport.
	/// </summary>
	IEnumerator DelayTeleport(Vector3 position)
	{
		yield return new WaitWhile(() => cube.rolling);
		transform.position = position;
		cube.floor = position.y;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
        switch(collision.tag)
        {
            case "NotFloor":
                //Avoid collision between top of the die and past NotFloor in "downstairs" scheme.
                if(collision.transform.position.y <= Mathf.RoundToInt(transform.position.y))
                    OnGroundLost();
                break;
            case "Floor":
                if(cube.Falling)
                    OnGroundGained();
                break;
            case "Boundary":
                if(collision.name == "BOTTOM")
                    FindObjectOfType<Notifier>().NotificateFail(); //TODO: change: If EOL, no fail!
                break;
        }
	}

    void OnGroundLost()
	{
		cube.Falling = true;
		FindObjectOfType<Notifier>().NotificateFall();
	}

	public void OnGroundGained()
	{
		cube.Falling = false;
		body.velocity = Vector2.zero;
		cube.floor = transform.position.y;
		cube.Snap();

		FindObjectOfType<Notifier>().NotificateLand();
	}
}
