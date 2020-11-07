using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightingSettings;

namespace Rendering {

	public class FogOfWarBuffer {
		public class Check {
			static public void RenderTexture(FogOfWarBuffer2D buffer) {
                Vector2Int screen = buffer.GetScreen();

                if (screen.x > 0 && screen.y > 0) {
                    Camera camera = buffer.cameraSettings.GetCamera();

                    if (buffer.renderTexture == null || screen.x != buffer.renderTexture.width || screen.y != buffer.renderTexture.height) {

                        switch(camera.cameraType) {
                            case CameraType.Game:
                                buffer.SetUpRenderTexture();
                            
                            break;

                            case CameraType.SceneView:
                                // Scene view pixel rect is constantly changing (Unity Bug?)
                                int differenceX = Mathf.Abs(screen.x - buffer.renderTexture.width);
                                int differenceY = Mathf.Abs(screen.y - buffer.renderTexture.height);
                                
                                if (differenceX > 5 || differenceY > 5) {
                                    buffer.SetUpRenderTexture();
                                }
                            
                            break;

                        }
                    }
                }
            }
		}
		public static void LateUpdate(FogOfWarBuffer2D buffer) {

			if (buffer.CameraSettingsCheck() == false) {
				buffer.DestroySelf();
				return;
			}

			Camera camera = buffer.cameraSettings.GetCamera();

			if (camera == null) {
				return;
			}

			buffer.transform.position = new Vector3(0, 0, 0);
			buffer.transform.rotation = Quaternion.Euler(0, 0, 0);

			if (Lighting2D.fogOfWar.enabled == false) {
				buffer.DestroySelf();

				return;
			}

		}

		public static void DrawOn(FogOfWarBuffer2D buffer) {
			if (Lighting2D.fogOfWar.enabled == false) {
				return;
			}

			switch(Lighting2D.renderingMode) {
				case RenderingMode.OnRender:
					FogOfWarRender.OnRender(buffer);
	
				break;

				case RenderingMode.OnPreRender:
					FogOfWarRender.PreRender(buffer);
				break;
			}
		}

		static public void Render(FogOfWarBuffer2D buffer) {
			Camera camera = buffer.cameraSettings.GetCamera();

			if (camera == null) {
				return;
			}

			bool draw = true;

			if (Lighting2D.fogOfWar.useOnlyInPlay) {
            	if (Application.isPlaying == false) {
					draw = false;
				}
			}

			float sizeY = camera.orthographicSize;
			float sizeX = sizeY * ( (float)camera.pixelWidth / camera.pixelHeight );

			GL.PushMatrix();
			GL.LoadPixelMatrix( -sizeX, sizeX, -sizeY, sizeY );

			if (draw) {
				if (Lighting2D.Profile.fogOfWar.sorting == LightingSettings.FogOfWar.Sorting.None) {
					FogOfWar.NoSort.Draw(camera);
				} else {
					FogOfWar.Sorted.Draw(camera);
				}
			}

			GL.PopMatrix();
		}
	}
}