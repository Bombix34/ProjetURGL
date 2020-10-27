using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Night.WithoutAtlas {
        
    public class TextureRenderer  {
        
		public static void Draw(LightingTextureRenderer2D id , Camera camera, float z) {
			if (id.InCamera(camera) == false) {
				return;
			}

			Vector2 offset = -camera.transform.position;

			Material material;

			switch(id.shaderMode) {
				case LightingTextureRenderer2D.ShaderMode.Additive:
					material = Lighting2D.materials.GetAdditive();
					material.SetColor ("_TintColor", id.color);

					material.mainTexture = id.texture;

					Rendering.Universal.WithoutAtlas.Texture.Draw(material, new Vector3(offset.x, offset.y) + id.transform.position, id.size, 0, z);
					
					material.mainTexture = null;
				break;

				case LightingTextureRenderer2D.ShaderMode.Multiply:

					material = Lighting2D.materials.GetMultiplyHDR();
					material.SetColor ("_TintColor", id.color);

					material.mainTexture = id.texture;

					Rendering.Universal.WithoutAtlas.Texture.Draw(material, new Vector3(offset.x, offset.y) + id.transform.position, id.size, 0, z);
					
					material.mainTexture = null;

				break;
			}

		
		}
    }
}