using UnityEngine;
using System.Collections.Generic;
using Utils.Extensions;

public enum ModularGroupType
{
	/// <summary>
	/// Just one of the sprites can be selected.
	/// </summary>
	Exclusive,
	/// <summary>
	/// All sprites can be selected together.
	/// </summary>
	Inclusive,
	/// <summary>
	/// Some of the sprites can be selected together.
	/// </summary>
	Diverse,
	/// <summary>
	/// A few sprites can be selected together, but usually none of them.
	/// </summary>
	Scarce
}

/// <summary>
/// <see cref="ScriptableObject"/> which merely groups a list of sprites candidates
/// and the relationship between these candidates when it comes to choose.
/// </summary>
[CreateAssetMenu(fileName = "ModularGroup", menuName = "Building/Modular Group", order = 2)]
public class ModularGroup : ScriptableObject
{
	public List<Sprite> sprites;
	public ModularGroupType type;
}
