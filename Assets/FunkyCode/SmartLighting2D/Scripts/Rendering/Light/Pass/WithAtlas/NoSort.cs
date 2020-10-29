using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithAtlas {

    public class NoSort  {

        public static void Draw(Rendering.Light.Pass pass) {
            Lighting2D.materials.GetAtlasMaterial().SetPass(0);

            GL.Begin(GL.TRIANGLES);

            if (pass.drawShadows) {
                Shadows.Draw(pass);
            }

            if (pass.drawMask) {
               Mask.Draw(pass);
            }

            GL.End();

            DrawBatchedColliderMask(pass);
            DrawBatchedTilemapColliderMask(pass);
        }

        public class Shadows {
            static public void Draw(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerCollisionList.Count;

                if (colliderCount > 0) {
                    for(int id = 0; id < colliderCount; id++) {
                        Shadow.Shape.Draw(pass.light, pass.layerCollisionList[id]);
                    }
                }

                #if UNITY_2017_4_OR_NEWER
                    for(int id = 0; id < pass.tilemapList.Count; id++) {
                        if (pass.tilemapList[id].shadowLayer != pass.layerID) {
                            continue;
                        }

                        switch(pass.tilemapList[id].mapType) {
                            case MapType.UnityRectangle:
                                Shadow.TilemapRectangle.Draw(pass.light, pass.tilemapList[id], pass.lightSizeSquared, pass.z);
                            break;

                        }

                        //if (pass.tilemapList[id].rectangle.shadowType == LightingTilemapCollider.Rectangle.shadowType.Collider) {
                            //Shadow.TilemapCollider.Draw(pass.buffer, pass.tilemapList[id], pass.lightSizeSquared, pass.z);
                        //}

                    }
                #endif 

            }
        }

        public class Mask {
             static public void Draw(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerMaskList.Count;

                if (colliderCount > 0) {
                    for(int id = 0; id < colliderCount; id++) {
                        LightingCollider2D collider = pass.layerMaskList[id]; 

                        switch(collider.mainShape.maskType) {
                            case LightingCollider2D.MaskType.Collider2D:
                              //  Shape.Mask(pass.buffer, collider, pass.layer, pass.z);
                            break;

                            case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
                            //    Shape.Mask(pass.buffer, collider, pass.layer, pass.z);
                            break;

                            case LightingCollider2D.MaskType.Sprite:
                                SpriteRenderer2D.Mask(pass.light, collider, pass.z);

                            break;
                        }
                    }
                }
                    
                #if UNITY_2017_4_OR_NEWER		
                    for(int id = 0; id < pass.tilemapList.Count; id++) {
                        if (pass.tilemapList[id].maskLayer != pass.layerID) {
                            continue;
                        }

                        WithoutAtlas.TilemapRectangle.MaskShape(pass.light, pass.tilemapList[id], pass.z);

                        // ?
                        WithAtlas.TilemapRectangle.Sprite(pass.light, pass.tilemapList[id], pass.z);
                    }
                #endif
            } 
        }
       
        static public void DrawBatchedColliderMask(Rendering.Light.Pass pass) {
            // Partialy Batched (Default Edition)
            if (pass.light.Buffer.lightingAtlasBatches.colliderList.Count > 0) {		

                for(int i = 0; i < pass.light.Buffer.lightingAtlasBatches.colliderList.Count; i++) {
                    PartiallyBatchedCollider batch = pass.light.Buffer.lightingAtlasBatches.colliderList[i];

                    WithoutAtlas.SpriteRenderer2D.Mask(pass.light, batch.collider, pass.materialWhite, pass.layer, pass.z);
                }

                pass.light.Buffer.lightingAtlasBatches.colliderList.Clear();
            }
        }

        static public void DrawBatchedTilemapColliderMask(Rendering.Light.Pass pass) {
            if (pass.light.Buffer.lightingAtlasBatches.tilemapList.Count > 0) {

                for(int i = 0; i < pass.light.Buffer.lightingAtlasBatches.tilemapList.Count; i++) {
                    PartiallyBatchedTilemap batch = pass.light.Buffer.lightingAtlasBatches.tilemapList[i];

                    pass.materialWhite.color = LayerSettingColor.Get(batch.polyOffset, pass.layer, MaskEffect.Lit);

                    pass.materialWhite.mainTexture = batch.virtualSpriteRenderer.sprite.texture;

                    Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(batch.tile.spriteMeshObject, pass.materialWhite, batch.virtualSpriteRenderer, batch.polyOffset, batch.tileSize, 0, pass.z);
                }

                pass.materialWhite.mainTexture = null;
                
                pass.light.Buffer.lightingAtlasBatches.tilemapList.Clear();
            }
        }
    }
}
