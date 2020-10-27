using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light {

    public struct UVRect {
        public float x0;
        public float y0;
        public float x1;
        public float y1;

        public UVRect(float x0, float y0, float x1, float y1) {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }
    }

        
    public static class ShadowEngine {
        public static LightingSource2D light;
        public static Vector2 lightOffset = Vector2.zero;
        public static float lightSize = 0;
        public static bool lightDrawAbove = false;

        public static Vector2 objectOffset = Vector2.zero;

        public static bool perpendicularIntersection;
        public static LightingLayer effectLayer = LightingLayer.Layer1;

        // Layer Effect
        public static List<List<Polygon2D>> effectPolygons = new List<List<Polygon2D>>();

        // public static float shadowDistance;
        public static float shadowZ = 0;

        public static int drawMode = 0;
        
        public static void Draw(List<Polygon2D> polygons, float height) {
            switch(ShadowEngine.drawMode) {
                case 0:
                    Shadow.Legacy.Draw(polygons, height);
                break;
            
                case 1:
                    Shadow.Soft.Draw(polygons);
                break;

                case 2:
                    Shadow.PerpendicularIntersection.Draw(polygons, height);
                break;

            }
        }

        public static void SetPass(LightingSource2D lightObject, LayerSetting layer) {
            light = lightObject;
            lightSize = Mathf.Sqrt(light.size * light.size + light.size * light.size);
            lightOffset = -light.transform2D.position;

            effectLayer = layer.shadowEffectLayer;

            objectOffset = Vector2.zero;

            effectPolygons.Clear();
                
            if (layer.shadowEffect == LightingLayerShadowEffect.Projected) {
                drawMode = 2;

                GenerateEffectLayers();

            } else if (layer.shadowEffect == LightingLayerShadowEffect.Soft) {
                drawMode = 1;
                
            } else {
                drawMode = 0;
            }
        }

        public static void GenerateEffectLayers() {
            int layerID = (int)ShadowEngine.effectLayer;

            foreach(LightingCollider2D c in LightingCollider2D.GetEffectList((layerID))) {
                List<Polygon2D> polygons = c.mainShape.GetPolygonsWorld();

                if (polygons == null) {
                    continue;
                }
    
                if (c.InLightSource(light)) {
                    effectPolygons.Add(polygons);
                }
            }
        }
        
        public static void Prepare(LightingSource2D light) {
            FillWhite.Calculate();
            FillBlack.Calculate();

            Penumbra.Calculate();

            lightDrawAbove = light.whenInsideCollider == LightingSource2D.WhenInsideCollider.DrawAbove;
        }

      
        static public class Penumbra {
            static public UVRect uvRect = new UVRect();
            static public Vector2 size;
    
            static Sprite sprite = null;

            public static void Calculate() {
                LightingManager2D manager = LightingManager2D.Get();
                
                sprite = Lighting2D.materials.GetAtlasPenumbraSprite();

                if (sprite == null || sprite.texture == null) {
                    return;
                }

                Rect spriteRect = sprite.textureRect;
                int atlasSize = AtlasSystem.Manager.GetAtlasPage().atlasSize / 2;

                uvRect.x0 = spriteRect.x / sprite.texture.width;
                uvRect.y0 = spriteRect.y / sprite.texture.height;

                size.x = ((float)spriteRect.width) / sprite.texture.width;
                size.y = ((float)spriteRect.height) / sprite.texture.height;

                uvRect.x1 = spriteRect.width / 2 / sprite.texture.width;
                uvRect.y1 = spriteRect.height / 2 / sprite.texture.height;
                uvRect.x1 += uvRect.x0;
                uvRect.y1 += uvRect.y0;

                uvRect.x0 += 1f / atlasSize;
                uvRect.y0 += 1f / atlasSize;
                uvRect.x1 -= 1f / atlasSize;
                uvRect.y1 -= 1f / atlasSize;
            }
        }

        public class FillWhite {
            static public UVRect uvRect = new UVRect();

            static public void Calculate() {
                LightingManager2D manager = LightingManager2D.Get();
                
                Sprite fillSprite = Lighting2D.materials.GetAtlasWhiteMaskSprite();

                if (fillSprite != null) {
                    Rect spriteRect = fillSprite.textureRect;

                    uvRect.x0 = spriteRect.x / fillSprite.texture.width;
                    uvRect.y0 = spriteRect.y / fillSprite.texture.height;
                    uvRect.x1 = spriteRect.width / fillSprite.texture.width;
                    uvRect.y1 = spriteRect.height / fillSprite.texture.height;

                    uvRect.x0 += uvRect.x1 / 2;
                    uvRect.y0 += uvRect.x1 / 2;
                }
            }
        }

        public class FillBlack {
            static public UVRect uvRect = new UVRect();

            static public void Calculate() {
                LightingManager2D manager = LightingManager2D.Get();
                
                Sprite fillSprite = Lighting2D.materials.GetAtlasBlackMaskSprite();

                if (fillSprite != null) {
                    Rect spriteRect = fillSprite.textureRect;

                    uvRect.x0 = spriteRect.x / fillSprite.texture.width;
                    uvRect.y0 = spriteRect.y / fillSprite.texture.height;
                    uvRect.x1 = spriteRect.width / fillSprite.texture.width;
                    uvRect.y1 = spriteRect.height / fillSprite.texture.height;

                    uvRect.x0 += uvRect.x1 / 2;
                    uvRect.y0 += uvRect.x1 / 2;
                }
            }
        }   
    }
}