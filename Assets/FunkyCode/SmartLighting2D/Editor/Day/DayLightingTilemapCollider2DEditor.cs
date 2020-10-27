using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using LightingTilemapCollider;

#if UNITY_2017_4_OR_NEWER

[CustomEditor(typeof(DayLightingTilemapCollider2D))]
public class DayLightingTilemapCollider2DEditor : Editor {
    

    override public void OnInspectorGUI() {
        DayLightingTilemapCollider2D script = target as DayLightingTilemapCollider2D;

		script.tilemapType = (MapType)EditorGUILayout.EnumPopup("Tilemap Type", script.tilemapType);

        EditorGUILayout.Space();

        switch(script.tilemapType) {
			case MapType.UnityRectangle:
				script.rectangle.colliderType = (LightingTilemapCollider.Rectangle.ColliderType)EditorGUILayout.EnumPopup("Shadow Type", script.rectangle.colliderType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.colliderType == LightingTilemapCollider.Rectangle.ColliderType.None);

				script.shadowLayer = (LightingLayer)EditorGUILayout.Popup("Shadow Layer (Light)", (int)script.shadowLayer, Lighting2D.Profile.layers.lightLayers.GetNames());
				
				switch(script.rectangle.colliderType) {
					case LightingTilemapCollider.Rectangle.ColliderType.Grid:
					case LightingTilemapCollider.Rectangle.ColliderType.SpriteCustomPhysicsShape:
						script.shadowTileType = (ShadowTileType)EditorGUILayout.EnumPopup("Shadow Tile Type", script.shadowTileType);
					break;
				}

				EditorGUI.EndDisabledGroup();

				EditorGUILayout.Space();

				script.rectangle.maskType = (LightingTilemapCollider.Rectangle.MaskType)EditorGUILayout.EnumPopup("Mask Type", script.rectangle.maskType);
				
				EditorGUI.BeginDisabledGroup(script.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.None);

				script.maskLayer = (LightingLayer)EditorGUILayout.Popup("Mask Layer (Light)", (int)script.maskLayer, Lighting2D.Profile.layers.lightLayers.GetNames());

				//if (script.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.BumpedSprite) {
				//	GUIBumpMapMode.Draw(script.bumpMapMode);
				//}

				EditorGUI.EndDisabledGroup();

			break;

			case MapType.UnityIsometric:
			

			break;


			case MapType.UnityHexagon:
			
			break;

			case MapType.SuperTilemapEditor:
				
			break;
		}

		EditorGUILayout.Space();

        UpdateCollisions(script);

		if (GUI.changed) {

            if (EditorApplication.isPlaying == false) {
				script.Initialize();
				
				EditorUtility.SetDirty(target);
            	EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
    }

    static void UpdateCollisions(DayLightingTilemapCollider2D script) {
		if (GUILayout.Button("Update")) {
			// CustomPhysicsShapeManager.Clear();
			
			script.Initialize();

			//LightingSource2D.ForceUpdateAll();
			LightingManager2D.ForceUpdate();
		}
	}
}


#endif