using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Night.WithAtlas {

    public class NoSort {
        
        public static void Draw(Pass pass) {
            Rendering.Night.WithAtlas.SpriteRenderer.Draw(pass.camera, pass.z, pass.layerId);

            foreach (LightingSource2D id in LightingSource2D.GetList()) {
                if ((int)id.nightLayer != pass.layerId) {
                    continue;
                }

               Rendering.Night.LightSource.Draw(id, pass.camera, pass.z);
            }
        }
    }
}