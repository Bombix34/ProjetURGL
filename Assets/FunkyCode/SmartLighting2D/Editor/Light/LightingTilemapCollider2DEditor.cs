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
				script.rectangle.colliderType = (LightingTilemapCollider.Rectangle.ColliderType)EditorGUILayout.EnumPopup("Shadow Type", script.rectangle.colliderType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.colliderType == LightingTilemapCollider.Rectangle.ColliderType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				switch(script.rectangle.colliderType) {
					case LightingTilemapCollider.Rectangle.ColliderType.Grid:
					case LightingTilemapCollider.Rectangle.ColliderType.SpriteCustomPhysicsShape:
						script.colliderTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.colliderTileType);
					break;
				}

				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.rectangle.maskType = (LightingTilemapCollider.Rectangle.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.rectangle.maskType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				if (script.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.BumpedSprite) {
					GUIBumpMapMode.Draw(serializedObject, script);
				}

				EditorGUI.EndDisabledGroup();

			break;

			case MapType.UnityIsometric:
				
				script.isometric.colliderType = (LightingTilemapCollider.Isometric.ColliderType)EditorGUILayout.EnumPopup("Shadow Type", script.isometric.colliderType);
				
				EditorGUI.BeginDisabledGroup(script.isometric.colliderType == LightingTilemapCollider.Isometric.ColliderType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				script.colliderTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.colliderTileType);
				
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.isometric.maskType = (LightingTilemapCollider.Isometric.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.isometric.maskType);
				
				EditorGUI.BeginDisabledGroup(script.isometric.maskType == LightingTilemapCollider.Isometric.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				EditorGUI.EndDisabledGroup();

			break;


			case MapType.UnityHexagon:
				
				script.hexagon.colliderType = (LightingTilemapCollider.Hexagon.ColliderType)EditorGUILayout.EnumPopup("Shadow Type", script.hexagon.colliderType);
				
				EditorGUI.BeginDisabledGroup(script.hexagon.colliderType == LightingTilemapCollider.Hexagon.ColliderType.None);

				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				script.colliderTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.colliderTileType);
					
				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.hexagon.maskType = (LightingTilemapCollider.Hexagon.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.hexagon.maskType);
				
				EditorGUI.BeginDisabledGroup(script.hexagon.maskType == LightingTilemapCollider.Hexagon.MaskType.None);

				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				EditorGUI.EndDisabledGroup();
			break;

			case MapType.SuperTilemapEditor:
				script.superTilemapEditor.colliderType = (SuperTilemapEditorSupport.TilemapCollider2D.ColliderType)EditorGUILayout.EnumPopup("Shadow Type", script.superTilemapEditor.colliderType);
			
				script.shadowLayer = EditorGUILayout.Popup("Shadow Layer (Light)", script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				EditorGUILayout.Space();

				script.superTilemapEditor.maskType = (SuperTilemapEditorSupport.TilemapCollider2D.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.superTilemapEditor.maskType);
				
				EditorGUI.BeginDisabledGroup(script.superTilemapEditor.maskType == SuperTilemapEditorSupport.TilemapCollider2D.MaskType.None);
				
				script.maskLayer = EditorGUILayout.Popup("Mask Layer (Light)", script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				EditorGUI.EndDisabledGroup();
			break;
		}

		EditorGUILayout.Space();

		UpdateCollisions(script);
		
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

	static void UpdateCollisions(LightingTilemapCollider2D script) {
		if (GUILayout.Button("Update")) {
			CustomPhysicsShapeManager.Clear();
			
			script.Initialize();

			LightingSource2D.ForceUpdateAll();
			LightingManager2D.ForceUpdate();
		}
	}
}

#endif