using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Night.WithoutAtlas {
	
    public class TilemapRoom {

        static public void Draw(LightingTilemapRoom2D id, Camera camera, float z) {
            Material materialColormask = Lighting2D.materials.GetSpriteMask();
            Material materialMultiply = Lighting2D.materials.GetMultiplyHDR();

            Material material = null;

            switch(id.shaderType) {
                case LightingTilemapRoom2D.ShaderType.ColorMask:
                    material = materialColormask;
                    break;

                case LightingTilemapRoom2D.ShaderType.MultiplyTexture:
                    material = materialMultiply;
                    break;
            }

            switch(id.maskType) {
                case LightingTilemapRoom2D.MaskType.Sprite:
                    
                    switch(id.mapType) {
                        case MapType.UnityRectangle:
                            material.color = id.color;
                            
                            Sprite.Draw(camera, id, material, z);

                            material.color = Color.white;
                        break;	

                        case MapType.SuperTilemapEditor:
                            SuperTilemapEditorSupport.RenderingRoom.DrawTiles(camera, id, material, z);

                        break;
                    }
                    
                break;
            }			
        }

        public class Sprite {
            
            public static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

            public static void Draw(Camera camera, LightingTilemapRoom2D id, Material material, float z) {
                Vector2 cameraPosition = -camera.transform.position;

                float cameraRadius = CameraHelper.GetRadius(camera);

                LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();

                foreach(LightingTile tile in id.rectangle.mapTiles) {
                    if (tile.GetOriginalSprite() == null) {
                       continue;
                    }

                    Vector2 tilePosition = tile.GetWorldPosition(tilemapCollider);

                    tilePosition += cameraPosition;

                    if (tile.NotInRange(tilePosition, cameraRadius)) {
                       continue;
                    }

                    spriteRenderer.sprite = tile.GetOriginalSprite();
                
                    material.mainTexture = spriteRenderer.sprite.texture;
        
                    Rendering.Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, spriteRenderer, tilePosition, tile.worldScale, tile.worldRotation, z);
                    
                    material.mainTexture = null;
                }
            }
        }
    }
}
