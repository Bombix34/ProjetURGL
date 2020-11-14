using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FafaFloat))]
public class FafaFloatInspector : Editor {
	public override void OnInspectorGUI()
	{

		FafaVariable fafaVariable = (FafaVariable)target;
		DrawDefaultInspector();
		if(EditorApplication.isPlaying && fafaVariable.resetAtStart){
			fafaVariable.localValue = EditorGUILayout.FloatField("Game Value", (float)fafaVariable.localValue);
		}
	}
}
