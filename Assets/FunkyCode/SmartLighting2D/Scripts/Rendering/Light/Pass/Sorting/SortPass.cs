using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingTilemapCollider;

namespace Rendering.Light.Sorting {

    public class SortPass {

        public Sorting.SortList sortList = new Sorting.SortList();
        public Sorting.SortObject sortObject;
        public Rendering.Light.Pass pass;

        public void Clear() {
            sortList.count = 0;
        }
        
        public void SortObjects() {
            if (pass == null) {
                return;
            }
            
            sortList.Reset();

            for(int id = 0; id < pass.colliderList.Count; id++) {
                LightingCollider2D collider = pass.colliderList[id]; // Check If It's In Light Area?

                if (collider.shadowLayer != pass.layerID && collider.maskLayer != pass.layerID) {
                    continue;
                }

                if (collider.InLightSource(pass.light) == false) {
                    continue;
                }

                switch(pass.layer.sorting) {
                    case LightingLayerSorting.ZAxisDown:
                        if (pass.layer.sortingIgnore == LightingLayerSortingIgnore.IgnoreAbove) {
                            if (collider.transform.position.z >= pass.light.transform.position.z) {
                                sortList.Add(collider, -collider.transform.position.z);
                            }
                        } else {
                            sortList.Add(collider, -collider.transform.position.z);
                        }
                        
                        
                    break;

                    case LightingLayerSorting.ZAxisUp:
                        if (pass.layer.sortingIgnore == LightingLayerSortingIgnore.IgnoreAbove) {
                            if (collider.transform.position.z <= pass.light.transform.position.z) {
                                sortList.Add(collider, collider.transform.position.z);
                            }
                        } else {
                            sortList.Add(collider, collider.transform.position.z);
                        }

                    break;

                    case LightingLayerSorting.YAxisDown:
                        sortList.Add(collider, -collider.transform.position.y);
                    break;

                    case LightingLayerSorting.YAxisUp:
                        sortList.Add(collider, collider.transform.position.y);
                    break;

                    case LightingLayerSorting.DistanceToLight:
                        sortList.Add(collider, -Vector2.Distance(collider.transform.position, pass.light.transform.position));
                    break;
                }
            }

            #if UNITY_2017_4_OR_NEWER

                for(int id = 0; id < pass.tilemapList.Count; id++) {
                    LightingTilemapCollider2D tilemap = pass.tilemapList[id];

                    if (tilemap.IsNotInRange(pass.light)) {
                        continue;
                    }

                    if (tilemap.mapType == MapType.UnityRectangle) {
                        AddTiles(tilemap);
                    } else {
                        AddTileMap(tilemap);
                    }
                }

            #endif

            sortList.Sort();
        }
     
        #if UNITY_2017_4_OR_NEWER

            public void AddTileMap(LightingTilemapCollider2D id) {
                switch(pass.layer.sorting) {
                        case LightingLayerSorting.ZAxisDown:
                            sortList.AddTilemap(id, -id.transform.position.z);
                        break;

                        case LightingLayerSorting.ZAxisUp:
                            sortList.AddTilemap(id, id.transform.position.z);
                        break;
                        
                        case LightingLayerSorting.YAxisDown:
                            sortList.AddTilemap(id, -id.transform.position.y);
                        break;

                        case LightingLayerSorting.YAxisUp:
                            sortList.AddTilemap(id, id.transform.position.y);
                        break;

                        case LightingLayerSorting.DistanceToLight:
                            sortList.AddTilemap(id,  -Vector2.Distance(id.transform.position, pass.light.transform.position));
                        break;
                    
                    }	
            }

            public void AddTiles(LightingTilemapCollider2D id) {
                Vector2 lightPosition = - pass.light.transform2D.position;

                foreach(LightingTile tile in id.GetTileList()) {
                    if (tile.GetOriginalSprite() == null) {
                        return;
                    }

                    Vector2 tilePosition = tile.GetWorldPosition();

                    if (tile.NotInRange(tilePosition + lightPosition, pass.light.size)) {
                       continue;
                    }

                    switch(pass.layer.sorting) {
                        case LightingLayerSorting.ZAxisDown:
                            sortList.Add(id, tile, -id.transform.position.z);
                        break;

                        case LightingLayerSorting.ZAxisUp:
                            sortList.Add(id, tile, id.transform.position.z);
                        break;
                        
                        case LightingLayerSorting.YAxisDown:
                            sortList.Add(id, tile, -tilePosition.y);
                        break;

                        case LightingLayerSorting.YAxisUp:
                            sortList.Add(id, tile, tilePosition.y);
                        break;

                        case LightingLayerSorting.DistanceToLight:
                            sortList.Add(id, tile,  -Vector2.Distance(tilePosition, pass.light.transform.position));
                        break;
                    
                    }	
                }
            }
        
        #endif
    }

}