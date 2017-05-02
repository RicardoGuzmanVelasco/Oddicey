using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabsBuilder))]
public class PrefabsBuilderEditor : Editor
{
	PrefabsBuilder script;

	void OnEnable()
	{
		script = target as PrefabsBuilder;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GUILayout.Space(10);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button(new GUIContent("+", "Spawn selected prefab"), GUILayout.Width(25)))
			script.SpawnObject();
		if (GUILayout.Button(new GUIContent("-", "Destroy selected object"), GUILayout.Width(25)))
			script.DestroyObject();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
	}
}