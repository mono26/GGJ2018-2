using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AsteroidSpawner))]
public class AsteroidSpawnerEditor : SpawnerEditor 
{
	public override void OnInspectorGUI() 
	{
		base.OnInspectorGUI();
	}
}
