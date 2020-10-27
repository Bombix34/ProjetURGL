using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightingShape {
		
	public class Base {
		public List<Polygon2D> polygons_world = null;
		public List<Polygon2D> polygons_world_cache = null;

		public List<Polygon2D> polygons_local = null;

		public List<MeshObject> meshes = null;
		public float localRadius = -1f;

		public Rect worldRect;

		public Transform transform;

		public void SetTransform(Transform transform) {
			this.transform = transform;
		}

		virtual public List<Polygon2D> GetPolygonsLocal() {
			return(polygons_local);
		}

		virtual public List<Polygon2D> GetPolygonsWorld() {
			return(polygons_world);
		}

		virtual public void ResetLocal() {
			localRadius = -1f;
			meshes = null;

			polygons_local = null;

			polygons_world = null;
			polygons_world_cache = null;

			ResetWorld();
		}

		virtual public void ResetWorld() {
			polygons_world = null;

			worldRect = new Rect();
		}

		public float GetRadius() {
			if (localRadius < 0) {

				localRadius = 0;

				List<Polygon2D> polygons = GetPolygonsLocal();

				if (polygons.Count > 0) {
					foreach(Polygon2D poly in polygons) {
						foreach (Vector2D id in poly.pointsList) {
							localRadius = Mathf.Max(localRadius, Vector2.Distance(id.ToVector2(), Vector2.zero));
						}
					}
				}
			}
			return(localRadius);
		}

		public Rect GetWorldRect() {
			if (worldRect.width < 0.01f) {
				worldRect = Polygon2DList.GetRect(GetPolygonsWorld());
			}

			return(worldRect);
		}
	}
}