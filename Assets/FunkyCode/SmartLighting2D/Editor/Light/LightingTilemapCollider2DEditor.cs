using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using LightingTilemapCollider;

#if UNITY_2017_4_OR_NEWER

[CustomEditor(typeof(LightingTilemapCollider2D))]
public class LightingTilemapCollider2DEditor : Editor {

	override public void OnInspectorGUI() {
		LightingTilemapCollider2D script = target as LightingTilemapCollider2D;

		script.mapType = (MapType)EditorGUILayout.EnumPopup("Tilemap Type", script.mapType);

		EditorGUILayout.Space();

		switch(script.mapType) {
			case MapType.UnityRectangle:
				script.rectangle.shadowType = (Rectangle.ShadowType)EditorGUILayout.EnumPopup("Shadow Type", script.rectangle.shadowType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.shadowType == Rectangle.ShadowType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				switch(script.rectangle.shadowType) {
					case Rectangle.ShadowType.Grid:
					case Rectangle.ShadowType.SpriteCustomPhysicsShape:
						script.shadowTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.shadowTileType);
					break;
				}

				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.rectangle.maskType = (Rectangle.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.rectangle.maskType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.maskType == Rectangle.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				if (script.rectangle.maskType == Rectangle.MaskType.BumpedSprite) {
					GUIBumpMapMode.Draw(serializedObject, script);
				}

				EditorGUI.EndDisabledGroup();

			break;

			case MapType.UnityIsometric:
				
				script.isometric.shadowType = (Isometric.ShadowType)EditorGUILayout.EnumPopup("Shadow Type", script.isometric.shadowType);
				
				EditorGUI.BeginDisabledGroup(script.isometric.shadowType == Isometric.ShadowType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				script.shadowTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.shadowTileType);
				
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.isometric.maskType = (Isometric.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.isometric.maskType);
				
				EditorGUI.BeginDisabledGroup(script.isometric.maskType == Isometric.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				EditorGUI.EndDisabledGroup();

			break;


			case MapType.UnityHexagon:
				
				script.hexagon.shadowType = (Hexagon.ShadowType)EditorGUILayout.EnumPopup("Shadow Type", script.hexagon.shadowType);
				
				EditorGUI.BeginDisabledGroup(script.hexagon.shadowType == Hexagon.ShadowType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				script.shadowTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.shadowTileType);
					
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.hexagon.maskType = (Hexagon.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.hexagon.maskType);
				
				EditorGUI.BeginDisabledGroup(script.hexagon.maskType == Hexagon.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				EditorGUI.EndDisabledGroup();
			break;

			case MapType.SuperTilemapEditor:
				script.superTilemapEditor.shadowType = (SuperTilemapEditorSupport.TilemapCollider2D.ShadowType)EditorGUILayout.EnumPopup("Shadow Type", script.superTilemapEditor.shadowType);
			
				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				EditorGUILayout.Space();

				script.superTilemapEditor.maskType = (SuperTilemapEditorSupport.TilemapCollider2D.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.superTilemapEditor.maskType);
				
				EditorGUI.BeginDisabledGroup(script.superTilemapEditor.maskType == SuperTilemapEditorSupport.TilemapCollider2D.MaskType.None);
				
				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				EditorGUI.EndDisabledGroup();
			break;
		}

		EditorGUILayout.Space();

		Update(script);

		serializedObject.ApplyModifiedProperties();
		
		if (GUI.changed) {
			script.Initialize();

			LightingSource2D.ForceUpdateAll();
			LightingManager2D.ForceUpdate();

			if (EditorApplication.isPlaying == false) {
				EditorUtility.SetDirty(target);
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	static void Update(LightingTilemapCollider2D script) {
		if (GUILayout.Button("Update")) {
			CustomPhysicsShapeManager.Clear();
			
			script.Initialize();

			LightingSource2D.ForceUpdateAll();
			LightingManager2D.ForceUpdate();
		}
	}
}

#endif