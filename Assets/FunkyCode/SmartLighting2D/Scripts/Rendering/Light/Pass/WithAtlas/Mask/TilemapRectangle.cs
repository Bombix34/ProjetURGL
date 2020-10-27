using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithAtlas {

    public class TilemapRectangle {

		public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();

        static public void Sprite(LightingSource2D light, LightingTilemapCollider2D id, float z) {
			if (id.mapType != MapType.UnityRectangle) {
				return;
			}

			if (id.rectangle.maskType != LightingTilemapCollider.Rectangle.MaskType.Sprite) {
				return;
			}
			
			Vector2 lightPosition = -light.transform.position;
			TilemapProperties properties = id.GetTilemapProperties();

            foreach(LightingTile tile in id.rectangle.mapTiles) {

				if (tile.GetOriginalSprite() == null) {
					continue;
				}

				Vector2 tilePosition = Tilemap.Rectangle.GetTilePosition(tile, properties);

				tilePosition += lightPosition;

				if (tile.NotInRange(tilePosition, light.size)) {
					continue;
				}
				
				virtualSpriteRenderer.sprite = tile.GetAtlasSprite();

				if (virtualSpriteRenderer.sprite == null) {
					Sprite reqSprite = AtlasSystem.Manager.RequestSprite(tile.GetOriginalSprite(), AtlasSystem.Request.Type.WhiteMask);
					if (reqSprite == null) {
						// Add Partialy Batched
						PartiallyBatchedTilemap batched = new PartiallyBatchedTilemap();

						batched.virtualSpriteRenderer = new VirtualSpriteRenderer();
						batched.virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

						batched.polyOffset = tilePosition;
						batched.tile = tile;

						batched.tileSize = id.transform.lossyScale;

						light.Buffer.lightingAtlasBatches.tilemapList.Add(batched);
						continue;
					} else {
						tile.SetAtlasSprite(reqSprite);
						virtualSpriteRenderer.sprite = reqSprite;
					}
				}

				LayerSetting[] layerSettings = light.GetLayerSettings();

				Rendering.Universal.WithAtlas.Sprite.Draw(virtualSpriteRenderer, layerSettings[0], MaskEffect.Lit, tilePosition, id.transform.lossyScale, 0, z);
			}	

		}
    }
}
