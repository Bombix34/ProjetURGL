using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

namespace Rendering.Day {
	
	public class Main {

		static Pass pass = new Pass();

		public static void Draw(Camera camera, BufferPreset bufferPreset) {
			if (Lighting2D.dayLightingSettings.alpha == 0) {
				return;
			}

			LightingLayerSetting[] layerSettings = bufferPreset.dayLayers.Get();

			if (layerSettings.Length > 0) {
				for(int i = 0; i < layerSettings.Length; i++) {
					LightingLayerSetting dayLayer = layerSettings[i];

					LightingLayerSettingSorting sorting = dayLayer.sorting;

					if (pass.Setup(dayLayer, camera) == false) {
						continue;
					}

					if (sorting == LightingLayerSettingSorting.None) {
						int layer = (int)dayLayer.layer;

						if (Lighting2D.atlasSettings.lightingSpriteAtlas) {
							// ???
						} else {
							Rendering.Day.WithoutAtlas.NoSort.Draw(pass);
						}
					} else {
						pass.SortObjects();

						if (Lighting2D.atlasSettings.lightingSpriteAtlas) {
							// ???
						} else {
							Rendering.Day.WithoutAtlas.Sorted.Draw(pass);
						}
					}
				}
				
				ShadowDarkness(camera);
			}

			
		}

		static void ShadowDarkness(Camera camera) {
			if (Lighting2D.dayLightingSettings.enable == false) {
				return;
			}

			Color color = new Color(0, 0, 0,  (1f - Lighting2D.dayLightingSettings.alpha) / 2);

			if (color.a > 0) {
				color.r = 0.5f;
				color.g = 0.5f;
				color.b = 0.5f;
					
				Material material = Lighting2D.materials.GetAlphaBlend();
				material.mainTexture = null;		
				material.SetColor ("_TintColor", color);

				Rendering.Universal.WithoutAtlas.Texture.Draw(material, Vector2.zero, LightingRender2D.GetSize(camera), camera.transform.eulerAngles.z, 0);
			}
		}
	}
}