using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

namespace Rendering.Day {

    public class Pass {
        
        public Sorting.SortList sortList = new Sorting.SortList();
        public Sorting.SortObject sortObject;
        public int layerId;
        public LightingLayerSetting layer;

        public Camera camera;
        public Vector2 offset;

        public float z = 0;

        public void SortObjects() {
            sortList.Reset();

            List<DayLightingCollider2D> colliderList = DayLightingCollider2D.GetList();
            for(int id = 0; id < colliderList.Count; id++) {
                DayLightingCollider2D collider = colliderList[id];

                if (collider.shadowLayer != layerId && collider.maskLayer != layerId) {
                    continue;
                }

                switch(layer.sorting) {
                    case LightingLayerSettingSorting.ZAxisDown:
                        sortList.Add((object)collider, Sorting.SortObject.Type.Collider, - collider.transform.position.z);
                    break;

                    case LightingLayerSettingSorting.ZAxisUp:
                        sortList.Add((object)collider, Sorting.SortObject.Type.Collider, collider.transform.position.z);
                    break;
                }

                switch(layer.sorting) {
                    case LightingLayerSettingSorting.YAxisDown:
                        sortList.Add((object)collider, Sorting.SortObject.Type.Collider, - collider.transform.position.y);
                    break;

                    case LightingLayerSettingSorting.YAxisUp:
                        sortList.Add((object)collider, Sorting.SortObject.Type.Collider, collider.transform.position.y);
                    break;
                }
            }

            sortList.Sort();
        }


        public bool Setup(LightingLayerSetting slayer, Camera camera) {
            if (slayer.layer < 0) {
                return(false);
            }
            layerId = (int)slayer.layer;
            layer = slayer;

            this.camera = camera;
            offset = -camera.transform.position;

            return(true);
        }
    }
}