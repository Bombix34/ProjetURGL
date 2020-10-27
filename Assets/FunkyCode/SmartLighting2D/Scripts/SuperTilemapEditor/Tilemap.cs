using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

#if (SUPER_TILEMAP_EDITOR)

using CreativeSpore.SuperTilemapEditor;


    namespace SuperTilemapEditorSupport {

        public class Tilemap : LightingTilemapCollider.Base {
            public List<Polygon2D> colliders = new List<Polygon2D>();
            public CreativeSpore.SuperTilemapEditor.STETilemap tilemap;
            public bool eventsInit = false;

            public override MapType TilemapType()
            {
                return MapType.SuperTilemapEditor;
            }

            private void OnTileChanged(STETilemap tilemap, int gridX, int gridY, uint tileData) {  
                Initialize();
			
				LightingSource2D.ForceUpdateAll();
            }

            private void OnMeshUpdated(STETilemap source) {  
                Initialize();

                LightingSource2D.ForceUpdateAll();
            }

            public override void Initialize() {
                base.Initialize();

                tilemap = gameObject.transform.GetComponent<STETilemap>();

                if (tilemap == null) {
                    return;
                }

                if (eventsInit == false) {
                    eventsInit = true;

                    tilemap.OnTileChanged += OnTileChanged;

                    tilemap.OnMeshUpdated += OnMeshUpdated;
                }
                
                properties.cellSize = tilemap.CellSize;

                InitializeGrid();

                InitializeColliders();
            }

            public const uint k_TileFlag_FlipV = 0x80000000;
            public const uint k_TileFlag_FlipH = 0x40000000;
            
            public void InitializeGrid() {
                mapTiles.Clear();



                for (int x = tilemap.MinGridX; x <= tilemap.MaxGridX; x++) {
                    for (int y = tilemap.MinGridY; y <= tilemap.MaxGridY; y++) {
                        Tile tileSTE = tilemap.GetTile(x, y);
                        uint tileDataSTE = tilemap.GetTileData(x, y);
                        uint dataSTE = Tileset.GetTileFlagsFromTileData(tileDataSTE);


                        if (tileSTE == null) {
                            continue;
                        }

                        LightingTile tile = new LightingTile();
                        tile.gridPosition = new Vector2Int(x, y);
                        tile.uv = tileSTE.uv;

                        tile.flipX = (dataSTE & k_TileFlag_FlipH) != 0;
                        tile.flipY = (dataSTE & k_TileFlag_FlipV) != 0;

                        mapTiles.Add(tile);
                    }            
                }
            }
    
            public void InitializeColliders() {
                colliders.Clear();

                foreach(Transform t in gameObject.transform) {
                    foreach(Component c in t.GetComponents<EdgeCollider2D>()) {
                        Polygon2D poly = Polygon2DHelper.CreateFromEdgeCollider(c as EdgeCollider2D);
                        poly = poly.ToWorldSpace(t);
                        colliders.Add(poly);
                    }
                    foreach(Component c in t.GetComponents<PolygonCollider2D>()) {
                        Polygon2D poly = Polygon2DList.CreateFromPolygonColliderToWorldSpace(c as PolygonCollider2D)[0];
                        colliders.Add(poly);
                    }
                }			
            }
        }
    }

 #endif