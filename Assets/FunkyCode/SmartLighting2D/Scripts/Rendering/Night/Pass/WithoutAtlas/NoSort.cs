using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Night.WithoutAtlas {

    public class NoSort {

        public static void Draw(Pass pass) {
            // Draw Rooms
            foreach (LightingRoom2D id in LightingRoom2D.GetList()) {
                if ((int)id.nightLayer != pass.layerId) {
                    continue;
                }

                Room.Draw(id, pass.camera, pass.z);
            }

            // Draw Tilemap Rooms
            #if UNITY_2017_4_OR_NEWER
            
                foreach (LightingTilemapRoom2D id in LightingTilemapRoom2D.GetList()) {
                    if ((int)id.nightLayer != pass.layerId) {
                        continue;
                    }
                    
                    TilemapRoom.Draw(id, pass.camera, pass.z);
                }
            #endif

            // Draw Light Sprite
            List<LightingSpriteRenderer2D> spriteRendererList = LightingSpriteRenderer2D.GetList();
            for(int i = 0; i < spriteRendererList.Count; i++) {
                LightingSpriteRenderer2D id = spriteRendererList[i];

                if ((int)id.nightLayer != pass.layerId) {
                    continue;
                }

                SpriteRenderer2D.Draw(id, pass.camera, pass.z);
            }

            // Draw Light Texture
            List<LightingTextureRenderer2D> textureRendererList= LightingTextureRenderer2D.GetList();
			for(int i = 0; i < textureRendererList.Count; i++) {
				LightingTextureRenderer2D id = textureRendererList[i];

				if ((int)id.nightLayer != pass.layerId) {
					continue;
				}

				TextureRenderer.Draw(id, pass.camera, pass.z);
			}

            // Draw Light Particle Renderer
            List<LightingParticleRenderer2D> particleRendererList = LightingParticleRenderer2D.GetList();
			for(int i = 0; i < particleRendererList.Count; i++) {
				LightingParticleRenderer2D id = particleRendererList[i];

				if ((int)id.nightLayer != pass.layerId) {
					continue;
				}

				ParticleRenderer.Draw(id, pass.camera, pass.z);
			}

            // Draw Light Source
            foreach (LightingSource2D id in LightingSource2D.GetList()) {
                if ((int)id.nightLayer != pass.layerId) {
                    continue;
                }

               Rendering.Night.LightSource.Draw(id, pass.camera, pass.z);
            }

            // Draw Light Mesh
            foreach (LightMesh2D id in LightMesh2D.GetList()) {
                if ((int)id.nightLayer != pass.layerId) {
                    continue;
                }

               Rendering.Night.LightMesh.Draw(id, pass.camera, pass.z);
            }
        }
    }
}