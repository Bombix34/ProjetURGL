using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithoutAtlas {
        
    public class Sorted {

        public static void Draw(Rendering.Light.Pass pass) {
            for(int i = 0; i < pass.sortPass.sortList.count; i ++) {
                pass.sortPass.sortObject = pass.sortPass.sortList.list[i];

                switch (pass.sortPass.sortObject.type) {
                    case Sorting.SortObject.Type.Collider:
                        DrawCollider(pass);
                    break;

                    case Sorting.SortObject.Type.Tile:
                        DrawTile(pass);
                    break;

                     case Sorting.SortObject.Type.TilemapMap:
                        DrawTileMap(pass);
                    break;

                }
            }
        }

        public static void DrawCollider(Rendering.Light.Pass pass) {
            LightingCollider2D collider = (LightingCollider2D)pass.sortPass.sortObject.lightObject;

            if (collider.shadowLayer == pass.layerID && pass.drawShadows) {	

                Lighting2D.materials.GetAtlasMaterial().SetPass(0);
                GL.Begin(GL.TRIANGLES);
                    Light.Shadow.Shape.Draw(pass.light, collider);
                GL.End();
                
            }

            // Masking
            if (collider.maskLayer == pass.layerID && pass.drawMask) {
                pass.materialWhite.color = Color.white;

                switch(collider.mainShape.maskType) {
                    case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
                    case LightingCollider2D.MaskType.Collider2D:
                    case LightingCollider2D.MaskType.CompositeCollider2D:
                    
                        pass.materialWhite.SetPass(0);
                        GL.Begin(GL.TRIANGLES);
                            Light.Shape.Mask(pass.light, collider, pass.layer, pass.z);
                        GL.End();

                    break;

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

                    case LightingCollider2D.MaskType.MeshRenderer:
                        pass.materialWhite.SetPass(0);
                        
                        GL.Begin(GL.TRIANGLES);
                            Mesh.Mask(pass.light, collider, pass.materialWhite, pass.layer, pass.z);
                        GL.End();
                    break;

                    case LightingCollider2D.MaskType.SkinnedMeshRenderer:
                        pass.materialWhite.SetPass(0);
            
                        GL.Begin(GL.TRIANGLES);
                            SkinnedMesh.Mask(pass.light, collider, pass.materialWhite, pass.layer, pass.z);
                        GL.End();
                    break;
                }
            }
        }

        static public void DrawTile(Rendering.Light.Pass pass) {
            #if UNITY_2017_4_OR_NEWER

                LightingTile tile = (LightingTile)pass.sortPass.sortObject.lightObject;
                LightingTilemapCollider2D tilemap = pass.sortPass.sortObject.tilemap;

                if (pass.sortPass.sortObject.tilemap.shadowLayer == pass.layerID && pass.drawShadows) {
                    Lighting2D.materials.GetAtlasMaterial().SetPass(0);
                                        
                    GL.Begin(GL.TRIANGLES); 
                        Light.Shadow.Tile.Draw(pass.light, tile, tilemap);
                    GL.End();  
                }

                // sprite mask - but what about shape mask?
                if (tilemap.maskLayer == pass.layerID && pass.drawMask) {
                    Tile.MaskSprite(tile, pass.layer, pass.materialWhite, -pass.light.transform2D.position, tilemap, pass.lightSizeSquared, pass.z);
                }    

             #endif     
        }

        static public void DrawTileMap(Rendering.Light.Pass pass) {
            #if UNITY_2017_4_OR_NEWER

                LightingTilemapCollider2D tilemap = pass.sortPass.sortObject.tilemap;
               
                if (tilemap.shadowLayer == pass.layerID && pass.drawShadows) {	
                                 
                    Lighting2D.materials.GetAtlasMaterial().SetPass(0);
                                        
                    GL.Begin(GL.TRIANGLES);

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

                    GL.End();  
                }

                if (pass.sortPass.sortObject.tilemap.maskLayer == pass.layerID && pass.drawMask) {
                    switch(tilemap.mapType) {
                        case MapType.UnityRectangle:
                            switch(tilemap.rectangle.maskType) {
                                case LightingTilemapCollider.Rectangle.MaskType.Sprite:
                                    TilemapRectangle.Sprite(pass.light, tilemap, pass.materialWhite, pass.layer, pass.z);
                                break;
                                
                                case LightingTilemapCollider.Rectangle.MaskType.BumpedSprite:
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