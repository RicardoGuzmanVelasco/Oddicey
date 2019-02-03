using UnityEngine;
using Utils.Extensions;

/// <summary>
/// This component remains unabled. It rebuilds the platform when its size changes.
/// </summary>
/// <remarks>
/// Enable the component isn't dangerous, as far as is known so far.
/// Thus, as a <see cref="Builder"/>, it self-destroys when playing.
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
	/// 2- It removes "center" tiles but one.
	/// 3- It populates all outstanding "center" tiles until fill <see cref="size"/>-2.
	/// 4- It puts the "right" tile on the new right edge.
	/// 5- It forces "right" tile stay down in the hierarchy.
	/// 6- It ends the <see cref="rebuild"/> flag. Then disables the component itself.
	/// </remarks>
	protected override void Build()
	{
        base.Build();

        GameObject centerTile = transform.DestroyDuplicatedChildren("Center");
        GameObject rightTile = transform.Find("Right").gameObject;

		//Repopulate with 'size-2' "Center" children (in local positions).
		for(int i = 3; i < size; i++)
			centerTile.Clone((Vector2)transform.position + Vector2.right * Builder.ToUnits(i - 1));

        //Put right tile on the new right end (in local position).
        rightTile.transform.localPosition = Vector2.right * Builder.ToUnits(size - 1);
        rightTile.transform.SetAsLastSibling();

        this.enabled = false;
	}

    /// <summary>
    /// If <see cref="size"/> changed, platform needs <see cref="Build"/>.
    /// </summary>
	void OnValidate()
	{
		//TODO: support size=2, or even size=1 (less priority).
		if(size<3)
		{
			Debug.LogWarning("Minimum size of Platforms is 3.\n" + size + " was resized..");
			size = 3;
		}
        rebuild = true;
        this.enabled = true;
	}
}
