using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Tilemap {

    public class Isometric {

        static public Vector2 GetTilePosition(LightingTile tile, TilemapProperties properties) {
            Vector2 tilemapOffset = properties.transform.position;
            
            // Tile Offset
            Vector2 tileOffset = new Vector2(tile.gridPosition.x, tile.gridPosition.y);
            tileOffset.x += properties.cellAnchor.x;
            tileOffset.y += properties.cellAnchor.y;

            tileOffset.x += properties.cellGap.x * tile.gridPosition.x;
            tileOffset.y += properties.cellGap.y * tile.gridPosition.y;
            
            // Tile Position
            Vector2 tilePosition = tilemapOffset;
            
            tilePosition.x += tileOffset.x * 0.5f;
            tilePosition.x += tileOffset.y * -0.5f;
            tilePosition.x *= properties.cellSize.x;

            tilePosition.y += tileOffset.x * 0.5f * properties.cellSize.y;
            tilePosition.y += tileOffset.y * 0.5f * properties.cellSize.y;
            
            tilePosition.x *= properties.transform.lossyScale.x;
            tilePosition.y *= properties.transform.lossyScale.y;

            //tilePosition.x *= (properties.cellGap.x + properties.cellSize.x);
            //tilePosition.y *= (properties.cellGap.y + properties.cellSize.y);
            
            return(tilePosition);
        }
       
        public static Vector2 GetScale(TilemapProperties properties) {
            Vector2 scale = new Vector2();

            //  = id.GetTilemapProperties();

            scale.x = properties.transform.lossyScale.x; //properties.cellSize.x * 
            scale.y = properties.transform.lossyScale.y; // properties.cellSize.y *

            return(scale);
        }

    }

}