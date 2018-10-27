using UnityEngine;
using Utils.Extensions;

/// <summary>
/// This component remains unabled. It rebuild the platform when its size is changed.
/// </summary>
/// <remarks>
/// Enable the component isn't dangerous, as far as is known so far.
/// Thus, as a <see cref="Builder"/>, it selfdestroys when playing.
/// </remarks>
[ExecuteInEditMode]
public class PlatformBuilder : Builder
{
	[SerializeField]
	[Range(3, 50)]
	int size = 3;

	/// <summary>
	/// Builds a platform of <see cref="size"/> squares.
	/// </summary>
	/// <remarks>
	/// 1- It keeps the "left" and "right" tiles untouched.
	/// 2- It calls to <see cref="Clean"/> "center" tiles but one.
	/// 3- It populates all outstanding "center" tiles until fill <see cref="size"/>-2.
	/// 4- It puts the "right" tile on the new right edge.
	/// 5- It reinstantiates the "right" tile in order to stay it down on the hierarchy.
	/// 6- It ends the <see cref="rebuild"/> flag. Then disables the component itself.
	/// </remarks>
	protected override void Build()
	{
        base.Build();

		Clean();

        GameObject centerTile = transform.Find("Center").gameObject;
        GameObject rightTile = transform.Find("Right").gameObject;

		//Repopulate with 'size-2' "Center" childs (in local positions).
		for(int i = 3; i < size; i++)
			centerTile.Clone((Vector2)transform.position + Vector2.right * Builder.ToUnits(i - 1));

		//Put right tile on the new right end (in local position).
		rightTile.Clone((Vector2)transform.position + Vector2.right * Builder.ToUnits(size - 1), rightTile.name);
		DestroyImmediate(rightTile);
		//Clone & Destroy let "Right" stay down on the hierarchy.

		this.enabled = false;
	}

    /// <summary>
    /// Destroy all inner childs but one. Also keeps just one "right" tile.
    /// </summary>
    /// <remarks>
    /// Refresh tiles and keeps Left>Center>Right priority using <see cref="GameObjectExtensions.Clone(GameObject, string)"/>,
    /// <see cref="Object.DestroyImmediate(Object)"/> and <see cref="Transform.SetSiblingIndex(int)"/>.
    /// </remarks>
    void Clean()
	{   var leftTile = transform.Find("Left").gameObject;
        leftTile.Clone().transform.SetSiblingIndex(0);
        DestroyImmediate(leftTile);
         
        var centerTile = transform.DestroyDuplicatedChildren("Center");
        centerTile.Clone();
        DestroyImmediate(centerTile);

        transform.DestroyDuplicatedChildren("Right");
	}

    /// <summary>
    /// If <see cref="size"/> changed, platform needs <see cref="Build"/>.
    /// </summary>
	void OnValidate()
	{
		//TODO: support size=2, or even size=1 (less priority).
		if(size<3)
		{
			Debug.LogWarning("Minimum size of Platforms is 3.\n" + size + " was resized to the minimum.");
			size = 3;
		}
		this.enabled = true;
		rebuild = true;
	}
}
