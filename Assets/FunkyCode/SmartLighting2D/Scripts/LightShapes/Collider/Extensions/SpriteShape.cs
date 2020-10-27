using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingShape;

namespace LightingShape {
		
	public class SpriteShape : Base {

		private Sprite originalSprite;
		private Sprite atlasSprite;

		private SpriteRenderer spriteRenderer;

		VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();

		private Polygon2D rectPolygon;
		private Polygon2D GetRectPolygon() {
			if (rectPolygon == null) {
				rectPolygon = new Polygon2D();
				rectPolygon.AddPoint(0, 0);
				rectPolygon.AddPoint(0, 0);
				rectPolygon.AddPoint(0, 0);
				rectPolygon.AddPoint(0, 0);
			}

        	return(rectPolygon);
		}

		public override List<Polygon2D> GetPolygonsLocal() {

			if (polygons_local == null) {
				polygons_local = new List<Polygon2D>();

				if (spriteRenderer == null) {
					return(polygons_local);
				}

				if (spriteRenderer.drawMode == SpriteDrawMode.Tiled && spriteRenderer.tileMode == SpriteTileMode.Continuous) {

					float rot = transform.eulerAngles.z;
					Vector2 size = transform.localScale * spriteRenderer.size * 0.5f;
					Vector2 pos = Vector3.zero;

					rot = rot * Mathf.Deg2Rad + Mathf.PI;

					float rectAngle = Mathf.Atan2(size.y, size.x);
					float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

					Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
					Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
					Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
					Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
				
					Polygon2D polygon = new Polygon2D();
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);

					polygon.pointsList[0].x = v1.x;
					polygon.pointsList[0].y = v1.y;

					polygon.pointsList[1].x = v2.x;
					polygon.pointsList[1].y = v2.y;

					polygon.pointsList[2].x = v3.x;
					polygon.pointsList[2].y = v3.y;

					polygon.pointsList[3].x = v4.x;
					polygon.pointsList[3].y = v4.y;

					polygons_local.Add(polygon);

				} else {

					virtualSpriteRenderer.Set(spriteRenderer);

					Vector2 position = Vector3.zero;
					Vector2 scale = transform.localScale;
					float rotation = transform.eulerAngles.z;
		
					SpriteTransform spriteTransform = new SpriteTransform(virtualSpriteRenderer, position, scale, rotation);

					float rot = spriteTransform.rotation;
					Vector2 size = spriteTransform.scale;
					Vector2 pos = spriteTransform.position;

					rot = rot * Mathf.Deg2Rad + Mathf.PI;

					float rectAngle = Mathf.Atan2(size.y, size.x);
					float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

					Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
					Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
					Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
					Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
				
					Polygon2D polygon = new Polygon2D();
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);
					polygon.AddPoint(0, 0);
					
					polygon.pointsList[0].x = v1.x;
					polygon.pointsList[0].y = v1.y;

					polygon.pointsList[1].x = v2.x;
					polygon.pointsList[1].y = v2.y;

					polygon.pointsList[2].x = v3.x;
					polygon.pointsList[2].y = v3.y;

					polygon.pointsList[3].x = v4.x;
					polygon.pointsList[3].y = v4.y;

					polygons_local.Add(polygon);
				}


			}

