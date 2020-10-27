using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering {
    public class LightingBuffer {

        static public void Render(LightingSource2D light) {
			float size = light.size;

			GL.PushMatrix();

			GL.LoadPixelMatrix( -size, size, -size, size );

			Rendering.Light.Main.Draw(light);

			GL.PopMatrix();
		}

        static public void UpdateName(LightingBuffer2D buffer) {
            string freeString = "";

            if (buffer.Free) {
                freeString = "free";
            } else {
                freeString = "taken";
            }

            if (buffer.renderTexture != null) {
                    
                buffer.name = "Buffer (Id: " + (LightingBuffer2D.GetList().IndexOf(buffer) + 1) + ", Size: " + buffer.renderTexture.width + ", " + freeString + ")";

            } else {
                buffer.name = "Buffer (Id: " + (LightingBuffer2D.GetList().IndexOf(buffer) + 1) + ", No Texture, " + freeString + ")";

            }
           
            if (Lighting2D.commonSettings.HDR) {
                buffer.name = "HDR " + buffer.name;
            }
        }

        static public void InitializeRenderTexture(LightingBuffer2D buffer, int textureSize) {
            RenderTextureFormat format = RenderTextureFormat.Default;
            format = RenderTextureFormat.R8; 

            if (Lighting2D.commonSettings.HDR) {
                format = RenderTextureFormat.DefaultHDR;
                format = RenderTextureFormat.RHalf;
            }

            buffer.renderTexture = new LightTexture(textureSize, textureSize, 0, format);
            buffer.renderTexture.renderTexture.filterMode = Lighting2D.Profile.qualitySettings.lightFilterMode;

            UpdateName(buffer);
        }
    }
}