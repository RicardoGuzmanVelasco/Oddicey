using UnityEngine;
using System.Collections.Generic;
using Utils.Extensions;
using System;

/// <summary>
/// Draw sprites from <see cref="groups"/> as children of a <see cref="GameObject"/> called "/Sprites".
/// It requires <see cref="SpriteRenderer"/> component to set the sprites setup.
/// </summary>
/// <remarks>
/// As a child of <see cref="Builder"/>, it self-destroys on play mode,
/// so <see cref="Start"/> acts just on edit mode.
/// </remarks>
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class ModularChooser : Builder
{
	GameObject spritesContainer;

	[SerializeField]
	List<ModularGroup> groups;

	bool flipX;
	bool flipY;
	string sortingLayer;

	void Start()
	{
        sortingLayer = GetComponent<SpriteRenderer>().sortingLayerName;
        flipX = GetComponent<SpriteRenderer>().flipX;
        flipY = GetComponent<SpriteRenderer>().flipY;
        Draw();
	}

	public void Draw()
	{
		Clean();

		List<Sprite> spritesToDraw = ChooseSprites();

		spritesToDraw.Reverse(); //Let the sorting layer order be correct.
		for(int i = 0; i < spritesToDraw.Count; i++)
		{
			Sprite sprite = spritesToDraw[i];
			GameObject gameobject = spritesContainer.CreateChild(sprite.name);
			CreateSpriteComponent(sprite, gameobject, i + 1);
		}
	}

	/// <summary>
	/// Selects within each group of sprites.
	/// </summary>
	/// <returns>The list of modular sprites that conforms the final sprite.</returns>
	private List<Sprite> ChooseSprites()
	{
		List<Sprite> chosen = new List<Sprite>();

		foreach(var group in groups)
			chosen.AddRange(Choose(group));
		return chosen;
	}

	/// <summary>
	/// Selects sprites from a group according to the group type. 
	/// </summary>
	/// <param name="group">ModularGroup of sprites.</param>
	/// <returns>The list with selected sprites (just one in the list if exclusive type).</returns>
	List<Sprite> Choose(ModularGroup group)
	{
		List<Sprite> selected = new List<Sprite>();

		switch(group.type)
		{
			case ModularGroupType.Exclusive:
				selected.Add(group.sprites.GetRandom());
				break;
			case ModularGroupType.Inclusive:
				foreach(var sprite in group.sprites)
					selected.Add(sprite);
				break;
			case ModularGroupType.Diverse:
				foreach(var sprite in group.sprites)
					if(UnityEngine.Random.value < 0.5)
						selected.Add(sprite);
				break;
			case ModularGroupType.Scarce:
				foreach(var sprite in group.sprites)
					if(UnityEngine.Random.value < 0.25)
						selected.Add(sprite);
				break;
		}

		return selected;
	}


	/// <summary>
	/// Remove previous sprites if there are.
	/// </summary>
	private void Clean()
	{
		//Refactor when C#6.0 with ?.
		if(transform.Find("Sprites") != null)
			DestroyImmediate(transform.Find("Sprites").gameObject);
		spritesContainer = gameObject.CreateChild("Sprites");
	}

	private void CreateSpriteComponent(Sprite sprite, GameObject gameObject, int index)
	{
		SpriteRenderer spriteComponent = gameObject.AddComponent<SpriteRenderer>();
		spriteComponent.sprite = sprite;
		spriteComponent.sortingLayerName = sortingLayer; //TODO: assert sortingLayer exists.

		spriteComponent.sortingOrder = index * 10;

		if(flipX)
			spriteComponent.flipX = true;
		if(flipY)
			spriteComponent.flipY = true;
	}

}
