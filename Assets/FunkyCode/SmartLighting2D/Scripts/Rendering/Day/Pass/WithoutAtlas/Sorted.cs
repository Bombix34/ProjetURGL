using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Day.WithoutAtlas {

    public class Sorted {
        static public void Draw(Pass pass) {
            for(int id = 0; id < pass.sortList.count; id++) {
                Sorting.SortObject sortObject = pass.sortList.list[id];

                switch(sortObject.type) {
                    case Sorting.SortObject.Type.Collider:
                        DayLightingCollider2D collider = (DayLightingCollider2D)sortObject.lightObject;

                        if (collider != null) {


                            if (collider.mainShape.shadowType == DayLightingCollider2D.ShadowType.Collider || collider.mainShape.shadowType == DayLightingCollider2D.ShadowType.SpriteCustomPhysicsShape) {
                                Lighting2D.materials.GetShadowBlur().SetPass (0);
                                GL.Begin(GL.TRIANGLES);
                                WithoutAtlas.Shadow.Draw(collider, pass.offset, pass.z);  
                                GL.End(); 
                            }

                            WithoutAtlas.SpriteRendererShadow.Draw(collider, pass.offset, pass.z);
                
                            WithoutAtlas.SpriteRenderer2D.Draw(collider, pass.offset, pass.z);

                        }

                    break;
                }
            }
        }
    }
}
