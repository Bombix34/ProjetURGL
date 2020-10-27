using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

namespace Rendering.Night {
	
	public class Main {

		static Pass pass = new Pass();

		public static void Draw(Camera camera, BufferPreset bufferPreset) {
			DarknessColor(camera, bufferPreset);

			LightingLayerSetting[] layerSettings = bufferPreset.nightLayers.Get();
			
			if (layerSettings == null) {
				return;
			}

			if (layerSettings.Length < 1) {
				return;
			}

			for(int i = 0; i < layerSettings.Length; i++) {
				LightingLayerSetting nightLayer = layerSettings[i];

				if (pass.Setup(nightLayer, camera) == false) {
					continue;
				}

				if (nightLayer.sorting == LightingLayerSettingSorting.None) {

					if (Lighting2D.atlasSettings.lightingSpriteAtlas) {
						Rendering.Night.WithAtlas.NoSort.Draw(pass);
					} else {
						Rendering.Night.WithoutAtlas.NoSort.Draw(pass);
					}

				} else {

					pass.SortObjects();

					if (Lighting2D.atlasSettings.lightingSpriteAtlas) {
						// ???
					} else {
						Rendering.Night.WithoutAtlas.Sorted.Draw(pass);
					}

				}
			}
		}

		public static void DarknessColor(Camera camera, BufferPreset bufferPreset) {
			Color color = bufferPreset.darknessColor;

			if (color.a > 0) {
				Material material = Lighting2D.materials.GetAlphaBlend();		
				material.SetColor ("_TintColor", color);
				material.mainTexture = null;

				Rendering.Universal.WithoutAtlas.Texture.Draw(material, Vector2.zero, LightingRender2D.GetSize(camera), camera.transform.eulerAngles.z, 0);
			}
		}		
	}
}