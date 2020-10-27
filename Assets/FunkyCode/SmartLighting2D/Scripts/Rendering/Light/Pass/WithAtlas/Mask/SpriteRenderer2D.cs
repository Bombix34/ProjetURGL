using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.WithAtlas {

    public class SpriteRenderer2D {
		public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();

        public static void Mask(LightingSource2D light,LightingCollider2D id, float z) {
			if (id.InLightSource(light) == false) {
				return;
			}

            foreach(LightingColliderShape shape in id.shapes) {
				UnityEngine.SpriteRenderer spriteRenderer = shape.spriteShape.GetSpriteRenderer();

				if (shape.spriteShape.GetOriginalSprite() == null || spriteRenderer == null) {
					continue;
				}

				Sprite sprite = shape.spriteShape.GetAtlasSprite();
				if (sprite == null) {
					Sprite reqSprite = AtlasSystem.Manager.RequestSprite(shape.spriteShape.GetOriginalSprite(), AtlasSystem.Request.Type.WhiteMask);
					if (reqSprite == null) {
						PartiallyBatchedCollider batched = new PartiallyBatchedCollider();

						batched.collider = id;

						light.Buffer.lightingAtlasBatches.colliderList.Add(batched);
						continue;
					} else {
						shape.spriteShape.SetAtlasSprite(reqSprite);
						sprite = reqSprite;
					}
				}

				Vector2 position = shape.transform2D.position - light.transform2D.position;
		
				virtualSpriteRenderer.sprite = sprite;

				LayerSetting[] layerSettings = light.GetLayerSettings();

				Rendering.Universal.WithAtlas.Sprite.Draw(virtualSpriteRenderer, layerSettings[0], id.maskEffect, position, shape.transform2D.scale, shape.transform2D.rotation, z);
			}
		}
	}
}