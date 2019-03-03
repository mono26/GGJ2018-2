using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoDestroyComponent))]
public class AutoDestroyEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		EditorGUILayout.Separator();

		AutoDestroyComponent autoDestroy = target as AutoDestroyComponent;

		if (GUILayout.Button("Auto Destroy"))
		{
			autoDestroy.AutoDestroy();
		}
	}
}
