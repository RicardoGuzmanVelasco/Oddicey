using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SquareTransform))]
public class SquareTransformEditor : Editor
{
	SquareTransform script;

	void OnEnable()
	{
		script = target as SquareTransform;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.Space(10);
		EditorGUILayout.HelpBox("This component will autosnap transform on squares.", MessageType.Warning);
		GUILayout.Space(5);

		GUILayout.BeginHorizontal();
		script.X = EditorGUILayout.IntField("Horizontal Square", script.X, GUILayout.ExpandWidth(false));
		if (GUILayout.Button("˂", GUILayout.ExpandWidth(false)))
			script.X--;
		if (GUILayout.Button("˃", GUILayout.ExpandWidth(false)))
			script.X++;
		GUILayout.EndHorizontal();

		GUILayout.Space(5);
		GUILayout.BeginHorizontal();
		script.Y = EditorGUILayout.IntField("Vertical Square", script.Y, GUILayout.ExpandWidth(false));
		if (GUILayout.Button("˅", GUILayout.ExpandWidth(false)))
			script.Y--;
		if (GUILayout.Button("˄", GUILayout.ExpandWidth(false)))
			script.Y++;
		GUILayout.EndHorizontal();
	}
}
