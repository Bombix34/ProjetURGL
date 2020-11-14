using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FafaInt))]
public class FafaIntInspector : Editor {
	public override void OnInspectorGUI()
	{

		FafaVariable fafaVariable = (FafaVariable)target;
		DrawDefaultInspector();
		if(EditorApplication.isPlaying && fafaVariable.resetAtStart){
			fafaVariable.localValue = EditorGUILayout.IntField("Game Value", (int)fafaVariable.localValue);
		}
	}
}
