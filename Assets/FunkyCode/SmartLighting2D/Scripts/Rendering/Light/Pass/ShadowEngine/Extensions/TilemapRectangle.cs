using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.Shadow {

    public class TilemapRectangle {

        static public void Draw(LightingSource2D light, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
            Vector2 lightPosition = -light.transform.position;
            LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();

            foreach(LightingTile tile in id.rectangle.mapTiles) {
                switch(id.shadowTileType) {
                    case ShadowTileType.AllTiles:
                    break;

                    case ShadowTileType.ColliderOnly:
                        if (tile.colliderType == UnityEngine.Tilemaps.Tile.ColliderType.None) {
                            continue;
                        }
                    break;
                }

                List<Polygon2D> polygons = tile.GetWorldPolygons(tilemapCollider);
                Vector2 tilePosition = tile.GetWorldPosition();

                if (tile.NotInRange(lightPosition + tilePosition, light.size)) {
                    continue;
                }

                ShadowEngine.Draw(polygons, 0);
            }
        }
    }
}

/* if (x-1 > 0 && y-1 > 0 && x + 1 < properties.area.size.x && y + 1 < properties.area.size.y) {
    if (tilePosition.x > 0 && tilePosition.y > 0) {
        LightingTile tileA = id.rectangle.map.map[x - 1, y];
        LightingTile tileB = id.rectangle.map.map[x, y - 1];
        LightingTile tileC = id.rectangle.map.map[x - 1, y - 1];
        if (tileA != null && tileB != null && tileC != null) {
            continue;
        }
    } else if (tilePosition.x < 0 && tilePosition.y > 0) {
        LightingTile tileA = id.rectangle.map.map[x+1, y];
        LightingTile tileB = id.rectangle.map.map[x, y-1];
        LightingTile tileC = id.rectangle.map.map[x+1, y-1];
        if (tileA != null && tileB != null && tileC != null) {
            continue;
        }
    } else if (tilePosition.x > 0 && tilePosition.y < 0) {
        LightingTile tileA = id.rectangle.map.map[x-1, y];
        LightingTile tileB = id.rectangle.map.map[x, y+1];
        LightingTile tileC = id.rectangle.map.map[x-1, y+1];
        if (tileA != null && tileB != null && tileC != null) {
            continue;
        }
    } else if (tilePosition.x < 0 && tilePosition.y < 0) {
        LightingTile tileA = id.rectangle.map.map[x+1, y];
        LightingTile tileB = id.rectangle.map.map[x, y+1];
        LightingTile tileC = id.rectangle.map.map[x+1, y+1];
        if (tileA != null && tileB != null && tileC != null) {
            continue;
        }
    }
}*/