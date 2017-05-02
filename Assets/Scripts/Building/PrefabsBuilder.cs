using UnityEngine;
using System.Collections.Generic;
using Utils.Extensions;

public class PrefabsBuilder : Builder
{
	public List<GameObject> prefabs;
	public int selectedPrefab = 0;

	public List<GameObject> objects = new List<GameObject>();
	public int selectedObject = 0;

	public void SpawnObject()
	{
		objects.Add(prefabs[selectedPrefab].Clone());

	}

	public void DestroyObject()
	{
		DestroyImmediate(objects[selectedObject]);
		objects.RemoveAt(selectedObject);
	}
}
