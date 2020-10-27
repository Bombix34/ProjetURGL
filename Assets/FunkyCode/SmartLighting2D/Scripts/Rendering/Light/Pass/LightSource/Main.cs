using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Light.LightSource {

    public class Main {

         public static void Draw(LightingSource2D light) {
            Vector2 size = new Vector2(light.size, light.size);
            float z = 0;

            Material material = Lighting2D.materials.GetMultiplyHDR();

            if (light != null) {
                UnityEngine.Sprite lightSprite = light.GetSprite();

                if (lightSprite.texture != null) {
                    material.mainTexture = lightSprite.texture;
                }
            }

            if (light.applyRotation) {
                Bounds.CalculatePoints();
                Bounds.CalculateOffsets();

                material.SetPass(0);

                material.color = Color.white;

                Sprite.Draw(Vector2.zero, size, light.transform.rotation.eulerAngles.z, z, light.spriteFlipX, light.spriteFlipY);

                material.color = Color.black;

                Bounds.Draw(light, material, z);
                
            } else {
                material.SetPass (0); 

                material.color = Color.white;

                Sprite.Draw(Vector2.zero, size, 0, z, light.spriteFlipX, light.spriteFlipY);
            }

            if (light.spotAngle != 360) {
                Lighting2D.materials.GetAtlasMaterial().SetPass(0);

                GL.Begin(GL.TRIANGLES);

                GL.Color(Color.black);

                Angle.Draw(light, z);

                GL.End ();
            }
        }
    }
}