			return(polygons_local);
		}

		public override List<Polygon2D> GetPolygonsWorld() {
			if (polygons_world == null) {

				if (polygons_world_cache == null) {

					

					polygons_world = new List<Polygon2D>();

					if (spriteRenderer.drawMode == SpriteDrawMode.Tiled && spriteRenderer.tileMode == SpriteTileMode.Continuous) {

						float rot = transform.eulerAngles.z;
						Vector2 size = transform.lossyScale * spriteRenderer.size * 0.5f;
						Vector2 pos = transform.position;

						rot = rot * Mathf.Deg2Rad + Mathf.PI;

						float rectAngle = Mathf.Atan2(size.y, size.x);
						float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

						Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
						Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
						Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
						Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
					
						Polygon2D polygon = new Polygon2D();
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);

						polygon.pointsList[0].x = v1.x;
						polygon.pointsList[0].y = v1.y;

						polygon.pointsList[1].x = v2.x;
						polygon.pointsList[1].y = v2.y;

						polygon.pointsList[2].x = v3.x;
						polygon.pointsList[2].y = v3.y;

						polygon.pointsList[3].x = v4.x;
						polygon.pointsList[3].y = v4.y;

						polygons_world.Add(polygon);

					} else {

						virtualSpriteRenderer.Set(spriteRenderer);

						Vector2 position = transform.position;
						Vector2 scale = transform.lossyScale;
						float rotation = transform.eulerAngles.z;
			
						SpriteTransform spriteTransform = new SpriteTransform(virtualSpriteRenderer, position, scale, rotation);

						float rot = spriteTransform.rotation;
						Vector2 size = spriteTransform.scale;
						Vector2 pos = spriteTransform.position;

						rot = rot * Mathf.Deg2Rad + Mathf.PI;

						float rectAngle = Mathf.Atan2(size.y, size.x);
						float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

						Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
						Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
						Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
						Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
					
						Polygon2D polygon = new Polygon2D();
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);
						polygon.AddPoint(0, 0);
						
						polygon.pointsList[0].x = v1.x;
						polygon.pointsList[0].y = v1.y;

						polygon.pointsList[1].x = v2.x;
						polygon.pointsList[1].y = v2.y;

						polygon.pointsList[2].x = v3.x;
						polygon.pointsList[2].y = v3.y;

						polygon.pointsList[3].x = v4.x;
						polygon.pointsList[3].y = v4.y;

						polygons_world.Add(polygon);

						polygons_world_cache = polygons_world;
					}
				} else {
					
					polygons_world = polygons_world_cache;



					if (spriteRenderer.drawMode == SpriteDrawMode.Tiled && spriteRenderer.tileMode == SpriteTileMode.Continuous) {

						float rot = transform.eulerAngles.z;
						Vector2 size = transform.lossyScale * spriteRenderer.size * 0.5f;
						Vector2 pos = transform.position;

						rot = rot * Mathf.Deg2Rad + Mathf.PI;

						float rectAngle = Mathf.Atan2(size.y, size.x);
						float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

						Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
						Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
						Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
						Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
					
						Polygon2D polygon = polygons_world[0];

						polygon.pointsList[0].x = v1.x;
						polygon.pointsList[0].y = v1.y;

						polygon.pointsList[1].x = v2.x;
						polygon.pointsList[1].y = v2.y;

						polygon.pointsList[2].x = v3.x;
						polygon.pointsList[2].y = v3.y;

						polygon.pointsList[3].x = v4.x;
						polygon.pointsList[3].y = v4.y;

					} else {
						
						virtualSpriteRenderer.Set(spriteRenderer);
						Vector2 position = transform.position;
						Vector2 scale = transform.lossyScale;
						float rotation = transform.eulerAngles.z;
			
						SpriteTransform spriteTransform = new SpriteTransform(virtualSpriteRenderer, position, scale, rotation);

						float rot = spriteTransform.rotation;
						Vector2 size = spriteTransform.scale;
						Vector2 pos = spriteTransform.position;

						rot = rot * Mathf.Deg2Rad + Mathf.PI;

						float rectAngle = Mathf.Atan2(size.y, size.x);
						float dist = Mathf.Sqrt(size.x * size.x + size.y * size.y);

						Vector2 v1 = new Vector2(pos.x + Mathf.Cos(rectAngle + rot) * dist, pos.y + Mathf.Sin(rectAngle + rot) * dist);
						Vector2 v2 = new Vector2(pos.x + Mathf.Cos(-rectAngle + rot) * dist, pos.y + Mathf.Sin(-rectAngle + rot) * dist);
						Vector2 v3 = new Vector2(pos.x + Mathf.Cos(rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(rectAngle + Mathf.PI + rot) * dist);
						Vector2 v4 = new Vector2(pos.x + Mathf.Cos(-rectAngle + Mathf.PI + rot) * dist, pos.y + Mathf.Sin(-rectAngle + Mathf.PI + rot) * dist);
					
						Polygon2D polygon = polygons_world[0];
						
						polygon.pointsList[0].x = v1.x;
						polygon.pointsList[0].y = v1.y;

						polygon.pointsList[1].x = v2.x;
						polygon.pointsList[1].y = v2.y;

						polygon.pointsList[2].x = v3.x;
						polygon.pointsList[2].y = v3.y;

						polygon.pointsList[3].x = v4.x;
						polygon.pointsList[3].y = v4.y;
					}







				}
			}

			return(polygons_world);
		}

		public override void ResetLocal() {
			base.ResetLocal();

			originalSprite = null;
			atlasSprite = null;
		}

		public SpriteRenderer GetSpriteRenderer() {
			if (spriteRenderer != null) {
				return(spriteRenderer);
			}
			
			if (transform == null) {
				return(spriteRenderer);
			}

			if (spriteRenderer == null) {
				spriteRenderer = transform.GetComponent<SpriteRenderer>();
			}
			
			return(spriteRenderer);
		}

		public Sprite GetOriginalSprite() {
            if (originalSprite == null) {
                GetSpriteRenderer();

                if (spriteRenderer != null) {
                    originalSprite = spriteRenderer.sprite;
                }
            }
			return(originalSprite);
		}

		public Sprite GetAtlasSprite() {
			return(atlasSprite);
		}

		public void SetAtlasSprite(Sprite sprite) {
			atlasSprite = sprite;
		}
	}
}