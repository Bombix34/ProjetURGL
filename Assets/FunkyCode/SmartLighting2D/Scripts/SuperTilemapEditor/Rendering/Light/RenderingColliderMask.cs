using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

 #if (SUPER_TILEMAP_EDITOR)

    namespace SuperTilemapEditorSupport {
        
        public class RenderingColliderMask { 

            public class WithoutAtlas {   
            
                static public void Sprite(LightingSource2D light, LightingTilemapCollider2D id, Material material) {
                    if (id.mapType != MapType.SuperTilemapEditor) {
                        return;
                    }

                    if (id.superTilemapEditor.maskType != SuperTilemapEditorSupport.TilemapCollider2D.MaskType.Sprite) {
                        return;
                    }

                    Vector2 lightPosition = -light.transform.position;
                    LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();

                    if (id.superTilemapEditor.tilemap != null) {
                        if (id.superTilemapEditor.tilemap.Tileset != null) {
                            material.mainTexture = id.superTilemapEditor.tilemap.Tileset.AtlasTexture;
                        }
                    }
                
                    material.SetPass (0); 
                    GL.Begin (GL.QUADS);
        
                    foreach(LightingTile tile in id.superTilemapEditor.mapTiles) {
                        tile.UpdatePosition(tilemapCollider);
                        
                        Vector2 tilePosition = tile.GetWorldPosition();
                        tilePosition += lightPosition;

                        if (tile.NotInRange(tilePosition, light.size)) {
                            continue;
                        }

                        Vector2 scale = tile.worldScale * 0.5f;

                        if (tile.flipX) {
                            scale.x = -scale.x;
                        }

                        if (tile.flipY) {
                            scale.y = -scale.y;
                        }
                    
                        Rendering.Universal.WithoutAtlas.Texture.DrawPassSTE(tilePosition, scale, tile.uv, tile.worldRotation, 0);
                    }

                    GL.End ();

                    material.mainTexture = null;
                }
            }
    
            static public void Grid(LightingSource2D light, LightingTilemapCollider2D id) {
                if (id.mapType != MapType.SuperTilemapEditor) {
                    return;
                }
                
                if (id.superTilemapEditor.maskType != SuperTilemapEditorSupport.TilemapCollider2D.MaskType.Grid) {
                    return;
                }

                Vector2 lightPosition = -light.transform.position;
                MeshObject tileMesh = LightingTile.Rectangle.GetStaticTileMesh(id);
          
                GL.Color(Color.white);

                foreach(LightingTile tile in id.superTilemapEditor.mapTiles) {
                    Vector2 tilePosition = tile.GetWorldPosition();
                    tilePosition += lightPosition;
                    
                    if (tile.NotInRange(tilePosition, light.size)) {
                        continue;
                    }

                    GLExtended.DrawMeshPass(tileMesh, tilePosition, tile.worldScale, tile.worldRotation);		
                }
            }

        }
    }

#else  

    namespace SuperTilemapEditorSupport {
            public class RenderingColliderMask { 
                static public void Grid(LightingSource2D light, LightingTilemapCollider2D id) {}
                public class WithoutAtlas {
                    static public void Sprite(LightingSource2D light, LightingTilemapCollider2D id, Material material) {
                }
            }
        }
    }

#endif