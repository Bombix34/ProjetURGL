using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Buffers))]
public class CameraBuffersEditor : Editor {
	static bool cameraFoldout = false;
	static bool lightFoldout = false;

	override public void OnInspectorGUI() {
		Buffers script = target as Buffers;

		cameraFoldout = EditorGUILayout.Foldout(cameraFoldout, "Cameras");

		if (cameraFoldout) {

			EditorGUI.indentLevel++;

			foreach(LightingMainBuffer2D buffer in LightingMainBuffer2D.list) {
				EditorGUILayout.ObjectField("Camera Target", buffer.cameraSettings.GetCamera(), typeof(Camera), true);
				
				EditorGUILayout.EnumPopup("Camera Type", buffer.cameraSettings.cameraType);
				EditorGUILayout.EnumPopup("Render Mode", buffer.cameraSettings.renderMode);
				EditorGUILayout.EnumPopup("Render Shader", buffer.cameraSettings.renderShader);
				EditorGUILayout.ObjectField("Render Texture", buffer.renderTexture.renderTexture, typeof(Texture), true);

				EditorGUILayout.Space();
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space();

		}

		lightFoldout = EditorGUILayout.Foldout(lightFoldout, "Lights");

		if (lightFoldout) {

			EditorGUI.indentLevel++;

			foreach(LightingBuffer2D buffer in LightingBuffer2D.list) {
				EditorGUILayout.LabelField(buffer.name);
				EditorGUILayout.ObjectField("Lighting Source", buffer.Light, typeof(LightingSource2D), true);

				EditorGUILayout.Toggle("Is Free", buffer.Free);

				EditorGUILayout.ObjectField("Render Texture", buffer.renderTexture.renderTexture, typeof(Texture), true);

				EditorGUILayout.Space();
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space();

		}
		
	}
}