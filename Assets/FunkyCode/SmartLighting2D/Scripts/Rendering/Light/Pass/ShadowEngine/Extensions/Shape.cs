using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.Shadow {

    public class Shape {

        public static void Draw(LightingSource2D light, LightingCollider2D id) {
            if (id.InLightSource(light) == false) {
                return;
            }

            light.AddCollider(id);

            foreach(LightingColliderShape shape in id.shapes) {
                List<Polygon2D> polygons = shape.GetPolygonsWorld();
                
                ShadowEngine.Draw(polygons, shape.shadowDistance);
            }
        }
    }
}