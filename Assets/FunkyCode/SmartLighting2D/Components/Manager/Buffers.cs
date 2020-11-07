using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Buffers : MonoBehaviour {
    static private Buffers instance;

	bool renderObject = false;

	public void Awake() {
		foreach(OnRenderMode onRenderMode in Object.FindObjectsOfType(typeof(OnRenderMode))) {
			onRenderMode.DestroySelf();
		}

		//foreach(LightingBuffer2D buffer in Object.FindObjectsOfType(typeof(LightingBuffer2D))) {
		//	buffer.DestroySelf();
		//}

		LightingBuffer2D.Clear();

		UpdateFlags();
	}

	private void OnDestroy() {
		instance = null;
	}

    static public Buffers Get() {
		if (instance != null) {
			if (instance.transform != null) {
				return(instance);
			} else {
				instance = null;
			}			
		}

		foreach(Buffers root in Object.FindObjectsOfType(typeof(Buffers))) {
			instance = root;

			return(instance);
		}


		LightingManager2D manager = LightingManager2D.Get();

		GameObject gameObject = new GameObject ();
		gameObject.transform.parent = manager.transform;
		gameObject.name = "Buffers";

		instance = gameObject.AddComponent<Buffers> ();
		instance.GetCamera();
		instance.UpdateFlags();

		return(instance);
	}

	void UpdateFlags() {
		if (Lighting2D.Profile.qualitySettings.managerInternal == LightingSettings.QualitySettings.ManagerInternal.HideInHierarchy) {
			gameObject.hideFlags = HideFlags.HideInHierarchy;
		} else {
			gameObject.hideFlags = HideFlags.None;
		}
	}

	private Camera lightingCamera = null;

	private void OnEnable() {
		SetUpCamera();
	}

	public Camera GetCamera() {
		if (lightingCamera == null) {
			lightingCamera = gameObject.GetComponent<Camera>();

			if (lightingCamera == null) {
				lightingCamera = gameObject.AddComponent<Camera>();
				SetUpCamera();
			}
		}

		return(lightingCamera);
	}

	#if UNITY_EDITOR
		private void Update() {
			LightingManager2D manager = LightingManager2D.Get();

			if (manager != null) {
				gameObject.layer = Lighting2D.ProjectSettings.sceneView.layer;
			}
		}
	#endif

	private void LateUpdate() {
		renderObject = true;
	}

    private void OnPreCull() {
		if (Lighting2D.disable) {
			return;
		}

        if (Lighting2D.Profile.qualitySettings.updateMethod != LightingSettings.QualitySettings.UpdateMethod.OnPreCull) {
			return;
        }
		
		LightingManager2D manager = LightingManager2D.Get();
		manager.RenderLoop();
    }

	private void OnRenderObject() {
		if (Lighting2D.disable) {
			return;
		}
		
        if (Lighting2D.Profile.qualitySettings.updateMethod != LightingSettings.QualitySettings.UpdateMethod.OnRenderObject) {
			return;
        }

		if (renderObject == false) {
			return;
		}

		renderObject = false;

		LightingManager2D manager = LightingManager2D.Get();
		manager.RenderLoop();
    }

	void SetUpCamera() {
		if (lightingCamera == null) {
			return;
		}

		lightingCamera.clearFlags = CameraClearFlags.Nothing;
		lightingCamera.backgroundColor = Color.white;
		lightingCamera.cameraType = CameraType.Game;
		lightingCamera.orthographic = true;
		lightingCamera.farClipPlane = 0.01f;
		lightingCamera.nearClipPlane = -0.01f;
		lightingCamera.allowHDR = false;
		lightingCamera.allowMSAA = false;
		lightingCamera.enabled = false;
		lightingCamera.depth = -50;
		lightingCamera.orthographicSize = Camera.main.orthographicSize;
	}
























	
    // Management
	static public LightingBuffer2D AddBuffer(int textureSize, LightingSource2D light) {
		Get();

        if (Lighting2D.Profile.qualitySettings.lightTextureSize != LightingSettings.LightingSourceTextureSize.Custom) {
            textureSize = LightingRender2D.GetTextureSize(Lighting2D.Profile.qualitySettings.lightTextureSize);
        }

		LightingBuffer2D lightingBuffer2D = new LightingBuffer2D ();
		lightingBuffer2D.Initiate (textureSize);
		lightingBuffer2D.Light = light; // Unnecessary?

		return(lightingBuffer2D);
	}

	static public LightingBuffer2D PullBuffer(int textureSize, LightingSource2D light) {
		Get();

        if (Lighting2D.Profile.qualitySettings.lightTextureSize != LightingSettings.LightingSourceTextureSize.Custom) {
            textureSize = LightingRender2D.GetTextureSize(Lighting2D.Profile.qualitySettings.lightTextureSize);
        }

		foreach (LightingBuffer2D id in LightingBuffer2D.GetList()) {
			if (id.Free && id.renderTexture.width == textureSize) {
				id.Light = light;

				light.ForceUpdate();
				
				return(id);
			}
		}
			
		return(AddBuffer(textureSize, light));		
	}

    static public void FreeBuffer(LightingBuffer2D buffer) {
        if (buffer == null) {
            return;
        }

		if (buffer.Light != null) {
			buffer.Light.Buffer = null;

			buffer.Light = null;
		}

		buffer.updateNeeded = false;
	}
}
