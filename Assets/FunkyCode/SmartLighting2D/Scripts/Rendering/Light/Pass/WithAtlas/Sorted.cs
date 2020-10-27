using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.WithAtlas {
        
    public class Sorted {

         public static void Draw(Rendering.Light.Pass pass) {
            Lighting2D.materials.GetAtlasMaterial().SetPass(0);

            GL.Begin(GL.TRIANGLES);
        
            for(int i = 0; i < pass.sortPass.sortList.count; i ++) {
                pass.sortPass.sortObject = pass.sortPass.sortList.list[i];

                switch (pass.sortPass.sortObject.type) {
                    case Sorting.SortObject.Type.Collider:
                        DrawCollder(pass);

                    break;

                    case Sorting.SortObject.Type.Tile:
                        DrawTilemapCollider(pass);
                    
                    break;
                }
            }
            
            GL.End();
        }

        public static void DrawCollder(Rendering.Light.Pass pass) {
            LightingCollider2D collider = (LightingCollider2D)pass.sortPass.sortObject.lightObject;

            if (pass.drawShadows && collider.shadowLayer == pass.layerID) {	
                switch(collider.mainShape.shadowType) {
                    case LightingCollider2D.ShadowType.Collider2D:
                    case LightingCollider2D.ShadowType.SpriteCustomPhysicsShape:
                        Shadow.Shape.Draw(pass.light, collider);
                    break;
                }
            }

            if (pass.drawMask && collider.maskLayer == pass.layerID) {
                switch(collider.mainShape.maskType) {
                    case LightingCollider2D.MaskType.Collider2D:
                    case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
                     //   Shape.Mask(pass.buffer, collider, pass.layer, pass.z);  

                    break;

                    case LightingCollider2D.MaskType.Sprite:
                        SpriteRenderer2D.Mask(pass.light, collider, pass.z);

                    break;
                }

                // Partialy Batched
                if (pass.light.Buffer.lightingAtlasBatches.colliderList.Count > 0) {
                    DrawColliderBatched(pass);
                }
            }
        }

        public static void DrawTilemapCollider(Rendering.Light.Pass pass) {
            LightingTile tile = (LightingTile)pass.sortPass.sortObject.lightObject;

            if (pass.drawShadows && pass.sortPass.sortObject.tilemap.shadowLayer == pass.layerID) {	
                Shadow.Tile.Draw(pass.light, tile, pass.sortPass.sortObject.tilemap);
            }

            if (pass.drawMask && pass.sortPass.sortObject.tilemap.maskLayer == pass.layerID) {
                Tile.MaskSprite(pass.light, tile, pass.layer, pass.sortPass.sortObject.tilemap, -pass.light.transform2D.position, pass.z);

                // Partialy Batched
                if (pass.light.Buffer.lightingAtlasBatches.tilemapList.Count > 0) {
                    DrawTilemapColliderBatched(pass);
                }
            }   
        }

        public static void DrawColliderBatched(Rendering.Light.Pass pass) {
            if (pass.light.Buffer.lightingAtlasBatches.colliderList.Count < 1) {
                return;
            }
            
            GL.End();

                
            for(int s = 0; s < pass.light.Buffer.lightingAtlasBatches.colliderList.Count; s++) {
                PartiallyBatchedCollider batch_collider = pass.light.Buffer.lightingAtlasBatches.colliderList[s];

                if (batch_collider.collider.mainShape.maskType == LightingCollider2D.MaskType.Sprite) {
                
                    WithoutAtlas.SpriteRenderer2D.Mask(pass.light, batch_collider.collider, pass.materialWhite, pass.layer, pass.z);
                }
            }

            pass.light.Buffer.lightingAtlasBatches.colliderList.Clear();

            Lighting2D.materials.GetAtlasMaterial().SetPass(0);
            GL.Begin(GL.TRIANGLES);
        }

        public static void DrawTilemapColliderBatched(Rendering.Light.Pass pass) {
            if (pass.light.Buffer.lightingAtlasBatches.tilemapList.Count < 1) {
                return;
            }

            GL.End();

            for(int s = 0; s < pass.light.Buffer.lightingAtlasBatches.tilemapList.Count; s++) {
                PartiallyBatchedTilemap batch_tilemap = pass.light.Buffer.lightingAtlasBatches.tilemapList[s];

                LightingTile tile = (LightingTile)pass.sortPass.sortObject.lightObject;
                
                WithoutAtlas.Tile.MaskSprite(tile, pass.layer, pass.materialWhite, batch_tilemap.polyOffset, batch_tilemap.tilemap, pass.lightSizeSquared, pass.z);
            }

            pass.light.Buffer.lightingAtlasBatches.tilemapList.Clear();

            Lighting2D.materials.GetAtlasMaterial().SetPass(0);
            GL.Begin(GL.TRIANGLES);
        }
    }
}