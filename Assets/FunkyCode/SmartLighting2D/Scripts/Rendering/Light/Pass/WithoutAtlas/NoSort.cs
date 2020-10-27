using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithoutAtlas {

    public class NoSort {

        public static void Draw(Rendering.Light.Pass pass) {
            if (pass.drawShadows) {
                Shadow.Draw(pass);
            }

            if (pass.drawMask) {
                Mask.Draw(pass);
            }
        }

        public class Shadow {

            public static void Draw(Rendering.Light.Pass pass) {
                Material material = Lighting2D.materials.GetAtlasMaterial();
                material.color = Color.white;

                material.SetPass(0);

                GL.Begin(GL.TRIANGLES);

                DrawCollider(pass);
                DrawTilemapCollider(pass);
        
                GL.End();
            }

            public static void DrawCollider(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerCollisionList.Count;

                if (colliderCount < 1) {
                    return;
                }

                for(int id = 0; id < colliderCount; id++) {
                    LightingCollider2D collider = pass.layerCollisionList[id];

                    if (collider.mainShape.shadowType == LightingCollider2D.ShadowType.None) {
                        continue;
                    }
                   
                    Light.Shadow.Shape.Draw(pass.light, collider);
                }
            }

            public static void DrawTilemapCollider(Rendering.Light.Pass pass) {
                #if UNITY_2017_4_OR_NEWER
                    for(int id = 0; id < pass.tilemapList.Count; id++) {
                        LightingTilemapCollider2D tilemap = pass.tilemapList[id];

                        if (tilemap.shadowLayer != pass.layerID) {
                            continue;
                        }

                         if (tilemap.IsNotInRange(pass.light)) {
                            continue;
                        }

                        switch(tilemap.mapType) {
                            case MapType.UnityRectangle:
                                Light.Shadow.TilemapRectangle.Draw(pass.light, tilemap, pass.lightSizeSquared, pass.z);
                                Light.Shadow.TilemapCollider.Rectangle.Draw(pass.light, tilemap);
                            break;

                            case MapType.UnityIsometric:
                                Light.Shadow.TilemapIsometric.Draw(pass.light, tilemap);
                            break;

                            case MapType.UnityHexagon:
                                Light.Shadow.TilemapHexagon.Draw(pass.light, tilemap);
                            break;

                            case MapType.SuperTilemapEditor:

                                 switch(tilemap.superTilemapEditor.colliderType) {

                                    case SuperTilemapEditorSupport.TilemapCollider2D.ColliderType.Grid:
                                         SuperTilemapEditorSupport.RenderingColliderShadow.Grid(pass.light, tilemap);
                                    break;
                                     
                                    case SuperTilemapEditorSupport.TilemapCollider2D.ColliderType.Collider:
                                        SuperTilemapEditorSupport.RenderingColliderShadow.Collider(pass.light, tilemap);
                                    break;
                                 }
                               
                            break;
                        }
                        
                        
                    }
                #endif 
            }
        }

        public class Mask {

           static public void Draw(Rendering.Light.Pass pass) {
                Lighting2D.materials.GetSpriteMask().SetPass(0);

                GL.Begin(GL.TRIANGLES);

                    GL.Color(Color.white);
                    DrawCollider(pass);

                    GL.Color(Color.white);
                    DrawTilemapCollider(pass);

                GL.End();

                DrawMesh(pass);

                DrawSprite(pass);

                DrawTilemapSprite(pass);
            }

            static public void DrawCollider(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerMaskList.Count;

                if (colliderCount < 1) {
                    return;
                }

                for(int id = 0; id < colliderCount; id++) {
                    LightingCollider2D collider = pass.layerMaskList[id];

                    switch(collider.mainShape.maskType) {
                        case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
                        case LightingCollider2D.MaskType.CompositeCollider2D:
                        case LightingCollider2D.MaskType.Collider2D:
                            Light.Shape.Mask(pass.light, collider, pass.layer, pass.z);
                        break;
                    }
                }
            }

             static public void DrawMesh(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerMaskList.Count;

                if (colliderCount < 1) {
                    return;
                }

                for(int id = 0; id < colliderCount; id++) {
                    LightingCollider2D collider = pass.layerMaskList[id];

                    switch(collider.mainShape.maskType) {
                        case LightingCollider2D.MaskType.MeshRenderer:
                            Mesh.Mask(pass.light, collider, pass.materialWhite, pass.layer, pass.z);
                        break;

                        case LightingCollider2D.MaskType.SkinnedMeshRenderer:
                            SkinnedMesh.Mask(pass.light, collider, pass.materialWhite, pass.layer, pass.z);
                        break;
                    }
                }
            }

            static public void DrawSprite(Rendering.Light.Pass pass) {
                int colliderCount = pass.layerMaskList.Count;

                if (colliderCount < 1) {
                    return;
                }

                for(int id = 0; id < colliderCount; id++) {
                    LightingCollider2D collider = pass.layerMaskList[id];

                    switch(collider.mainShape.maskType) {
                        case LightingCollider2D.MaskType.Sprite:
                            SpriteRenderer2D.Mask(pass.light, collider, pass.materialWhite, pass.layer, pass.z);
                        break;

                        case LightingCollider2D.MaskType.BumpedSprite:

                            Material material = pass.materialNormalMap_PixelToLight;

                            if (collider.bumpMapMode.type == NormalMapType.ObjectToLight) {
                                material = pass.materialNormalMap_ObjectToLight;
                            }

                            SpriteRenderer2D.MaskNormalMap(pass.light, collider, material, pass.layer, pass.z);

                        break;
                    }
                }
            }

            static public void DrawTilemapCollider(Rendering.Light.Pass pass) {
                #if UNITY_2017_4_OR_NEWER
                    for(int id = 0; id < pass.tilemapList.Count; id++) {
                        LightingTilemapCollider2D tilemap = pass.tilemapList[id];

                        if (tilemap.maskLayer != pass.layerID) {
                            continue;
                        }

                        switch(tilemap.mapType) {
                            case MapType.UnityRectangle:
                                TilemapRectangle.MaskShape(pass.light, tilemap, pass.z);
                            break;

                            case MapType.UnityIsometric:
                                TilemapIsometric.MaskShape(pass.light, tilemap, pass.z);
                            break;

                            case MapType.UnityHexagon:
                                TilemapHexagon.MaskShape(pass.light, tilemap, pass.z);
                            break;

                            case MapType.SuperTilemapEditor:
                                SuperTilemapEditorSupport.RenderingColliderMask.Grid(pass.light, tilemap);
                            break;
                        }
                    }
                #endif
            }

            static public void DrawTilemapSprite(Rendering.Light.Pass pass) {
                #if UNITY_2017_4_OR_NEWER
                    for(int id = 0; id < pass.tilemapList.Count; id++) {
                        LightingTilemapCollider2D tilemap = pass.tilemapList[id];

                        if (tilemap.maskLayer != pass.layerID) {
                            continue;
                        }

                        switch(tilemap.mapType) {
                            case MapType.UnityRectangle:
                            
                                switch(tilemap.rectangle.maskType) {
                                    case LightingTilemapCollider.Rectangle.MaskType.Sprite:
                                        TilemapRectangle.Sprite(pass.light, tilemap, pass.materialWhite, pass.layer, pass.z);
                                    break;

                                    case Rectangle.MaskType.BumpedSprite:
                                        TilemapRectangle.BumpedSprite(pass.light, tilemap, pass.materialNormalMap_PixelToLight, pass.z);
                                    break;
                                }
                                
                            break;

                            case MapType.UnityIsometric:
                                TilemapIsometric.MaskSprite(pass.light, tilemap, pass.materialWhite, pass.z);
                                
                            break;

                            case MapType.UnityHexagon:
                                //TilemapHexagon.MaskSprite(pass.buffer, tilemap, pass.materialWhite, pass.z);
                                
                            break;

                            case MapType.SuperTilemapEditor:
                                SuperTilemapEditorSupport.RenderingColliderMask.WithoutAtlas.Sprite(pass.light, tilemap, pass.materialWhite);
                            break;
                        }                   
                    }
                #endif
            }
        }
    }
}