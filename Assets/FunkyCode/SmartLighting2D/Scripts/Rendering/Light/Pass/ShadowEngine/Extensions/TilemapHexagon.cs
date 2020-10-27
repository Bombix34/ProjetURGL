using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.Shadow {

    public class TilemapHexagon {
        
        static public void Draw(LightingSource2D light, LightingTilemapCollider2D id) {
            if (id.mapType != MapType.UnityHexagon) {
                return;
            }

            if (id.hexagon.colliderType == LightingTilemapCollider.Hexagon.ColliderType.None) {
                return;
            }

            Vector2 lightPosition = -light.transform.position;
            LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();
 
            foreach(LightingTile tile in id.hexagon.mapTiles) {
                List<Polygon2D> polygons = tile.GetWorldPolygons(tilemapCollider);
                Vector2 tilePosition = tile.GetWorldPosition();

                if (tile.NotInRange(tilePosition + lightPosition, light.size)) {
					continue;
				}

                ShadowEngine.Draw(polygons, 0);
            }
        }
    }
}