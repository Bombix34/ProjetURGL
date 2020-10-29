using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FogOfWar {
	public static class NoSort {
		public static void Draw(Camera camera) {
			Material material = null;

			Vector2 objectPosition = new Vector2();

			foreach(FogOfWarSprite sprite in FogOfWarSprite.GetList()) {
				objectPosition.x = sprite.transform.position.x - camera.transform.position.x;
				objectPosition.y = sprite.transform.position.y - camera.transform.position.y;

				SpriteRenderer spriteRenderer = sprite.GetSpriteRenderer();

				if (spriteRenderer == null || sprite.GetSprite() == null) {
					continue;
				}

				material = spriteRenderer.sharedMaterial;
				material.mainTexture = sprite.GetSprite().texture;

				material.color = spriteRenderer.color;

				Rendering.Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(sprite.spriteMeshObject, material, sprite.GetSpriteRenderer(), objectPosition, sprite.transform.lossyScale, sprite.transform.rotation.eulerAngles.z, 0);			
			
				material.color = Color.white;
			}
		}
	}
}