using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(LightingCollider2D))]
public class LightingCollider2DEditor : Editor {
	LightingCollider2D lightCollider2D;

	SerializedProperty shadowType;
	SerializedProperty shadowLayer;
	SerializedProperty shadowEffectLayer;
	SerializedProperty shadowDistance;
	
	SerializedProperty maskType;
	SerializedProperty maskLayer;
	SerializedProperty maskEffect;

	SerializedProperty applyToChildren;

	private void InitProperties() {
		shadowType = serializedObject.FindProperty("shadowType");
		shadowLayer = serializedObject.FindProperty("shadowLayer");
		shadowEffectLayer = serializedObject.FindProperty("shadowEffectLayer");
		shadowDistance = serializedObject.FindProperty("shadowDistance");
		
		maskType = serializedObject.FindProperty("maskType");
		maskLayer = serializedObject.FindProperty("maskLayer");
		maskEffect = serializedObject.FindProperty("maskEffect");

		applyToChildren = serializedObject.FindProperty("applyToChildren");
	}

	private void OnEnable(){
		lightCollider2D = target as LightingCollider2D;

		InitProperties();
		
		Undo.undoRedoPerformed += RefreshAll;
	}

	internal void OnDisable(){
		Undo.undoRedoPerformed -= RefreshAll;
	}

	void RefreshAll(){
		LightingCollider2D.ForceUpdateAll();
	}

	override public void OnInspectorGUI() {
		if (lightCollider2D == null) {
			return;
		}
		
		// Shadow Properties

		EditorGUILayout.PropertyField(shadowType, new GUIContent ("Shadow Type"));

		EditorGUI.BeginDisabledGroup(shadowType.intValue == (int)LightingCollider2D.ShadowType.None);

		shadowLayer.intValue = EditorGUILayout.Popup("Shadow Layer (Light)", shadowLayer.intValue, Lighting2D.Profile.layers.lightLayers.GetNames());

		shadowEffectLayer.intValue = EditorGUILayout.Popup("Shadow Effect Layer (Light)", shadowEffectLayer.intValue, Lighting2D.Profile.layers.lightLayers.GetNames());

		string shadowDistanceName = "Shadow Distance";

		if (shadowDistance.floatValue == 0) {
			shadowDistanceName = "Shadow Distance (infinite)";
		}

		EditorGUILayout.PropertyField(shadowDistance, new GUIContent (shadowDistanceName));

		EditorGUI.EndDisabledGroup();

		EditorGUILayout.Space();

		// Mask Properties

		EditorGUILayout.PropertyField(maskType, new GUIContent ("Mask Type"));

		EditorGUI.BeginDisabledGroup(maskType.intValue == (int)LightingCollider2D.MaskType.None);

		maskLayer.intValue = EditorGUILayout.Popup("Mask Layer (Light)", maskLayer.intValue, Lighting2D.Profile.layers.lightLayers.GetNames());

		EditorGUILayout.PropertyField(maskEffect, new GUIContent ("Mask Effect"));

		EditorGUI.EndDisabledGroup();

		if (lightCollider2D.maskType == LightingCollider2D.MaskType.BumpedSprite) {
			GUIBumpMapMode.Draw(serializedObject, lightCollider2D);
		}

		EditorGUILayout.Space();

		// Apply to Children
		
		EditorGUILayout.PropertyField(applyToChildren, new GUIContent ("Apply to Children"));

		serializedObject.ApplyModifiedProperties();


		EditorGUILayout.Space();

		// Update

		if (GUILayout.Button("Update")) {
			CustomPhysicsShapeManager.Clear();

			foreach(Object target in targets) {
				LightingCollider2D lightCollider2D = target as LightingCollider2D;
				lightCollider2D.Initialize();
			}

			LightingManager2D.ForceUpdate();
		}

		if (GUI.changed) {
			foreach(Object target in targets) {
				LightingCollider2D lightCollider2D = target as LightingCollider2D;
				lightCollider2D.Initialize();
				lightCollider2D.UpdateNearbyLights();

				if (EditorApplication.isPlaying == false) {
					EditorUtility.SetDirty(target);
				}
			}

			if (EditorApplication.isPlaying == false) {
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}

			LightingManager2D.ForceUpdate();
		}
	}
}
