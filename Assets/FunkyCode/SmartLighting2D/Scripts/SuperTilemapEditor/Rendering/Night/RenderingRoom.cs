using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (SUPER_TILEMAP_EDITOR)

    namespace SuperTilemapEditorSupport {

        public class RenderingRoom {

            public static void DrawTiles(Camera camera, LightingTilemapRoom2D id, Material material, float z) {
                Vector2 cameraPosition = -camera.transform.position;

                float cameraRadius = CameraHelper.GetRadius(camera);

                if (id.superTilemapEditor.tilemap.Tileset != null) {
                    material.mainTexture = id.superTilemapEditor.tilemap.Tileset.AtlasTexture;
                }

                material.color = id.color;

                LightingTilemapCollider.Base tilemapCollider = id.GetCurrentTilemap();

                material.SetPass (0); 
                GL.Begin (GL.QUADS);

                foreach(LightingTile tile in id.superTilemapEditor.mapTiles) {
                    Vector2 tilePosition = tile.GetWorldPosition(tilemapCollider);

                    tilePosition += cameraPosition;

                    if (tile.NotInRange(tilePosition, cameraRadius)) {
                        continue;
                    }

                    Vector2 scale = tile.worldScale * 0.5f;

                    if (tile.flipX) {
                        scale.x = -scale.x;
                    }

                    if (tile.flipY) {
                        scale.y = -scale.y;
                    }
                
                    Rendering.Universal.WithoutAtlas.Texture.DrawPassSTE(tilePosition, scale, tile.uv, tile.worldRotation, z);
                }

                GL.End ();

                material.mainTexture = null;
                material.color = Color.white;
            }
        }
    }

#else 

    namespace SuperTilemapEditorSupport {
        public class RenderingRoom {
            public static void DrawTiles(Camera camera, LightingTilemapRoom2D id, Material material, float z) {}
        }
    }

#endif