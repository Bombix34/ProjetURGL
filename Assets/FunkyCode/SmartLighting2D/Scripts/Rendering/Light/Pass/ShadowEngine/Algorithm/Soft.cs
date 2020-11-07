using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Rendering.Light.Shadow {

    public class Soft {
        
        public static Pair2D pair = Pair2D.Zero();

		static EdgePass pass = new EdgePass();


        public static void Draw(List<Polygon2D> polygons) {
			Vector2 position = ShadowEngine.lightOffset;

			
			//float lightDirection = Mathf.Atan2((pa_y + pb_y) / 2 , (pa_x + pb_x) / 2 ) * Mathf.Rad2Deg;
			//float EdgeDirection = Mathf.Atan2(p_a_y - p_b_y, p_a_x - p_b_x) * Mathf.Rad2Deg - 180;

			//lightDirection -= EdgeDirection;

			//lightDirection = (lightDirection + 720) % 360;
			//if (lightDirection < 180 ) {
			//	continue;
			//}

			// Select Soft Mode - Vertex/Object
	
			for(int i = 0; i < polygons.Count; i++) {

				List<Vector2D> pointsList = polygons[i].pointsList;
				int pointsCount = pointsList.Count;

				if (ShadowEngine.softShadowObjects) {
					ShadowForObject(polygons[i], position);
				} else {
					ShadowForVertex(polygons[i], position);
				}
				

				

			}
		}

		static void ShadowForVertex(Polygon2D polygon, Vector2 position) {
			List<Vector2D> pointsList = polygon.pointsList;
			int pointsCount = pointsList.Count;

			for(int x = 0; x < pointsCount; x++) {
				pair.A.x = pointsList[(x) % pointsCount].x + position.x;
				pair.A.y = pointsList[(x) % pointsCount].y + position.y;

				pair.B.x = pointsList[(x + 2) % pointsCount].x + position.x;
				pair.B.y = pointsList[(x + 2) % pointsCount].y + position.y;

				Pair2D edge_world = pair;

				Vector2 edgePosition;
				edgePosition.x = (float)(pair.A.x + pair.B.x) / 2;
				edgePosition.y = (float)(pair.A.y + pair.B.y) / 2;

				float edgeRotation = (float)Math.Atan2(pair.B.y - pair.A.y, pair.B.x - pair.A.x);

				float edgeSize = (float)Vector2D.Distance(pair.A, pair.B) / 2;

				pass.edgePosition = edgePosition;
				pass.edgeRotation = edgeRotation;
				pass.edgeSize = edgeSize;
				pass.coreSize = ShadowEngine.light.coreSize;

				pass.Generate();
				pass.SetVars();
				pass.Draw();
			}
		}

		static void ShadowForObject(Polygon2D polygon, Vector2 position) {
			List<Vector2D> pointsList = polygon.pointsList;
			int pointsCount = pointsList.Count;

			LightingSource2D light = ShadowEngine.light;

			SoftShadowSorter.Set(polygon, light);

			pair.A.x = SoftShadowSorter.minPoint.x + position.x;
			pair.A.y = SoftShadowSorter.minPoint.y + position.y;

			pair.B.x = SoftShadowSorter.maxPoint.x + position.x;
			pair.B.y = SoftShadowSorter.maxPoint.y + position.y;

			Pair2D edge_world = pair;

			Vector2 edgePosition;
			edgePosition.x = (float)(pair.A.x + pair.B.x) / 2;
			edgePosition.y = (float)(pair.A.y + pair.B.y) / 2;

			float edgeRotation = (float)Math.Atan2(pair.B.y - pair.A.y, pair.B.x - pair.A.x);

			float edgeSize = (float)Vector2D.Distance(pair.A, pair.B) / 2;

			pass.edgePosition = edgePosition;
			pass.edgeRotation = edgeRotation;
			pass.edgeSize = edgeSize;
			pass.coreSize = light.coreSize;

			pass.Generate();
			pass.SetVars();
			pass.Draw();

			pass.edgePosition = edgePosition;
			pass.edgeRotation = edgeRotation + Mathf.PI;
			pass.edgeSize = edgeSize;
			pass.coreSize = light.coreSize;

			pass.Generate();
			pass.SetVars();
			pass.Draw();
		}
    }

	
}


public static class SoftShadowSorter {

	public static Polygon2D polygon;

	public static LightingSource2D light;

	public static Vector2 center;

	public static float[] direction = new float[1000];

	public static Vector2D minPoint;
	public static Vector2D maxPoint;

	public static void Set(Polygon2D poly, LightingSource2D light2D) {
		polygon = poly;
		
		light = light2D;

		Vector2 offset = -light.transform2D.position;

		center.x = 0;
		center.y = 0;

		foreach(Vector2D p in polygon.pointsList) {
			center.x += (float)p.x + offset.x;
			center.y += (float)p.y + offset.y;
		}

		center.x /= polygon.pointsList.Count;
		center.y /= polygon.pointsList.Count;

		float centerDirection = Mathf.Atan2(center.x, center.y) * Mathf.Rad2Deg;

		centerDirection = (centerDirection + 720) % 360 + 180;

		foreach(Vector2D p in polygon.pointsList) {
			int id = polygon.pointsList.IndexOf(p);

			direction[id] = Mathf.Atan2((float)p.x + offset.x, (float)p.y + offset.y) * Mathf.Rad2Deg;



		
			direction[id] = (direction[id] + 720 - centerDirection) % 360;

			
		}


		float min = 10000;
		float max = -10000;
	
		foreach(Vector2D p in polygon.pointsList) {
			int id = polygon.pointsList.IndexOf(p);

			if (direction[id] < min) {
				min = direction[id];
				minPoint = p;
			}

			if (direction[id] > max) {
				max = direction[id];
				maxPoint = p;
			}
		}


				

	}
}