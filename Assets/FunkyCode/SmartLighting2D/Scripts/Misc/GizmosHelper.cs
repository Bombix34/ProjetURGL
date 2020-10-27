using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosHelper {

	static public void DrawRect(Vector3 position, Rect rect) {
		Vector3 p0 = new Vector3(rect.x, rect.y, position.z);
		Vector3 p1 = new Vector3(rect.x + rect.width, rect.y, position.z);
		Vector3 p2 = new Vector3(rect.x + rect.width, rect.y + rect.height, position.z);
		Vector3 p3 = new Vector3(rect.x, rect.y + rect.height, position.z);

        Gizmos.DrawLine(p0, p1);
		Gizmos.DrawLine(p1, p2);
		Gizmos.DrawLine(p2, p3);
		Gizmos.DrawLine(p3, p0);
    }

    static public void DrawCircle(Vector3 position, float rotation, float angle, float size) {
        Vector3 center = position;
		int step = 10;

		int start = -(int)(angle / 2);
		int end = (int)(angle / 2);

		for(int i = start; i < end; i += step) {
			float rot = i + 90 + rotation;

			Vector3 pointA = center;
			float rotA = rot * Mathf.Deg2Rad;
			pointA.x += Mathf.Cos(rotA) * size;
			pointA.y += Mathf.Sin(rotA) * size;


			Vector3 pointB = center;
			float rotB = (rot + step) * Mathf.Deg2Rad;
			pointB.x += Mathf.Cos(rotB) * size;
			pointB.y += Mathf.Sin(rotB) * size;

			Gizmos.DrawLine(pointA, pointB);

			if (angle < 360 && angle > 0) {
				if (i == start) {
					Gizmos.DrawLine(pointA, center);
				}

				if (i + step > end) {
					Gizmos.DrawLine(pointB, center);
				}
			}
		}
    }

    static public void DrawPolygons(List<Polygon2D> polygons, Vector3 position) {
        foreach(Polygon2D polygon in polygons) {
            DrawPolygon(polygon, position);
        }
    }

	static public void DrawPolygon(Polygon2D polygon, Vector3 position) {
		for(int i = 0; i < polygon.pointsList.Count; i++) {
			Vector2D p0 = polygon.pointsList[i];
			Vector2D p1 = polygon.pointsList[(i + 1) % polygon.pointsList.Count];

			Vector3 a = new Vector3((float)p0.x, (float)p0.y, position.z);
			Vector3 b = new Vector3((float)p1.x, (float)p1.y, position.z);

			Gizmos.DrawLine(a, b);
		}
    }


	static public void DrawSpriteBounds(VirtualSpriteRenderer virtualSpriteRenderer, Transform transform, LightingSpriteRenderer2D.TransformOffset transformOffset) {
		Vector2 position = transform.position;
		position += transformOffset.offsetPosition;

		Vector2 scale = transform.localScale;
		scale *= transformOffset.offsetScale;

		float rotation = 0;

		if (transformOffset.applyTransformRotation) {
			rotation += transform.eulerAngles.z + transformOffset.offsetRotation;
		}

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

		DrawPolygon(polygon, transform.position); 
    }

}