using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoDestroy))]
public class AutoDestroyEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.Separator();

		AutoDestroy autoDestroy = target as AutoDestroy;

		if (GUILayout.Button("Auto Destroy"))
		{
			autoDestroy.AutoDestroyObject();
		}
	}
}
