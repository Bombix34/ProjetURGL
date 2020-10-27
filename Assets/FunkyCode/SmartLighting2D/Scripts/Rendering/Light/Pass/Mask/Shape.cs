using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light {
    
    public class Shape {

        public static void Mask(LightingSource2D light, LightingCollider2D id, LayerSetting layerSetting, float z) {
            if (id.InLightSource(light) == false) {
                return;
            }

            foreach(LightingColliderShape shape in id.shapes) {
                List<MeshObject> meshObjects = shape.GetMeshes();

                if (meshObjects == null) {
                    return;
                }
                            
                Vector2 position = shape.transform2D.position - light.transform2D.position;
                GL.Color(LayerSettingColor.Get(position, layerSetting, id.maskEffect));

                GLExtended.DrawMeshPass(meshObjects, position, shape.transform.lossyScale, shape.transform2D.rotation);
            }
        }
    }
}