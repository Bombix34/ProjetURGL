using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.WithoutAtlas {

    public class Tile {
		public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();

       	static public void MaskSprite(LightingTile tile, LayerSetting layerSetting, Material material, Vector2 lightPosition, LightingTilemapCollider2D tilemap, float lightSizeSquared, float z) {
			virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

			if (virtualSpriteRenderer.sprite == null) {
				return;
			}

			Vector2 tilePosition = tile.GetWorldPosition() + lightPosition;

			material.color = LayerSettingColor.Get(tilePosition, layerSetting, MaskEffect.Lit);

			material.mainTexture = virtualSpriteRenderer.sprite.texture;

			Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, virtualSpriteRenderer, tilePosition, tile.worldScale, tile.worldRotation, z);
			
			material.mainTexture = null;
		}
    }
}