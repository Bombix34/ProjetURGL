using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithoutAtlas {

    public class TilemapIsometric {
		public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();
        
        static public void MaskSprite(LightingSource2D light, LightingTilemapCollider2D id, Material material, float z) {
			if (id.mapType != MapType.UnityIsometric) {
				return;
			}

			if (id.isometric.maskType != LightingTilemapCollider.Isometric.MaskType.Sprite) {
				return;
			}

			GL.Color(Color.white);
			
			Vector2 lightPosition = -light.transform.position;
			TilemapProperties properties = id.GetTilemapProperties();
			Vector2 scale = Tilemap.Isometric.GetScale(properties);

			foreach(LightingTile tile in id.isometric.mapTiles) {
				virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

				Vector2 tilePosition = Tilemap.Isometric.GetTilePosition(tile, properties);

				tilePosition += lightPosition;

				if (Vector2.Distance(Vector2.zero, tilePosition) > light.size) {
					continue;
				}

				material.mainTexture = virtualSpriteRenderer.sprite.texture;

				Rendering.Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, virtualSpriteRenderer, tilePosition, scale, 0, z);
				
				material.mainTexture = null;
			}
		}

		// Supports only static "tile" shape
		// No support for Custom Physics Shape
		static public void MaskShape(LightingSource2D light, LightingTilemapCollider2D id, float z) {
			if (id.mapType != MapType.UnityIsometric) {
				return;
			}

			if (id.isometric.maskType == LightingTilemapCollider.Isometric.MaskType.None) {
				return;
			}

			if (id.isometric.maskType == LightingTilemapCollider.Isometric.MaskType.Sprite) {
				return;
			}

			GL.Color(Color.white);

			Vector2 lightPosition = -light.transform.position;
			LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();
			TilemapProperties properties = id.GetTilemapProperties();
			Vector2 scale = Tilemap.Isometric.GetScale(properties);

			MeshObject tileMesh = LightingTile.Isometric.GetStaticTileMesh(id);


			foreach(LightingTile tile in id.isometric.mapTiles) {
				List<Polygon2D> polygons = tile.GetLocalPolygons(tilemapCollider);

				Vector2 tilePosition = Tilemap.Isometric.GetTilePosition(tile, properties);		
				tilePosition.y -= 0.25f;

				tilePosition += lightPosition;

				if (tile.NotInRange(tilePosition, light.size)) {
					continue;
				}

				GLExtended.DrawMesh(tileMesh, tilePosition, scale, 0);
			}
		}
    }
}