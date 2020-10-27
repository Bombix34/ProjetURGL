using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.WithAtlas {

    public class Tile {
		public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();
        
		static public void MaskSprite(LightingSource2D light, LightingTile tile, LayerSetting layerSetting, LightingTilemapCollider2D id, Vector2 lightPosition, float z) {
			if (id.rectangle.maskType != LightingTilemapCollider.Rectangle.MaskType.Sprite) {
				return;
			}

			if (id.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.None) {
				return;
			}

			if (tile.GetOriginalSprite() == null) {
				return;
			}

			UnityEngine.Sprite sprite = tile.GetAtlasSprite();

			Vector2 tilePosition = tile.GetWorldPosition() + lightPosition;

			if (sprite == null) {
				UnityEngine.Sprite reqSprite = AtlasSystem.Manager.RequestSprite(tile.GetOriginalSprite(), AtlasSystem.Request.Type.WhiteMask);
				if (reqSprite == null) {
					PartiallyBatchedTilemap batched = new PartiallyBatchedTilemap();

					batched.virtualSpriteRenderer = new VirtualSpriteRenderer();
					batched.virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

					batched.polyOffset = tilePosition;

					batched.tileSize = id.transform.lossyScale;

					batched.tilemap = id;

					light.Buffer.lightingAtlasBatches.tilemapList.Add(batched);
					return;
				} else {
					tile.SetAtlasSprite(reqSprite);
					sprite = reqSprite;
				}
			}

			virtualSpriteRenderer.sprite = sprite;

			Rendering.Universal.WithAtlas.Sprite.Draw(virtualSpriteRenderer, layerSetting, MaskEffect.Lit, tilePosition, tile.worldScale, tile.worldRotation, z);	
		}
    }
}