using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LightingTilemapCollider {

	[System.Serializable]
    public class Hexagon : Base {
        public enum ShadowType {None, Grid, SpriteCustomPhysicsShape};
		public enum MaskType {None, Grid, Sprite, SpriteCustomPhysicsShape};
		
		public ShadowType shadowType = ShadowType.Grid;
		public MaskType maskType = MaskType.Sprite;

		public override MapType TilemapType() {
			return(MapType.UnityHexagon);
		}

        public override void Initialize() {
			base.Initialize();

			if (UpdateProperties() == false) {
				return;
			}

			mapTiles.Clear();

			Tilemap tilemap2D = properties.tilemap;

			ITilemap tilemap = (ITilemap) FormatterServices.GetUninitializedObject(typeof(ITilemap));
			typeof(ITilemap).GetField("m_Tilemap", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(tilemap, tilemap2D);

			foreach (Vector3Int position in tilemap2D.cellBounds.allPositionsWithin) {
				TileData tileData = new TileData();

				TileBase tilebase = tilemap2D.GetTile(position);

				if (tilebase != null) {
					tilebase.GetTileData(position, tilemap, ref tileData);

					LightingTile lightingTile = new LightingTile();
					lightingTile.gridPosition = new Vector2Int(position.x, position.y);
					lightingTile.SetOriginalSprite(tileData.sprite);
					lightingTile.GetCustomPhysicsShapePolygons();

					mapTiles.Add(lightingTile);
				}
			}
		}
    }
}