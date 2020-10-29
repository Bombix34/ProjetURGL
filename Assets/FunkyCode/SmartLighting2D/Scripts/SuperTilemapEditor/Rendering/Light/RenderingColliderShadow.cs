using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

#if (SUPER_TILEMAP_EDITOR)

    namespace SuperTilemapEditorSupport {
        
        public class RenderingColliderShadow { 

            static public void Grid(LightingSource2D light, LightingTilemapCollider2D id) {
                if (id.mapType != MapType.SuperTilemapEditor) {
                    return;
                }

                if (id.superTilemapEditor.shadowType != SuperTilemapEditorSupport.TilemapCollider2D.shadowType.Grid) {
                    return;
                }

                Vector2 lightPosition = -light.transform.position;
                LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();

                foreach(LightingTile tile in id.superTilemapEditor.mapTiles) {
                    List<Polygon2D> polygons = tile.GetWorldPolygons(tilemapCollider);
                    Vector2 tilePosition = tile.GetWorldPosition();

                    if (tile.NotInRange(tilePosition + lightPosition, light.size)) {
                        continue;
                    }

                    Rendering.Light.ShadowEngine.Draw(polygons, 0);
                }
            }

             static public void Collider(LightingSource2D light, LightingTilemapCollider2D id) {
                if (id.mapType != MapType.SuperTilemapEditor) {
                    return;
                }

                if (id.superTilemapEditor.shadowType != SuperTilemapEditorSupport.TilemapCollider2D.shadowType.Collider) {
                    return;
                }

                Rendering.Light.ShadowEngine.Draw(id.superTilemapEditor.colliders, 0);
            }
        }
    }

#else 

    namespace SuperTilemapEditorSupport {
        public class RenderingColliderShadow { 
            static public void Grid(LightingSource2D light, LightingTilemapCollider2D id) {}
            static public void Collider(LightingSource2D light, LightingTilemapCollider2D id) {}
        }
    }

#endif