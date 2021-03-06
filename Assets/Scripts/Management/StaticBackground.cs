﻿using UnityEngine;
using Utils.Extensions;

public class StaticBackground : MonoBehaviour
{
	/// <summary>
	/// Original center position of background.
	/// </summary>
	float xOrigin;
	/// <summary>
	/// Units between edge of camera and edge of background.
	/// </summary>
	float xOffset;

	/// <summary>
	/// Difference between real and used size. Avoid potential problems when end is reached.
	/// </summary>
	const float error = 0.98f;

	Transform cam;
	/// <summary>
	/// Original position of camera (right edge).
	/// </summary>
	float camOrigin;
	/// <summary>
	/// Units between camera center and right edge.
	/// </summary>
	float camOffset;
	float endOfLevel;

	void Awake()
	{
		endOfLevel = GameObject.Find("EOL").transform.position.x;
	}

	void Start()
	{
		Camera camera = GetComponentInParent<Camera>();
		cam = camera.transform;
		camOrigin = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, 0)).x;
		camOffset = camOrigin - cam.position.x;

		xOrigin = transform.localPosition.x;
		float backgroundOffset = xOrigin + GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.localScale.x * error;
		xOffset = backgroundOffset - camOffset;
	}

	void Update()
	{
		float camEdge = cam.position.x + camOffset;
		float camX = (camEdge - camOrigin) / (endOfLevel - camOrigin);

		transform.localPosition = transform.localPosition.X(xOrigin - xOffset * camX);
	}


}
