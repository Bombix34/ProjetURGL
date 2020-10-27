﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.Shadow {

    public class TilemapCollider {

        public class Rectangle {
            static public void Draw(LightingSource2D light, LightingTilemapCollider2D id) {
                Vector2 position = -light.transform.position;

                switch(id.rectangle.colliderType) {
                    case LightingTilemapCollider.Rectangle.ColliderType.CompositeCollider:
                        ShadowEngine.objectOffset = id.transform.position;

                        ShadowEngine.Draw(id.rectangle.compositeColliders, 0);

                        ShadowEngine.objectOffset = Vector2.zero;
                    break;
                }
            }
        }
    }
}