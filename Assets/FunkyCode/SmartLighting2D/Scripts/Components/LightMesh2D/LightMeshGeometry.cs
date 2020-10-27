using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LightMeshGeometry {
    public List<Polygon2D> worldPolygons = new List<Polygon2D>();
    public PointList rayCastPoints = new PointList();
    public List<Vector2> collisionPoints = new List<Vector2>();

    public Vector2[] optimizedPoints;
    public int optimizedPointsCount = 0;

    LightMesh2D light;

    public void Initialize(LightMesh2D light) {
        optimizedPoints = new Vector2[10000];

        this.light = light;
    }

    public void Update() {
        UpdateWorldPolygons();

        UpdateRayCastPoints();

        UpdateCollisionPoints();

        UpdateOptimizedPoints();
    }

    void UpdateWorldPolygons() {
        List<LightingCollider2D> colliders = LightingCollider2D.GetCollisionList((int)light.lightLayer);

        worldPolygons.Clear();

        foreach(LightingCollider2D lightingCollider2D in colliders) {
            if (Vector2.Distance(lightingCollider2D.mainShape.transform2D.position, light.transform.position) > lightingCollider2D.mainShape.GetRadiusWorld() + light.size) {
               continue;
            }

            light.collidersInside.Add(lightingCollider2D);

            List<Polygon2D> polygons = lightingCollider2D.mainShape.GetPolygonsWorld();

            foreach(Polygon2D polygon in polygons) {
                worldPolygons.Add(polygon);
            }
        }

        foreach(LightingTilemapCollider2D tilemap in LightingTilemapCollider2D.GetList()) {
            //if (tilemap.IsNotInRange()) {

            //}

            List<LightingTile> tiles = tilemap.GetTileList();

            foreach(LightingTile tile in tiles) {
                Vector2 position = tile.GetWorldPosition() - light.transform2D.position;

                if (tile.NotInRange(position, light.size)) {
                    continue;
                }

                LightingTilemapCollider.Base tilemapCollider = tilemap.GetCurrentTilemap();

                foreach(Polygon2D polygon in tile.GetWorldPolygons(tilemapCollider)) {
                    worldPolygons.Add(polygon);
                } 
            }

            
        }
    }

    void UpdateRayCastPoints() {
        rayCastPoints.Reset();

        Vector2 lightPosition = light.transform.position;

        float nearByOffset = 0.001f;

        foreach(Polygon2D poly in worldPolygons) {
            foreach(Vector2D p in poly.pointsList) {
                Vector2 worldPoint = p.ToVector2();

                float rotation = (worldPoint.Atan2(lightPosition) * Mathf.Rad2Deg + 720) % 360;
                rayCastPoints.Add(worldPoint, rotation, true);

                float rotationLeft = rotation - nearByOffset;
                Vector2 left = new Vector2();
                left.x = lightPosition.x + Mathf.Cos(rotationLeft * Mathf.Deg2Rad) * light.size;
                left.y = lightPosition.y + Mathf.Sin(rotationLeft * Mathf.Deg2Rad) * light.size;

                rayCastPoints.Add(left, rotationLeft, false);

                float rotationRight = rotation + nearByOffset;
                Vector2 right = new Vector2();
                right.x = lightPosition.x + Mathf.Cos(rotationRight * Mathf.Deg2Rad) * light.size;
                right.y = lightPosition.y + Mathf.Sin(rotationRight * Mathf.Deg2Rad) * light.size;

                rayCastPoints.Add(right, rotationRight, false);
            }
        }

        rayCastPoints.Sort();
    }

    void UpdateCollisionPoints() {
        collisionPoints.Clear();

        for(int i = 0; i < rayCastPoints.count; i++) {
            Point point = rayCastPoints.list[i];
            Vector2? intersection = GetClosestIntersection(point.value);

            if (intersection != null) {
                collisionPoints.Add(intersection.Value);
            } else { // if (point.geometry == false) 
                collisionPoints.Add(point.value);
                
            }

        }
    }

    void UpdateOptimizedPoints() {
        optimizedPointsCount = 0;


        float verticeStep = (360f / light.segments);
        Vector2 lightPosition = light.transform.position;

        if (collisionPoints.Count < 1) {
          
            for(float x = 0; x < 360; x += verticeStep) {
                float rot = x * Mathf.Deg2Rad;

                Vector2 v = new Vector2();
                v.x = lightPosition.x + Mathf.Cos(-rot) * light.size;
                v.y = lightPosition.y + Mathf.Sin(-rot) * light.size;

                optimizedPoints[optimizedPointsCount] = v;  optimizedPointsCount ++;
                
            }
            return;
        }

        float lightSizePrecision = 0.1f;

        Vector2 oldPoint = collisionPoints.Last();
        float oldRotation = collisionPoints.First().Atan2(oldPoint) * Mathf.Rad2Deg;
        bool oldIsEdgePoint = Vector2.Distance(oldPoint, lightPosition ) >= light.size - lightSizePrecision;

        bool edgePoint;

        int pointCount = collisionPoints.Count;
       
        for(int i = 0; i < pointCount + 1; i++) {
            Vector2 point = collisionPoints[(i - 1 + pointCount) % pointCount];
            float rotation = point.Atan2(oldPoint) * Mathf.Rad2Deg;
            rotation = (rotation + 720) % 360;
            edgePoint = Vector2.Distance(point, lightPosition ) >= light.size - lightSizePrecision;
        
            if (Mathf.Abs(rotation - oldRotation) > 0.1f) {
              
                optimizedPoints[optimizedPointsCount] = oldPoint;           optimizedPointsCount ++;
                
                if (oldIsEdgePoint && edgePoint) {
                    float degre = (point.Atan2(lightPosition) * Mathf.Rad2Deg + 720) % 360;
                    float oldDegre = (oldPoint.Atan2(lightPosition) * Mathf.Rad2Deg + 720) % 360;

                    if (degre > oldDegre) {
                        degre -= 360;
                    }

                    for(float x = oldDegre; x > degre; x -= verticeStep) {
                        float middleDegree = x;

                        float rot = (middleDegree ) * Mathf.Deg2Rad;

                        Vector2 v = new Vector2();
                        v.x = lightPosition.x + Mathf.Cos(rot) * light.size;
                        v.y = lightPosition.y + Mathf.Sin(rot) * light.size;

                        optimizedPoints[optimizedPointsCount] = v;          optimizedPointsCount ++;
                    }

                }

                oldRotation = rotation;
                oldPoint = point;
                oldIsEdgePoint = edgePoint;
            } else {
                oldRotation = rotation;
                oldPoint = point;
                oldIsEdgePoint = edgePoint;
            }
        }
    }





    Vector2? GetClosestIntersection(Vector2 point) {
        Vector2 lightPosition = light.transform.position;

        Vector2? minPoint = null;
        float minDistance =  Vector2.Distance(point, lightPosition);

        Vector2 pointA, pointB;

        int polygonCount = worldPolygons.Count;

        for(int x = 0; x < polygonCount; x++) {
            Polygon2D poly = worldPolygons[x];
  
            int pointCount = poly.pointsList.Count;

            for(int i = 0; i < pointCount; i++) {
                Vector2D pA = poly.pointsList[i];
                Vector2D pB = poly.pointsList[(i + 1) % pointCount];

                pointA.x = (float)pA.x;
                pointA.y = (float)pA.y;

                pointB.x = (float)pB.x;
                pointB.y = (float)pB.y;
                
                Vector2? intersection = Math2D.GetPointLineIntersectLine3(lightPosition, point, pointA, pointB);

                if (intersection != null) {
                    float dist = Vector2.Distance(lightPosition, intersection.Value);

                    if (dist < minDistance) {
                        minDistance = dist;

                        minPoint = intersection;
                    }
                }
            }
        }

        return(minPoint);
    }




    
    public struct Point : System.Collections.Generic.IComparer<Point>{
        public Vector2 value;
        public bool geometry;
        public float rotation;

        public int Compare(Point a, Point b) {
			Point c1 = (Point)a;
			Point c2 = (Point)b;

			if (c1.rotation < c2.rotation) {
				return 1;
			}
		
			if (c1.rotation > c2.rotation) {
				return -1;
			} else {
				return 0;
			}
		}

		public static System.Collections.Generic.IComparer<Point> Sort() {      
			return (System.Collections.Generic.IComparer<Point>) new Point();
		}
    }

	public class PointList {
		public Point[] list = new Point[4024];

		public int count = 0;
		public PointList() {
			for(int i = 0; i < list.Length; i++) {
				list[i] = new Point();
			}
		}

		public void Add(Vector2 p, float rotation, bool geometry) {
			if (count + 1 < list.Length) {
				list[count].value = p;
				list[count].rotation = rotation;
                list[count].geometry = geometry;
				count++;
			} else {
				Debug.LogError("Collider Depth Overhead!");
			}
		}

		public void Reset() {
			count = 0;
		}

		public void Sort() {
			Array.Sort<Point>(list, 0, count, Point.Sort());
		}
	}

}
