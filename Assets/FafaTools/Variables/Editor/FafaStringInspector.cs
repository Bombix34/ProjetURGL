using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FafaString))]
public class FafaStringInspector : Editor {
	public override void OnInspectorGUI()
	{

		FafaVariable fafaVariable = (FafaVariable)target;
		DrawDefaultInspector();
		if(EditorApplication.isPlaying && fafaVariable.resetAtStart){
			fafaVariable.localValue = EditorGUILayout.TextField("Game Value", (string)fafaVariable.localValue);
		}
	}
}
