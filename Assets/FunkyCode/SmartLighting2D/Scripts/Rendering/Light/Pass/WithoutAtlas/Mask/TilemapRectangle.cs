using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.WithoutAtlas {

    public class TilemapRectangle {
        public static VirtualSpriteRenderer virtualSpriteRenderer = new VirtualSpriteRenderer();
        
        static public void Sprite(LightingSource2D light, LightingTilemapCollider2D id, Material material, LayerSetting layerSetting, float z) {
            if (id.mapType != MapType.UnityRectangle) {
                return;
            }

            if (id.rectangle.maskType != LightingTilemapCollider.Rectangle.MaskType.Sprite) {
                return;
            }
            
            Vector2 lightPosition = -light.transform.position;
            bool isGrid = false;

            Vector2 scale = Tilemap.Rectangle.GetScale(id.GetTilemapProperties(), isGrid);
            float rotation = id.transform.eulerAngles.z;

            foreach(LightingTile tile in id.rectangle.mapTiles) {

                if (tile.GetOriginalSprite() == null) {
                    return;
                }

                Vector2 tilePosition = tile.GetWorldPosition();

                tilePosition += lightPosition;

                if (tile.NotInRange(tilePosition, light.size)) {
                    continue;
                }

                virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

                material.color = LayerSettingColor.Get(tilePosition, layerSetting, MaskEffect.Lit);
                
                material.mainTexture = virtualSpriteRenderer.sprite.texture;
    
                Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, virtualSpriteRenderer, tilePosition, scale, rotation, z);
                
                material.mainTexture = null;
            
            }
        }

        static public void BumpedSprite(LightingSource2D light, LightingTilemapCollider2D id, Material material, float z) {
            if (id.mapType != MapType.UnityRectangle) {
                return;
            }

            if (id.rectangle.maskType != LightingTilemapCollider.Rectangle.MaskType.BumpedSprite) {
                return;
            }
 
            Texture bumpTexture = id.bumpMapMode.GetBumpTexture();

            if (bumpTexture == null) {
                return;
            }

            material.SetTexture("_Bump", bumpTexture);

            Vector2 lightPosition = -light.transform.position;
            bool isGrid = false;

            TilemapProperties properties = id.GetTilemapProperties();

            Vector2 scale = Tilemap.Rectangle.GetScale(properties, isGrid);

            foreach(LightingTile tile in id.rectangle.mapTiles) {
                if (tile.GetOriginalSprite() == null) {
                    return;
                }

                Vector2 tilePosition = Tilemap.Rectangle.GetTilePosition(tile, properties);

                tilePosition += lightPosition;

                if (tile.NotInRange(tilePosition, light.size)) {
                    continue;
                }

                virtualSpriteRenderer.sprite = tile.GetOriginalSprite();

                material.mainTexture = virtualSpriteRenderer.sprite.texture;
    
                Universal.WithoutAtlas.Sprite.FullRect.Simple.Draw(tile.spriteMeshObject, material, virtualSpriteRenderer, tilePosition, scale, 0, z);

                material.mainTexture = null;
            }
        }

        static public void MaskShape(LightingSource2D light, LightingTilemapCollider2D id, float z) {
            if (id.mapType != MapType.UnityRectangle) {
                return;
            }
            
            if (false == (id.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.SpriteCustomPhysicsShape || id.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.Grid)) {
                return;
            }

            Vector2 lightPosition = -light.transform.position;
            bool isGrid = id.rectangle.maskType == LightingTilemapCollider.Rectangle.MaskType.Grid;

            TilemapProperties properties = id.GetTilemapProperties();
            Vector2 scale = Tilemap.Rectangle.GetScale(properties, isGrid);
            float rotation = id.transform.eulerAngles.z;

            MeshObject tileMesh = null;	
            if (isGrid) {
                tileMesh = LightingTile.Rectangle.GetStaticTileMesh(id);
            }

            GL.Color(Color.white);

            foreach(LightingTile tile in id.rectangle.mapTiles) {
                Vector2 tilePosition = Tilemap.Rectangle.GetTilePosition(tile, properties);

                tilePosition += lightPosition;
                
                if (tile.NotInRange(tilePosition, light.size)) {
                    continue;
                }

                if (isGrid == false) {
                    tileMesh = null;
                    tileMesh = tile.GetTileDynamicMesh();
                }

                if (tileMesh == null) {
                    continue;
                }

                GLExtended.DrawMeshPass(tileMesh, tilePosition, scale, rotation);		
            }
        }
    }
}