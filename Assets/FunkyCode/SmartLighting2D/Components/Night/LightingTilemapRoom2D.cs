using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using LightingTilemapCollider;

 //ITilemap.GetSprite(Vector3)
 
#if UNITY_2017_4_OR_NEWER

    using UnityEngine.Tilemaps;

    [ExecuteInEditMode]
    public class LightingTilemapRoom2D : MonoBehaviour {
        public LightingLayer nightLayer = LightingLayer.Layer1;
        public enum MaskType {Sprite}  // Separate For Each Map Type!
        public enum ShaderType {ColorMask, MultiplyTexture};
    
        public MapType mapType = MapType.UnityRectangle;
        public MaskType maskType = MaskType.Sprite;
        public ShaderType shaderType = ShaderType.ColorMask;
        public Color color = Color.black;

        public SuperTilemapEditorSupport.TilemapRoom2D superTilemapEditor = new SuperTilemapEditorSupport.TilemapRoom2D();
        public Rectangle rectangle = new Rectangle();

        public LighitngTilemapRoomTransform lightingTransform = new LighitngTilemapRoomTransform();
	

        public static List<LightingTilemapRoom2D> list = new List<LightingTilemapRoom2D>();

        static public List<LightingTilemapRoom2D> GetList() {
            return(list);
        }

        public void OnEnable() {
            list.Add(this);

            LightingManager2D.Get();

			rectangle.SetGameObject(gameObject);
            superTilemapEditor.SetGameObject(gameObject);

            Initialize();
        }

        public void OnDisable() {
            list.Remove(this);
        }

        public LightingTilemapCollider.Base GetCurrentTilemap() {
			switch(mapType) {
				case MapType.SuperTilemapEditor:
					return(superTilemapEditor);
				case MapType.UnityRectangle:
					return(rectangle);

			}
			return(null);
		}

        public void Initialize() {
			TilemapEvents.Initialize();
			
            GetCurrentTilemap().Initialize();
        }


        public void Update() {
			lightingTransform.Update(this);

			if (lightingTransform.UpdateNeeded) {

                GetCurrentTilemap().ResetWorld();

				LightingSource2D.ForceUpdateAll();
			}
		}

        public TilemapProperties GetTilemapProperties() {
			return(GetCurrentTilemap().Properties);
		}

        public List<LightingTile> GetTileList() {
			return(GetCurrentTilemap().mapTiles);
		}

        public float GetRadius() {
			return(GetCurrentTilemap().GetRadius());
		}

        void OnDrawGizmosSelected() {
			if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Selected) {
				return;
			}

			DrawGizmos();
		}

		private void OnDrawGizmos() {
			if (Lighting2D.ProjectSettings.sceneView.drawGizmos != LightingSettings.EditorView.DrawGizmos.Always) {
				return;
			}

			DrawGizmos();
		}

		private void DrawGizmos() {
			if (isActiveAndEnabled == false) {
				return;
			}

			Gizmos.color = new Color(1f, 0.5f, 0.25f);

            switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
				case LightingSettings.EditorView.GizmosBounds.Rectangle:
					GizmosHelper.DrawRect(transform.position, GetCurrentTilemap().GetRect());
				break;

				case LightingSettings.EditorView.GizmosBounds.Radius:
					float radius = GetRadius();
					GizmosHelper.DrawCircle(transform.position, 0, 360, radius);
				break;
			}
		}
    }

#endif