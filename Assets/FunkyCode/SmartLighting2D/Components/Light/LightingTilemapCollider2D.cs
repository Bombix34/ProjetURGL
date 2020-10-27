using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using LightingSettings;
using UnityEngine.Tilemaps;
using System;
using LightingTilemapCollider;

#if UNITY_2017_4_OR_NEWER

	public enum ShadowTileType {AllTiles, ColliderOnly};

	[ExecuteInEditMode]
	public class LightingTilemapCollider2D : MonoBehaviour {
		public MapType mapType = MapType.UnityRectangle;

		public int shadowLayer = 0;
		public int maskLayer = 0;

		public ShadowTileType colliderTileType = ShadowTileType.AllTiles;

		public BumpMapMode bumpMapMode = new BumpMapMode();

		public Rectangle rectangle = new Rectangle();
		public Isometric isometric = new Isometric();
		public Hexagon hexagon = new Hexagon();

		public SuperTilemapEditorSupport.TilemapCollider2D superTilemapEditor = new SuperTilemapEditorSupport.TilemapCollider2D();

		public LightingTilemapTransform lightingTransform = new LightingTilemapTransform();

		public static List<LightingTilemapCollider2D> list = new List<LightingTilemapCollider2D>();

		public void OnEnable() {
			list.Add(this);

			LightingManager2D.Get();

			rectangle.SetGameObject(gameObject);
			isometric.SetGameObject(gameObject);
			hexagon.SetGameObject(gameObject);

			superTilemapEditor.eventsInit = false;
			superTilemapEditor.SetGameObject(gameObject);

			Initialize();

			LightingSource2D.ForceUpdateAll();
		}

		public void OnDisable() {
			list.Remove(this);

			LightingSource2D.ForceUpdateAll();
		}

		static public List<LightingTilemapCollider2D> GetList() {
			return(list);
		}

		public void Update() {
			lightingTransform.Update(this);

			if (lightingTransform.UpdateNeeded) {
				GetCurrentTilemap().ResetWorld();

				// Update if light is in range
				foreach(LightingSource2D light in LightingSource2D.GetList()) {
					if (IsInRange(light)) {
						light.ForceUpdate();
					}
				}
			}
		}

		public bool IsInRange(LightingSource2D light) {
			float radius = GetCurrentTilemap().GetRadius() + light.size;
			float distance = Vector2.Distance(light.transform.position, transform.position);

			return(distance < radius);
		}

		public bool IsNotInRange(LightingSource2D light) {
			float radius = GetCurrentTilemap().GetRadius() + light.size;
			float distance = Vector2.Distance(light.transform.position, transform.position);

			return(distance > radius);
		}

		public LightingTilemapCollider.Base GetCurrentTilemap() {
			switch(mapType) {
				case MapType.SuperTilemapEditor:
					return(superTilemapEditor);
				case MapType.UnityRectangle:
					return(rectangle);
				case MapType.UnityIsometric:
					return(isometric);
				case MapType.UnityHexagon:
					return(hexagon);
			}
			return(null);
		}

		public void Initialize() {
			TilemapEvents.Initialize();

			GetCurrentTilemap().Initialize();
		}

		public List<LightingTile> GetTileList() {
			return(GetCurrentTilemap().mapTiles);
		}

		public TilemapProperties GetTilemapProperties() {
			return(GetCurrentTilemap().Properties);
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

			LightingTilemapCollider.Base tilemap = GetCurrentTilemap();


			foreach(LightingTile tile in GetTileList()) {
				GizmosHelper.DrawPolygons(tile.GetWorldPolygons(tilemap), transform.position);
			}

			switch(Lighting2D.ProjectSettings.sceneView.drawGizmosBounds) {
				case LightingSettings.EditorView.GizmosBounds.Rectangle:
					GizmosHelper.DrawRect(transform.position, GetCurrentTilemap().GetRect());
				break;

				case LightingSettings.EditorView.GizmosBounds.Radius:
					float radius = GetCurrentTilemap().GetRadius();
					GizmosHelper.DrawCircle(transform.position, 0, 360, radius);
				break;
			}
		}
	}

#endif