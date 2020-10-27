using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Universal.WithoutAtlas {

	public class Sprite : Base {
        static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();

		static  public void Draw(SpriteMeshObject spriteMeshObject, Material material, SpriteRenderer spriteRenderer, Vector2 position, Vector2 scale, float rotation, float z) {
			FullRect.Draw(spriteMeshObject, material, spriteRenderer, position, scale,  rotation, z);
		}

		public class Tight {
			// ??
		}

		public class FullRect {

			public class Simple {
				
				static public void Draw(SpriteMeshObject spriteMeshObject, Material material, SpriteRenderer spriteRenderer, Vector2 position, Vector2 scale, float rotation, float z) {
					virtualSpriteRenderer.Set(spriteRenderer);
					
					Draw(spriteMeshObject, material, virtualSpriteRenderer, position, scale, rotation, z);
				}

				static public void Draw(SpriteMeshObject spriteMeshObject, Material material, VirtualSpriteRenderer spriteRenderer, Vector2 position, Vector2 scale, float rotation, float z) {
					SpriteTransform spriteTransform = new SpriteTransform(spriteRenderer, position, scale, rotation);

					WithoutAtlas.Texture.Draw(material, spriteTransform.position, spriteTransform.scale, spriteTransform.uv, rotation, z);
				}
			}

			public class Tiled {
				static public void Draw(SpriteMeshObject spriteMeshObject, Material material, SpriteRenderer spriteRenderer, Vector2 pos, Vector2 size, float rotation, float z) {
					material.SetPass (0); 
					GLExtended.DrawMesh(spriteMeshObject.GetTiledMesh().GetMesh(spriteRenderer), pos, size, rotation);
				}
			}

			static public void Draw(SpriteMeshObject spriteMeshObject, Material material, SpriteRenderer spriteRenderer, Vector2 pos, Vector2 size, float rotation, float z) {
				if (spriteRenderer.drawMode == SpriteDrawMode.Tiled && spriteRenderer.tileMode == SpriteTileMode.Continuous) {
					Tiled.Draw(spriteMeshObject, material, spriteRenderer, pos, size, rotation, z);
				} else {
					Simple.Draw(spriteMeshObject, material, spriteRenderer, pos, size, rotation, z);
				}
			}
		}	
    }
}