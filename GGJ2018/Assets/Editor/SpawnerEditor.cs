using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Spawner spawner = target as Spawner;

		if (GUILayout.Button("Spawn Object"))
		{
			spawner.Spawn();
		}
	}
}
