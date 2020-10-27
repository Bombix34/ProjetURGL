using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.Shadow {

    public class Tile {
        
        static public void Draw(LightingSource2D light, LightingTile tile, LightingTilemapCollider2D tilemap) {
            LightingTilemapCollider.Base tilemapCollider = tilemap.GetCurrentTilemap();

            List<Polygon2D> polygons = tile.GetWorldPolygons(tilemapCollider);

            ShadowEngine.Draw(polygons, 0);
        }  
    }
}