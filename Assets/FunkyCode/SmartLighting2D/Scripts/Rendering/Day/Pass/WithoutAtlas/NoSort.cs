using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

namespace Rendering.Day.WithoutAtlas {

    public class NoSort {

        static public void Draw(Pass pass) {
            List<DayLightingCollider2D> colliderList = DayLightingCollider2D.GetList();
            int colliderCount = colliderList.Count;

            List<DayLightingTilemapCollider2D> tilemapColliderList = DayLightingTilemapCollider2D.GetList();
            int tilemapColliderCount = tilemapColliderList.Count;

            bool drawShadows = pass.layer.type != LightingLayerSettingType.MaskOnly;
            bool drawMask = pass.layer.type != LightingLayerSettingType.ShadowsOnly;

            if (drawShadows) {
                Lighting2D.materials.GetShadowBlur().SetPass (0);
                
                GL.Begin(GL.TRIANGLES);


                for(int i = 0; i < colliderCount; i++) {
                    DayLightingCollider2D id = colliderList[i];
                    
                    if (id.shadowLayer != pass.layerId) {
                        continue;
                    }

                    WithoutAtlas.Shadow.Draw(id, pass.offset, pass.z);                
                }

                for(int i = 0; i < tilemapColliderCount; i++) {
                    DayLightingTilemapCollider2D id = tilemapColliderList[i];
                    
                    if ((int)id.shadowLayer != pass.layerId) {
                        continue;
                    }

                    WithoutAtlas.Shadow.DrawTilemap(id, pass.offset, pass.z);                
                }


                GL.End();

                    
                for(int i = 0; i < colliderCount; i++) {
                    DayLightingCollider2D id = colliderList[i];

                    if (id.shadowLayer != pass.layerId) {
                        continue;
                    }
                
                    WithoutAtlas.SpriteRendererShadow.Draw(id, pass.offset, pass.z);
                }
            }

            
            if (drawMask) {
                for(int i = 0; i < colliderCount; i++) {
                    DayLightingCollider2D id = colliderList[i];

                    if (id.maskLayer != pass.layerId) {
                        continue;
                    }

                    WithoutAtlas.SpriteRenderer2D.Draw(id, pass.offset, pass.z);
                }

                for(int i = 0; i < tilemapColliderCount; i++) {
                    DayLightingTilemapCollider2D id = tilemapColliderList[i];

                    if ((int)id.maskLayer != pass.layerId) {
                        continue;
                    }

                    WithoutAtlas.SpriteRenderer2D.DrawTilemap(id, pass.offset, pass.z);
                }
            }
        }
    }
}
