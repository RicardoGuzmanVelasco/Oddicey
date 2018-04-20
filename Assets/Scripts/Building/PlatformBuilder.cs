using UnityEngine;
using Utils.Extensions;

[ExecuteInEditMode]
public class PlatformBuilder : Builder
{
	[SerializeField]
	[Range(3, 20)]
	int size = 3;

	[SerializeField] GameObject innerTile;
	[SerializeField] GameObject outerTile;

	void Update()
	{
		Clean();

		//Repopulate with 'size-2' "Center" childs (in local positions).
		innerTile.name = "Center";
		for (int i = 3; i < size; i++)
			innerTile.Clone((Vector2)transform.position + Vector2.right * Builder.ToUnits(i - 1));

		outerTile = transform.Find("Right").gameObject;
		//Put right tile on the new right end (in local position).
		outerTile.Clone((Vector2)transform.position + Vector2.right * Builder.ToUnits(size - 1), outerTile.name);
		DestroyImmediate(outerTile);
		//Clone & Destroy let "Right" stay down on the hierarchy.
	}

	/// <summary>
	/// Destroy all inner childs but one.
	/// </summary>
	void Clean()
	{
		innerTile = transform.Find("Center").gameObject;
		innerTile.name = "CenterTemplate";
		transform.DestroyChildren("Center");
	}

	void OnValidate()
	{
		if(size<3)
		{
			Debug.LogWarning("Minimum size of Platforms is 3.\n" + size + " was resized to the minimum.");
			size = 3;
		}
	}
}
