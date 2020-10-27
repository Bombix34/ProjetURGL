using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if (SUPER_TILEMAP_EDITOR)

    namespace SuperTilemapEditorSupport {

        public class TilemapTransform {

            static public Vector2 GetScale(TilemapProperties Properties) {
                Vector2 scale = Properties.transform.lossyScale;
                scale *= Properties.cellSize;
                return(scale);
            }

            static public Vector2 GetTilePosition(LightingTile tile, TilemapProperties Properties) {
                Transform transform = Properties.transform;

                float rotation = transform.eulerAngles.z * Mathf.Deg2Rad;

                Vector2 resultPosition = transform.position;

                Vector2 tilePosition = new Vector2(tile.gridPosition.x, tile.gridPosition.y);

                tilePosition.x *= Properties.cellSize.x;
                tilePosition.y *= Properties.cellSize.y;

                tilePosition.x += Properties.cellAnchor.x * Properties.cellSize.x;
                tilePosition.y += Properties.cellAnchor.y * Properties.cellSize.y;

                //tilePosition.x += Properties.area.position.x;
                //tilePosition.y += Properties.area.position.y;

                //tilePosition.x -= Properties.area.size.x / 2;
                //tilePosition.y -= Properties.area.size.y / 2;

                tilePosition.x *= transform.lossyScale.x;
                tilePosition.y *= transform.lossyScale.y;

                // Rotation
                float tileDirection = Mathf.Atan2(tilePosition.y, tilePosition.x) + rotation;
                float length = Mathf.Sqrt(tilePosition.x * tilePosition.x + tilePosition.y * tilePosition.y);

                tilePosition.x = Mathf.Cos(tileDirection) * length;
                tilePosition.y = Mathf.Sin(tileDirection) * length;

                resultPosition += tilePosition;
                
                return(resultPosition);
            }
        }
    }

 #endif