using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light {
	
	public class Main {
		private static Pass pass = new Pass();

		public static void Draw(LightingSource2D light) {
			ShadowEngine.Prepare(light);

            LayerSetting[] layerSettings = light.GetLayerSettings();

			if (layerSettings == null) {
				return;
			}

			if (layerSettings.Length < 1) {
				return;
			}

			for (int layerID = 0; layerID < layerSettings.Length; layerID++) {
				LayerSetting layerSetting = layerSettings[layerID];

				if (layerSetting == null) {
					continue;
				}

				if (pass.Setup(light, layerSetting) == false) {
					continue;
				}

				ShadowEngine.SetPass(light, layerSetting);

				bool useAtlas = Lighting2D.atlasSettings.lightingSpriteAtlas && AtlasSystem.Manager.GetAtlasPage() != null;

				if (layerSetting.sorting == LightingLayerSorting.None) {
					if (useAtlas) {
						WithAtlas.NoSort.Draw(pass);
					} else {
						WithoutAtlas.NoSort.Draw(pass);
					}
				} else {
					pass.sortPass.SortObjects();

					if (useAtlas) {
						WithAtlas.Sorted.Draw(pass);
					} else {
						WithoutAtlas.Sorted.Draw(pass);
					}
				}
			}
	
			Light.LightSource.Main.Draw(light);
		}
	}
}