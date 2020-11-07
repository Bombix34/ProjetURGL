﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Day.WithoutAtlas {

    public class SpriteRenderer2D {

		static public void Draw(DayLightingCollider2D id, Vector2 offset, float z) {
			if (id.InAnyCamera() == false) {
				return;
			}

			float dayLightRotation = -(Lighting2D.dayLightingSettings.direction - 180) * Mathf.Deg2Rad;
			float dayLightHeight = Lighting2D.dayLightingSettings.bumpMap.height;
			float dayLightStrength = Lighting2D.dayLightingSettings.bumpMap.strength;

			switch(id.mainShape.maskType) {
				case DayLightingCollider2D.MaskType.None:
					return;

				case DayLightingCollider2D.MaskType.Sprite:
				
					Material material = Lighting2D.materials.GetSpriteMask();

					foreach(DayLightingColliderShape shape in id.shapes) {
						UnityEngine.SpriteRenderer spriteRenderer = shape.spriteShape.GetSpriteRenderer();

						if (spriteRenderer == null || spriteRenderer.sprite == null) {
							continue;
						}

						Vector2 objectOffset = shape.transform2D.position + offset;

						material.mainTexture = spriteRenderer.sprite.texture;

						Universal.WithoutAtlas.Sprite.Draw(id.spriteMeshObject, material, spriteRenderer, objectOffset, shape.transform2D.scale, shape.transform2D.rotation, z);
					
					}
				break;

				case DayLightingCollider2D.MaskType.BumpedSprite:

					Texture bumpTexture = id.normalMapMode.GetBumpTexture();

					if (bumpTexture == null) {
						return;
					}

					material = Lighting2D.materials.GetBumpedDaySprite();
					material.SetFloat("_LightRZ", -dayLightHeight);
					material.SetTexture("_Bump", bumpTexture);

					foreach(DayLightingColliderShape shape in id.shapes) {
						UnityEngine.SpriteRenderer spriteRenderer = shape.spriteShape.GetSpriteRenderer();

						if (spriteRenderer == null || spriteRenderer.sprite == null) {
							continue;
						}

						float rotation = dayLightRotation - shape.transform2D.rotation * Mathf.Deg2Rad;
						material.SetFloat("_LightRX", Mathf.Cos(rotation) * dayLightStrength);
						material.SetFloat("_LightRY", Mathf.Sin(rotation) * dayLightStrength);
							
						Vector2 objectOffset = shape.transform2D.position + offset;

						material.mainTexture = spriteRenderer.sprite.texture;

						Universal.WithoutAtlas.Sprite.FullRect.Draw(id.spriteMeshObject, material, spriteRenderer, objectOffset, id.transform.lossyScale, shape.transform2D.rotation, z);
					}

				break;
			}
		}


		static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();
    
		static public void DrawTilemap(DayLightingTilemapCollider2D id, Vector2 offset, float z) {
			//if (id.InAnyCamera() == false) {
			//	return;
			//}

			if (id.rectangle.maskType != LightingTilemapCollider.Rectangle.MaskType.Sprite) {
				return;
			}

			bool isGrid = false;

			Vector2 scale = Tilemap.Rectangle.GetScale(id.GetTilemapProperties(), isGrid);
            float rotation = id.transform.eulerAngles.z;

			Material material = Lighting2D.materials.GetSpriteMask();

			LightingTilemapCollider.Base tilemap = id.GetCurrentTilemap();

            foreach(LightingTile tile in id.rectangle.mapTiles) {

                if (tile.GetOriginalSprite() == null) {
                    return;
                }

				tile.UpdatePosition(tilemap);

                Vector2 tilePosition = tile.GetWorldPosition();

                tilePosition += offset;

               // if (tile.NotInRange(tilePosition, light.size)) {
                 //   continue;
                //}

                virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

                material.color = Color.white;
                
                material.mainTexture = virtualSpriteRenderer.sprite.texture;
    
                Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, virtualSpriteRenderer, tilePosition, scale, rotation, z);
                
                material.mainTexture = null;
            
            }
		}











	}
}