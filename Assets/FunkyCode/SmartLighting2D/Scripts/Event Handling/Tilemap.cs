using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventHandling {
    public class Tilemap2D : Base {

        static List<Vector2> removePointsColliding = new List<Vector2>();
        static List<LightCollision2D> removeCollisions = new List<LightCollision2D>();

        public static List<LightCollision2D> RemoveHiddenCollisions(List<LightCollision2D> collisions, LightingSource2D light) {
            float lightSizeSquared = Mathf.Sqrt(light.size * light.size + light.size * light.size);
            double rotLeft, rotRight;

            Polygon2D testPolygon = GetPolygon();
            Vector2 lightPosition = - light.transform.position;
            int next;
           
            foreach(LightingTilemapCollider2D id in LightingTilemapCollider2D.GetList()) {
                LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();
    
                foreach(LightingTile tile in id.GetTileList()) {
                        switch(id.colliderTileType) {
                            case ShadowTileType.AllTiles:
                            break;

                            case ShadowTileType.ColliderOnly:
                                if (tile.colliderType == UnityEngine.Tilemaps.Tile.ColliderType.None) {
                                    continue;
                                }
                            break;
                        }

                        List<Polygon2D> polygons = tile.GetWorldPolygons(tilemapCollider);

                        if (polygons.Count < 1) {
                            continue;
                        }

                        Vector2 tilePosition = tile.GetWorldPosition() + lightPosition;
    
                        if (tile.NotInRange(tilePosition, light.size)) {
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
                            
                                projectionLeft.x = edgeLeft.x + System.Math.Cos(rotLeft) * lightSizeSquared;
                                projectionLeft.y = edgeLeft.y + System.Math.Sin(rotLeft) * lightSizeSquared;

                                projectionRight.x = edgeRight.x + System.Math.Cos(rotRight) * lightSizeSquared;
                                projectionRight.y = edgeRight.y + System.Math.Sin(rotRight) * lightSizeSquared;

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
                       
            }
            
            return(collisions);
        }
    }
}


