using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FogOfWar {

	public static class Sorted {
		static SortPass sortPass = new SortPass();
		
		public static void Draw(Camera camera) {
			sortPass.SortObjects();

			Material material = null;

			Vector2 objectPosition = new Vector2();

			for(int i = 0; i < sortPass.sortList.count; i++) {
				FogOfWarSprite sprite = sortPass.sortList.list[i].sprite;

				if (sprite == null) {
					continue;
				}

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

	public class SortPass {
        public Sorting.SortList sortList = new Sorting.SortList();
        public Sorting.SortObject sortObject;

        public void Clear() {
            sortList.count = 0;
        }

		public void SortObjects() {
            sortList.Reset();

			List<FogOfWarSprite> sprites = FogOfWarSprite.GetList();

			LightingSettings.FogOfWar.Sorting sorting = Lighting2D.Profile.fogOfWar.sorting;

            for(int id = 0; id < sprites.Count; id++) {
                FogOfWarSprite sprite = sprites[id]; // Check If It's In Light Area?

                //if (collider.InLightSource(pass.light) == false) {
                //    continue;
                //}

                switch(sorting) {
                    case LightingSettings.FogOfWar.Sorting.ZAxisLower:

						sortList.Add(sprite, -sprite.transform.position.z);

					break;

					case LightingSettings.FogOfWar.Sorting.SortingOrder:

						sortList.Add(sprite, sprite.GetSortingOrder());
			
                    break;

					case LightingSettings.FogOfWar.Sorting.SortingOrderAndLayer:

						sortList.Add(sprite, sprite.GetSortingOrder() + sprite.GetSortingLayer() * 100000);
			
                    break;
                }
            }

            sortList.Sort();
        }
	}
}