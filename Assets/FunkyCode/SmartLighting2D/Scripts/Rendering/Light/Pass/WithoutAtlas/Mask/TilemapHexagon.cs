using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithoutAtlas {

    public class TilemapHexagon {

        // Supports only static "tile" shape
		// No support for Custom Physics Shape
		static public void MaskShape(LightingSource2D light, LightingTilemapCollider2D id, float z) {
			if (id.mapType != MapType.UnityHexagon) {
				return;
			}

			if (id.hexagon.maskType == LightingTilemapCollider.Hexagon.MaskType.None) {
				return;
			}

			if (id.hexagon.maskType == LightingTilemapCollider.Hexagon.MaskType.Sprite) {
				return;
			}

			GL.Color(Color.white);

			Vector2 lightPosition = -light.transform.position;
			TilemapProperties properties = id.GetTilemapProperties();
			Vector2 scale = Tilemap.Hexagon.GetScale(properties);
			LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();
			
			MeshObject tileMesh = LightingTile.Hexagon.GetStaticTileMesh(id);
			
			foreach(LightingTile tile in id.hexagon.mapTiles) {
				List<Polygon2D> polygons = tile.GetLocalPolygons(tilemapCollider);

				Vector2 tilePosition = Tilemap.Hexagon.GetTilePosition(tile, properties);		
			
				tilePosition += lightPosition;

				if (tile.NotInRange(tilePosition, light.size)) {
					continue;
				}

				GLExtended.DrawMesh(tileMesh, tilePosition, scale, 0);
			}
		}
        
    }
}