using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventHandling {

    public class Collider : Base {

        static public void GetCollisions(List<LightCollision2D> collisions, LightingSource2D lightingSource) {
            List<LightingCollider2D> colliderList = LightingCollider2D.listEventReceivers;

            foreach (LightingCollider2D id in colliderList) { // Why all and not selected? + Specific layer
                if (id.usingEvents == false) {
                    continue;
                }
                
                if (id.mainShape.shadowType == LightingCollider2D.ShadowType.None) {
                    continue;
                }

                float radius = id.mainShape.GetRadiusWorld() + lightingSource.size;

                if (Vector2.Distance(id.transform.position, lightingSource.transform.position) > radius) {
                    continue;
                }

                List<Polygon2D> polygons = id.mainShape.GetPolygonsWorld();

                if (polygons.Count < 1) {
                    continue;
                }

                Polygon2 polygon = new Polygon2(polygons[0]);
                polygon.ToOffsetItself(-lightingSource.transform.position);

                LightCollision2D collision = new LightCollision2D();
                collision.light = lightingSource;
                collision.collider = id;
                collision.points = new List<Vector2>();
      
                foreach(Vector2 point in polygon.points) {
                    if (point.magnitude < lightingSource.size) {
                   
                        float direction = point.Atan2(Vector2.zero) * Mathf.Rad2Deg;

                        if (lightingSource.applyRotation) {
                            direction -= lightingSource.transform2D.rotation;
                        }

                        direction = (direction + 1080 - 90) % 360;

                        if (direction <= lightingSource.spotAngle / 2 || direction >= 360 - lightingSource.spotAngle / 2) {
                            collision.points.Add(point);
                        }
                    }
                }

                collisions.Add(collision);
            }
        }

        static List<Vector2> removePointsColliding = new List<Vector2>();
        static List<LightCollision2D> removeCollisions = new List<LightCollision2D>();
  
        public static List<LightCollision2D> RemoveHiddenCollisions(List<LightCollision2D> collisions, LightingSource2D light) {
            List<LightingCollider2D> colliderList = LightingCollider2D.GetList();
            
            float lightSize = Mathf.Sqrt(light.size * light.size + light.size * light.size);
            double rotLeft, rotRight;	
            
            Polygon2D testPolygon = GetPolygon();

            Vector2 lightPosition = - light.transform.position;
            int next;

            foreach (LightingCollider2D id in colliderList) {
                if (id.InLightSource(light) == false) {
                    continue;
                }

                if (id.mainShape.shadowType == LightingCollider2D.ShadowType.None) {
                    continue;
                }

                List<Polygon2D> polygons = id.mainShape.GetPolygonsWorld();

                if (polygons.Count < 1) {
                    continue;
                }
                
                removePointsColliding.Clear();
                removeCollisions.Clear();
                
                for(int i = 0; i < polygons.Count; i++) {

                    List<Vector2D> pointsList = polygons[i].pointsList;
                    int pointsCount = pointsList.Count;

                    for(int x = 0; x < pointsCount; x++) {
                        next = (x + 1) % pointsCount;

                        Vector2D left = pointsList[x];
                        Vector2D right = pointsList[next];

                        edgeLeft.x = left.x + lightPosition.x;
                        edgeLeft.y = left.y + lightPosition.y;

                        edgeRight.x = right.x + lightPosition.x;
                        edgeRight.y = right.y + lightPosition.y;

                        rotLeft = System.Math.Atan2 (edgeLeft.y, edgeLeft.x);
                        rotRight = System.Math.Atan2 (edgeRight.y, edgeRight.x);
                       
                        projectionLeft.x = edgeLeft.x + System.Math.Cos(rotLeft) * lightSize;
                        projectionLeft.y = edgeLeft.y + System.Math.Sin(rotLeft) * lightSize;

                        projectionRight.x = edgeRight.x + System.Math.Cos(rotRight) * lightSize;
                        projectionRight.y = edgeRight.y + System.Math.Sin(rotRight) * lightSize;

                        testPolygon.pointsList[0].x = projectionLeft.x;
                        testPolygon.pointsList[0].y = projectionLeft.y;

                        testPolygon.pointsList[1].x = projectionRight.x;
                        testPolygon.pointsList[1].y = projectionRight.y;

                        testPolygon.pointsList[2].x = edgeRight.x;
                        testPolygon.pointsList[2].y = edgeRight.y;

                        testPolygon.pointsList[3].x = edgeLeft.x;
                        testPolygon.pointsList[3].y = edgeLeft.y;

                        foreach(LightCollision2D col in collisions) {
                            if (col.collider == id) {
                                continue;
                            }

                            foreach(Vector2 point in col.points) {
                                if (testPolygon.PointInPoly(point)) {
                                    removePointsColliding.Add(point);
                                }
                            }

                            foreach(Vector2 point in removePointsColliding) {
                                col.points.Remove(point);
                            }

                            removePointsColliding.Clear();
                            
                            if (col.points.Count < 1) {
                                removeCollisions.Add(col);
                            }
                        }

                        foreach(LightCollision2D col in removeCollisions) {
                            collisions.Remove(col);
                        }

                        removeCollisions.Clear();
                    }
                }
            }

            return(collisions);
        }
    }
}