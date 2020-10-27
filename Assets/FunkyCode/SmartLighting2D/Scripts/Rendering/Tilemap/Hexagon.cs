using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Tilemap {
    
   public class Hexagon {

        static public Vector2 GetTilePosition(LightingTile tile, TilemapProperties properties) {
            Vector2 resultPosition = properties.transform.position;

            Vector2 tilePosition = new Vector2(tile.gridPosition.x + tile.gridPosition.y / 2, tile.gridPosition.y);
            tilePosition.x += properties.cellAnchor.x;
            tilePosition.y += properties.cellAnchor.y;

            tilePosition.x = tilePosition.x + tilePosition.y * -0.5f;
            tilePosition.y = tilePosition.y * 0.75f;

            tilePosition.x *= properties.transform.lossyScale.x;
            tilePosition.y *= properties.transform.lossyScale.y;

            resultPosition += tilePosition;

            return(resultPosition);
        }

        public static Vector2 GetScale(TilemapProperties properties) {
            Vector2 scale = new Vector2();

            scale.x = properties.cellSize.x * properties.transform.lossyScale.x;
            scale.y = properties.cellSize.y * properties.transform.lossyScale.y;

            return(scale);
        }

    }

}