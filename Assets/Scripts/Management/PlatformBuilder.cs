using UnityEngine;
using Utils.Extensions;

[ExecuteInEditMode]
public class PlatformBuilder : MonoBehaviour
{
	[SerializeField]
	int size = 3;
	int lastSize = 3;

	GameObject innerTile;
	GameObject outerTile;

	void Awake()
	{
		if (Application.isPlaying)
			Destroy(this);
	}

	void Update()
	{
		if (lastSize == size)
			return;
		lastSize = size;

		//Destroy all inner childs but one.
		innerTile = transform.Find("Center").gameObject.Clone("CenterTemplate");
		foreach (Transform child in transform)
			if (child.name == "Center")
				DestroyImmediate(child.gameObject);
		innerTile.name = "Center";

		outerTile = transform.Find("Right").gameObject;
		 
		for (int i = 3; i < size; i++)
			innerTile.Clone(Vector2.right * 4 * (i-1));
		//Put right tile on the new right end.
		outerTile.Clone(Vector2.right * 4 * (size - 1), outerTile.name);
		DestroyImmediate(outerTile);
		//Clone & Destroy let "Right" stay down on the hierarchy.

		enabled = false;
	}

	void OnValidate()
	{
		size = Mathf.Max(3, size);
	}
}
